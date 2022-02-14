using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using VideoConverter;

namespace Tests
{
    public class ConverterTests
    {
        const string TestVideoPath = "Testvideos";

        [Theory]
        [InlineData("example.mp4", VideoFormat.Webm)]
        [InlineData("example2.webm", VideoFormat.Mp4)]
        public async Task TestConversion(string inputFile, string outputFormat)
        {
            var inputFilePath = Path.Join(TestVideoPath, inputFile);
            var outputFileDir = Path.Join(Environment.CurrentDirectory, TestVideoPath);

            var outputFilePath = await Converter.ConvertVideoAsync(inputFilePath, outputFileDir, outputFormat);

            Assert.True(File.Exists(outputFilePath));
            File.Delete(outputFilePath);
        }

        [Theory]
        [InlineData("mp4", true)]
        [InlineData("webm", true)]
        [InlineData("gif", true)]
        [InlineData("wmv", false)]
        public void TestSupportedVideoFormat(string format, bool isSupported)
        {
            Assert.Equal(isSupported, VideoFormat.IsSupportedVideoFormat(format));
        }
    }
}