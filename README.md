# Video converter

What is it?

- Conversions between common video formats
- Supported output formats are mp4, webm and gif
- Input as file url or path

Requirements:

- [FFmpeg](https://ffmpeg.org/download.html) executables downloaded
- (Optional) use _FFmpegDownloader_ to download required executables
- FFmpeg executables directory set to Path environment variable

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

- [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed

Create application executable:

```bash
cd VideoConverter
dotnet publish -o <target path>
```

Run tests:

```bash
cd Tests
dotnet test
```
