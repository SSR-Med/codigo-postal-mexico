namespace Api.Binders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public class SnakeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var modelType = context.ModelType;
            var modelInstance = Activator.CreateInstance(modelType);
            var props = modelType.GetProperties();

            var request = context.ActionContext.HttpContext.Request;
            var hasForm = request.HasFormContentType;
            var form = hasForm ? request.Form : null;

            foreach (var prop in props)
            {
                var snakeKey = ToSnakeCase(prop.Name);

                if (typeof(IFormFile).IsAssignableFrom(prop.PropertyType) && hasForm)
                {
                    var file = form?.Files.FirstOrDefault(f =>
                        string.Equals(ToSnakeCase(f.Name), snakeKey, StringComparison.OrdinalIgnoreCase));

                    if (file != null) prop.SetValue(modelInstance, file);

                    continue;
                }

                var valueResult = context.ValueProvider.GetValue(snakeKey);

                if (valueResult == ValueProviderResult.None) continue;

                try
                {
                    var typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
                    var convertedValue = typeConverter.ConvertFromString(valueResult.FirstValue!);
                    prop.SetValue(modelInstance, convertedValue);
                }
                catch
                {
                    context.ModelState.AddModelError(prop.Name, $"Invalid value for {prop.Name}");
                }
            }

            context.Result = ModelBindingResult.Success(modelInstance);
            return Task.CompletedTask;
        }

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}