using System.IO;
using System.Threading.Tasks;
using Xunit;
using VideoConverter;

namespace Tests
{
    public class UtilityTests
    {
        [Fact]
        public async Task TestDownload()
        {
            var url = "https://tinyurl.com/yw6vak3d";
            var downloadedFile = await Utility.DownloadFileAsync(url);

            Assert.True(File.Exists(downloadedFile));
            File.Delete(downloadedFile);
        }

        [Theory]
        [InlineData("https://localhost", true)]
        [InlineData("https://localhost.com", true)]
        [InlineData("https://www.localhost.com", true)]
        [InlineData("http://localhost", true)]
        [InlineData("http://localhost.com", true)]
        [InlineData("http://www.localhost.com", true)]
        [InlineData("www.localhost.com", false)]
        [InlineData("localhost.com", false)]
        [InlineData("localhost", false)]
        [InlineData(@"C:\Users\User\Videos", false)]
        public void TestValidUrl(string url, bool isValid)
        {
            Assert.Equal(isValid, Utility.IsValidUrl(url));
        }
    }
}