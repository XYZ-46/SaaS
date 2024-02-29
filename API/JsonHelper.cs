namespace API
{
    public static class JsonHelper
    {
        public static string RemoveJsonWhitespace(this string input)
        {
            return new string(input.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
    }
}
