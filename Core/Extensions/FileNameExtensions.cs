namespace Core.Extensions
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class FileNameExtensions
    {
        public static string ToNormalizedFileName(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;

            string normalized = fileName.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            }

            normalized = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

            normalized = Regex.Replace(normalized, @"\s+", "_");

            normalized = Regex.Replace(normalized, @"[^a-z0-9_\-\.]", "");

            return normalized;
        }
    }
}