using System;
using System.Diagnostics;
using System.IO;

namespace DiscordLOLader.Functions
{
    internal class AudioCompress
    {
        private string FilePath;

        private string CacheFile;

        private string TempPath;

        private double FileDuration;

        private FileInfo CurrentFile;

        public void Mp3Compress(string FilePath, string CacheFile, string BitRate = "4")
        {
            this.FilePath = FilePath;
            this.CacheFile = CacheFile;

            DeleteMeta();

            if (CurrentFile.Length > 8388608)
            {
                InitializationMediaInfo();

                ProcessStartInfo AudioCompress = new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = $@"-i {CacheFile} -y -codec:a libmp3lame -qscale:a {SetBitrate()} {TempPath}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
               };
                Process Input = Process.Start(AudioCompress);
                Input.WaitForExit();

                PrepareCompressedFile();
            }
        }

        private string SetBitrate()
        {
            int[,] Bitrates = new int[,]
            {
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 260, 250, 210, 195, 185, 150, 120, 120, 105, 85}
            };

            double PotentialFileSize;

            for (int i = 0; i < Bitrates.Length / 2; i++)
            {
                PotentialFileSize = FileDuration * (Bitrates[1, i] / 8);
                if (7600 > PotentialFileSize)
                {
                    return Bitrates[0, i].ToString();
                }
            }
            return "4";
        }

        private void PrepareCompressedFile()
        {
            File.Delete(CacheFile);
            File.Move(TempPath, CacheFile);
        }

        private void DeleteMeta()
        {
            ProcessStartInfo AudioMetaDelete = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {FilePath} -y -map_metadata -1 -vn -c:a copy {CacheFile}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(AudioMetaDelete);
            Input.WaitForExit();
            CurrentFile = new FileInfo(CacheFile);
            TempPath = @$"{CurrentFile.Directory}\compessed{CurrentFile.Name}";
        }

        private string Duration;
        private void InitializationMediaInfo()
        {
            ProcessStartInfo AudioInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {CacheFile}",
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(AudioInfo);
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
