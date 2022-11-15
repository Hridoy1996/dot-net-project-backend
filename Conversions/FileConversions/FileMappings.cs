namespace Conversions.FileConversions
{
    public static class FileMappings
    {
        public static string GetContentType(string fileExtension)
        {
            if (FileExtensionMapping.ContainsKey(fileExtension))
            {
                return FileExtensionMapping[fileExtension];
            }

            return string.Empty;
        }

        public static Dictionary<string, string> FileExtensionMapping = new Dictionary<string, string>()
                {
                    {"pdf", "application/pdf"},
                    {"jpeg", "image/jpeg"},
                    {"jpg", "image/jpeg"},
                    {"png", "image/png"},
                    {"txt", "text/plain"},
                    {"csv", "text/csv"},
                    {"mp3", "audio/mpeg"},
                    {"mp4", "video/mp4"}
                };
    }
}
