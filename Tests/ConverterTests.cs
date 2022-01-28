using System;
using System.IO;
using System.Threading.Tasks;
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
        public async Task ConversionIsSuccessful(string inputFile, string outputFormat)
        {
            var inputFilePath = Path.Join(TestVideoPath, inputFile);
            var outputFileDir = Path.Join(Environment.CurrentDirectory, TestVideoPath);

            var outputFilePath = await Converter.ConvertVideoAsync(inputFilePath, outputFileDir, outputFormat);
            var fileExists = File.Exists(outputFilePath);

            Assert.True(fileExists);
            File.Delete(outputFilePath);
        }

        [Fact]
        public async Task DownloadIsSuccessful()
        {
            var url = "https://tinyurl.com/yw6vak3d";
            var downloadedFile = await Utility.DownloadFileAsync(url);

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