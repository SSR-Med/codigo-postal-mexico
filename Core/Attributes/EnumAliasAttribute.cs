namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EnumAliasAttribute(string alias) : Attribute
    {
        public string Alias { get; } = alias;
    }
}