using System.IO;
using System.Xml.Linq;
using System;
using Xabe.FFmpeg.Downloader;


namespace FFmpeg.Downloader
{
    public class Program
    {
        public static void Main()
        {
            var path = Path.Join(Environment.CurrentDirectory, "FFmpeg");
            Console.WriteLine($"Downloading FFmpeg executables to {path}");
            DownloadFFmpegExecutables(path);
            Console.WriteLine("Download finished.");
        }

        private static void DownloadFFmpegExecutables(string destinationPath)
        {
            FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full, destinationPath).Wait();
        }
    }
}
