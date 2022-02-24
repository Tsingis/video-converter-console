using System;
using System.Threading.Tasks;
using Xabe.FFmpeg.Downloader;

namespace FFmpeg.Downloader;

public class Program
{
    public static void Main()
    {
        var path = Environment.CurrentDirectory;
        Console.WriteLine($"Downloading FFmpeg executables to {path}");
        DownloadFFmpegExecutables(path).Wait();
        Console.WriteLine("Download finished.");
    }

    private async static Task DownloadFFmpegExecutables(string destinationPath)
    {
        await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full, destinationPath);
    }
}