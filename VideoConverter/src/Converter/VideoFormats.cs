namespace VideoConverter
{
    public static class VideoFormats
    {
        public const string Mp4 = "mp4";
        public const string Webm = "webm";
        public const string Gif = "gif";

        public static bool IsSupportedVideoFormat(string format)
        {
            var allowedFormats = typeof(VideoFormats).GetAllPublicConstantsValues<string>();
            return allowedFormats.Contains(format);
        }
    }
}