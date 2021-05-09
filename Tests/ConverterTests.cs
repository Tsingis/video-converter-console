using Xunit;
using System.IO;
using VideoConverter;

namespace Tests
{
    public class ConverterTests
    {
        [Theory]
        [InlineData("Testvideos/example.mp4", VideoFormats.Webm)]
        [InlineData("Testvideos/example2.webm", VideoFormats.Mp4)]
        public void ConversionIsSuccessful(string file, string format)
        {
            Converter.ConvertVideoFromFile(file, format).Wait();

            var outFile = Path.ChangeExtension(file, format);
            var fileExists = File.Exists(outFile);

            Assert.True(fileExists);
            File.Delete(outFile);
        }

        [Theory]
        [InlineData("mp4", true)]
        [InlineData("webm", true)]
        [InlineData("gif", true)]
        [InlineData("wmv", false)]
        public void SupportedVideoFormats(string format, bool isSupported)
        {
            Assert.Equal(isSupported, VideoFormats.IsSupportedVideoFormat(format));
        }
    }
}