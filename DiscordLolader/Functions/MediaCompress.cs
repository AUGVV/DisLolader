using System;
using System.Diagnostics;

namespace DiscordLOLader.Functions
{
    class MediaCompress
    {
        private const int MediaHeight = 260;
        private const int MediaWidth = 410;

        private string FilePath;
        private double FileDuration;
        private string FileBitrate;

        public void WebmCompress(string FilePath, string CacheFile)
        {
            this.FilePath = FilePath;

            InitializationMediaInfo();       
            FileBitrate = SetBitrate();
            CompressFile(CacheFile);
        }

        private void CompressFile(string CacheFile)
        {
            ProcessStartInfo VideoConfig = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {FilePath} -y -c:v libvpx -quality realtime -speed 15 -minrate {FileBitrate}K -b:v {FileBitrate}K -maxrate {FileBitrate}K -bufsize 75K -s {MediaWidth}x{MediaHeight} -r 22 -crf 4 -c:a libopus -b:a 64K -vbr:a off {CacheFile}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(VideoConfig);
            Input.WaitForExit();
        }

        private string SetBitrate()
        {
            double kbs = (8000 * 8 / FileDuration)-64;
            return Convert.ToInt32(kbs).ToString();
        }

        private string Duration;
        private void InitializationMediaInfo()
        {
            ProcessStartInfo VideoInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {FilePath}",
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(VideoInfo);
            Duration = Input.StandardError.ReadToEnd();
            Input.WaitForExit();
            GetDuration();
        }

        private void GetDuration()
        {
            Duration = Duration.Substring(Duration.IndexOf("Duration")).Substring(10, 11);
            DateTime DurationTime = Convert.ToDateTime(Duration);
            FileDuration = DurationTime.Minute * 60 + DurationTime.Second + 1;
        }
    }
}
