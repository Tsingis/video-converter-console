# Video converter

What is it?

- Conversions between common video formats
- Supported output formats are mp4, webm and gif
- Input as file url or path

Requirements:

- [FFmpeg](https://ffmpeg.org/download.html) executables downloaded
- (Optional) use _FFmpegDownloader_ to download required executables
- FFmpeg executables directory set to Path environment variable
- (Optional) FFmpeg executables inside the root of the application directory

Configuration:

- Change default values for output directory and default output format in the `config.json` file. Example below:

```json
{
  "defaultOutputDir": "%USERPROFILE%\\Downloads",
  "defaultOutputFormat": "mp4"
}
```

## Development

Additional requirements:

- [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) installed

Run project:

```bash
dotnet run --project <project>
```

Create project files:

```bash
dotnet publish -o <output path>
```

Run tests:

```bash
dotnet test
```
