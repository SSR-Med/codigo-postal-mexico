namespace Core.Extensions
{
    public class IncludeExtension
    {
        public static FluentIncludeExtension<T> Include<T>() => new();
    }
}