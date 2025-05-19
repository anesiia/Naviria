namespace NaviriaAPI.Helpers
{
    public static class JsonHelper
    {
        public static string ExtractJsonFromCodeBlock(string input)
        {
            const string codeBlockStart = "```json";
            const string codeBlockEnd = "```";

            if (input.Contains(codeBlockStart))
            {
                var start = input.IndexOf(codeBlockStart) + codeBlockStart.Length;
                var end = input.IndexOf(codeBlockEnd, start);
                if (end > start)
                {
                    return input.Substring(start, end - start).Trim();
                }
            }

            return input;
        }
    }
}
