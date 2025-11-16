namespace Api.Filters
{
    using Core.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException validationEx:
                    HandleValidationException(context, validationEx);
                    break;
                case NotFoundException notFoundEx:
                    HandleNotFoundException(context, notFoundEx);
                    break;
                case NoContentException:
                    HandleNoContentException(context);
                    break;
                case BadRequestException badRequestEx:
                    HandleBadRequestException(context, badRequestEx);
                    break;
                case ConflictException conflictException:
                    HandleConflictException(context, conflictException);
                    break;
                default:
                    HandleUnknownException(context);
                    break;
            }

            base.OnException(context);
        }

        #region Private Method
        private static void HandleValidationException(ExceptionContext context,ValidationException exception)
        {
            var details = new ValidationProblemDetails(exception.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleConflictException(ExceptionContext context, ConflictException exception)
        {
            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                Title = "A conflict occurred with the current state of the resource.",
                Detail = exception.Message
            };

            context.Result = new ConflictObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleNotFoundException(ExceptionContext context, NotFoundException exception)
        {
            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleNoContentException(ExceptionContext context)
        {
            context.Result = new NoContentResult();

            context.ExceptionHandled = true;
        }

        private static void HandleBadRequestException(ExceptionContext context, BadRequestException exception)
        {
            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleUnknownException(ExceptionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = $"Exception => {context.Exception.Message} || StackTrace => {context.Exception.StackTrace} || InnerException {context.Exception.InnerException}."
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
        #endregion
    }
}