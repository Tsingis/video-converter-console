using System;
using System.IO;
using Xunit;
using VideoConverter;

namespace Tests
{
    public class ConverterTests
    {
        private readonly string TestVideoPath = "Testvideos";

        [Theory]
        [InlineData("example.mp4", VideoFormat.Webm)]
        [InlineData("example2.webm", VideoFormat.Mp4)]
        public void ConversionIsSuccessful(string inputFile, string outputFormat)
        {
            var inputFilePath = Path.Join(TestVideoPath, inputFile);
            var outputFilePath = Path.Join(Environment.CurrentDirectory, TestVideoPath);

            var converter = new Converter(outputFilePath);
            converter.ConvertVideoAsync(inputFilePath, outputFormat).Wait();

            var outFile = Path.ChangeExtension(inputFilePath, outputFormat);
            var fileExists = File.Exists(outFile);

            Assert.True(fileExists);
            File.Delete(outFile);
        }

        [Fact]
        public void DownloadIsSuccessful()
        {
            var url = "https://tinyurl.com/yw6vak3d";
            var downloadedFile = Utility.DownloadFileAsync(url).Result;

            var fileExists = File.Exists(downloadedFile);

            Assert.True(fileExists);
            File.Delete(downloadedFile);
        }

        [Theory]
        [InlineData("mp4", true)]
        [InlineData("webm", true)]
        [InlineData("gif", true)]
        [InlineData("wmv", false)]
        public void IsSupportedVideoFormat(string format, bool isSupported)
        {
            Assert.Equal(isSupported, VideoFormat.IsSupportedVideoFormat(format));
        }
    }
}