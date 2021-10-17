# Video converter

- Supported output formats are mp4, webm and gif
- Input as file url or path

Requirements:

- .NET 5.0 SDK installed
- [FFmpeg](https://ffmpeg.org/download.html) installed and added to PATH

Build:

```bash
cd VideoConverter
dotnet publish -o <target path>
```

Configuration:

- Change default values for output folder and default output format in the `config.json` file

```json
{
  "outputFolder": "%USERPROFILE%\\Downloads",
  "defaultOutputFormat": "mp4"
}
```

Test:

```bash
cd Tests
dotnet test
```
