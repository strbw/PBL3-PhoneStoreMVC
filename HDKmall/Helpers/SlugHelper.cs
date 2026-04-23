using System.Text.RegularExpressions;
using System.Text;

namespace HDKmall.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return "";

            string str = phrase.ToLower();
            
            // Remove diacritics (Vietnamese characters)
            str = RemoveDiacritics(str);

            // Invalid chars
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            
            // Convert multiple spaces into one space
            str = Regex.Replace(str, @"\s+", " ").Trim();
            
            // Cut and trim
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            
            // Hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
