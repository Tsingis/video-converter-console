using System.Net;
using Moq;
using Moq.Protected;
using VideoConverter;
using Xunit;

namespace Tests;

public class UtilityTests
{
    [Fact]
    public async Task TestDownloadOk()
    {
        var url = "https://someurl.com/video.mp4";
        var content = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var mockHandler = new Mock<HttpClientHandler>();

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(content),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        Utility.HttpClientFactory = () => httpClient;

        var downloadedFile = await Utility.DownloadFileAsync(url);

        Assert.True(File.Exists(downloadedFile));

        var downloadedContent = await File.ReadAllBytesAsync(downloadedFile);
        Assert.Equal(content, downloadedContent);

        File.Delete(downloadedFile);
    }

    [Fact]
    public async Task TestDownload_Fail()
    {
        var url = "https://someurl.com/video.mp4";
        var mockHandler = new Mock<HttpClientHandler>();

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            });

        var httpClient = new HttpClient(mockHandler.Object);
        Utility.HttpClientFactory = () => httpClient;

        var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await Utility.DownloadFileAsync(url));
        Assert.Equal("Download failed", exception.Message); ;
        Assert.IsType<HttpRequestException>(exception.InnerException);
        Assert.Contains("Status code: NotFound", exception.InnerException.Message);
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
        Assert.Equal(isValid, result);
    }
}