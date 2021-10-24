using System;
using Xunit;
using System.IO;
using VideoConverter;

namespace Tests
{
    public class ConverterTests
    {
        [Theory]
        [InlineData("Testvideos/example.mp4", VideoFormat.Webm)]
        [InlineData("Testvideos/example2.webm", VideoFormat.Mp4)]
        public void ConversionIsSuccessful(string file, string format)
        {
            var path = Path.Join(Environment.CurrentDirectory, "Testvideos");
            var converter = new Converter(path);
            converter.ConvertVideo(file, format).Wait();

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
            Assert.Equal(isSupported, VideoFormat.IsSupportedVideoFormat(format));
        }
    }
}