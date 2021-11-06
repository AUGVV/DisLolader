using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DiscordLOLader.Functions
{
    class AudioCompress
    {
        private string FilePath;

        private string CacheFile;

        private string TempPath;

        private double FileDuration;

        private FileInfo CurrentFile;

        public void Mp3Compress(string FilePath, string CacheFile, string BitRate = "4")
        {
            Debug.WriteLine("start");
            this.FilePath = FilePath;
            this.CacheFile = CacheFile;

            DeleteMeta();

            if (CurrentFile.Length > 8388608)
            {
                Debug.WriteLine("Need compress " + CacheFile);
                Debug.WriteLine("To " + TempPath);
                InitializationMediaInfo();

                ProcessStartInfo AudioCompress = new ProcessStartInfo
                {
                        FileName = "ffmpeg.exe",
                        Arguments = $@"-i {CacheFile} -y -vn -q:a {SetBitrate()} -codec:a libmp3lame {TempPath}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false
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
                { 320, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 320, 260, 250, 210, 195, 185, 150, 120, 120, 105, 85}
            };

            double PotentialFileSize;

            for (int i = 0; i < Bitrates.Length/2; i++)
            {
                PotentialFileSize = FileDuration * (Bitrates[1, i] / 8);
                Debug.WriteLine("Potential: "+PotentialFileSize);

                Debug.WriteLine(Bitrates[0, i]);
                Debug.WriteLine(Bitrates[1, i]);
                if (7600 > PotentialFileSize)
                {
                    Debug.WriteLine("OK");
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
                CreateNoWindow = false
            };
            Process Input = Process.Start(AudioInfo);
            Duration = Input.StandardError.ReadToEnd();
            Input.WaitForExit();
            GetDuration();
        }

        private void GetDuration()
        {
            Duration = Duration.Substring(Duration.IndexOf("Duration")).Substring(10, 11); 
            Debug.WriteLine(Duration);
            DateTime DurationTime = Convert.ToDateTime(Duration);
            FileDuration = DurationTime.Minute * 60 + DurationTime.Second + 1;
        }
    }
}
