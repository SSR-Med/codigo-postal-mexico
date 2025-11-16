namespace Api.Policies
{
    using System.Text.Json;
    using System.Text.RegularExpressions;

    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => Regex.Replace(name, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}