namespace Rex2
{
    internal static class Utils
    {
        public static string SubTextAnimation(this string input, int position, int length)
        {
            return input.Substring(position, Math.Min(length, input.Length));
        }

        public static string Rot13TextAnimation(this string input, int position, int length)
        {
            if (length < input.Length)
            {
                var l = Math.Min(length, input.Length);
                return input.Substring(position, l) + input.Rot13().Substring(l);
            }
            else
                return input;
        }

        public static string Rot13(this string input)
        {
            return !string.IsNullOrEmpty(input) ? new string(input.ToCharArray().Select(s => { return (char)((s >= 97 && s <= 122) ? ((s + 13 > 122) ? s - 13 : s + 13) : (s >= 65 && s <= 90 ? (s + 13 > 90 ? s - 13 : s + 13) : s)); }).ToArray()) : input;
        }
    }
}