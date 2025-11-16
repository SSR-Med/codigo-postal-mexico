namespace Api.Binders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class SnakeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var bindingSource = context.Metadata.BindingSource;

            if (context.Metadata.IsComplexType && bindingSource != BindingSource.Body && bindingSource != BindingSource.Services) return new SnakeModelBinder();

            return null;
        }
    }
}