using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoConverter;

public static class Utility
{
    public static Func<HttpClient> HttpClientFactory { get; set; } = () => new HttpClient();

    public static async Task<string> DownloadFileAsync(string fileUrl)
    {
        try
        {
            var url = new Uri(fileUrl);
            var file = Path.GetFileName(url.LocalPath);
            var downloadPath = Path.Join(Path.GetTempPath(), file);

            using (var client = HttpClientFactory())
            {
                var res = await client.GetAsync(url, HttpCompletionOption.ResponseContentRead);
                if (res.IsSuccessStatusCode)
                {
                    using (var fs = new FileStream(downloadPath, FileMode.Create))
                    {
                        await res.Content.CopyToAsync(fs);
                        return downloadPath;
                    }
                }

                throw new HttpRequestException($"Status code: {res.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Download failed.", ex);
        }
    }

    public static bool IsValidUrl(string inputFilePath)
    {
        var success = Uri.TryCreate(inputFilePath, UriKind.Absolute, out Uri validUri);
        return success && (validUri.Scheme == Uri.UriSchemeHttp || validUri.Scheme == Uri.UriSchemeHttps);
    }

    public static string GetOutputFilepath(string inputFilePath, string outputDir, string outputFormat)
    {
        var inputFile = Path.GetFileName(inputFilePath);
        var outputFilepath = Path.Join(outputDir, inputFile);
        return Path.ChangeExtension(outputFilepath, outputFormat);
    }
}