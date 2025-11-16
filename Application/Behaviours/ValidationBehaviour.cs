namespace Application.Behaviours
{
    using CoreException = Core.Exceptions;
    using FluentValidation;
    using FluentValidation.Results;
    using MediatR;

    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (validators.Any())
            {
                ValidationContext<TRequest> context = new(request);
                ValidationResult[] validationResults = await Task.WhenAll(
                    validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken)));
                List<ValidationFailure> failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();
                
                if (failures.Any())
                {
                    var errors = failures
                        .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                        .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

                    throw new CoreException.ValidationException("One or more validation errors have occurred.", errors);
                }
            }

            return await next();
        }
    }
}