using System.IO;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using VideoConverter;

namespace Tests;

public class UtilityTests
{
    [Theory]
    [InlineData("https://github.com/Tsingis/video-converter-console/raw/main/Tests/Testvideos/example.mp4")]
    [InlineData("https://github.com/Tsingis/video-converter-console/raw/main/Tests/Testvideos/example2.webm")]

    public async Task TestDownload(string url)
    {
        var downloadedFile = await Utility.DownloadFileAsync(url);
        File.Exists(downloadedFile).Should().BeTrue();
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
        var result = Utility.IsValidUrl(url);
        result.Should().Be(isValid);
    }
}