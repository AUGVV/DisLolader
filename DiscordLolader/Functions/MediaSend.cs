using DiscordLOLader.Functions;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    class MediaSend : FileToConvert
    {
        private BotCore Bot;
        private ConvertedFile ConvertedFile;
        private DiscordMessageBuilder Builder;

        public delegate void Handler(bool button);
        public event Handler MediaSendingCompleted;

        private const double Lock = 8000000;

        public MediaSend(BotCore Bot, ConvertedFile ConvertedFile)
        {
            this.Bot = Bot;
            this.ConvertedFile = ConvertedFile;
            Builder = new DiscordMessageBuilder();
        }

        public Task PrepareMedia(string Path)
        {
            FileInitialization(Path);
            if (FileSize > Lock)
            {
                if (FileExtension == ".mp3" || FileExtension == ".wav")
                {
                    ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.Sound);
                    CreateThumbImage(FilePath);
                    Mp3Compress(FilePath, ConvertedFile.FilePath);
                    ConvertedFile.GetSize();
                    if (ConvertedFile.FileSize > Lock)
                    {
                        Mp3Compress(Path, ConvertedFile.FilePath, "9");
                    }
                }
                else if (FileExtension == ".webm" || FileExtension == ".mp4")
                {
                    ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.Media);
                    CreateThumbImage(FilePath);
                    WebmCompress(FilePath, ConvertedFile.FilePath);
                    ConvertedFile.GetSize();
                }
            }
            else
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.None, FilePath);
                ConvertedFile.GetSize();
                CreateThumbImage(FilePath);
            }
            return Task.CompletedTask;
        }

        public System.Windows.Media.ImageSource GetMediaThumb()
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = File.Exists(ConvertedFile.ThumbFile) && (FileExtension != ".mp3" || FileExtension == ".wav") ? new Uri(ConvertedFile.ThumbFile) : new Uri(@"pack://application:,,,/Resources/Mp3Thumb.png");
            Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Bitmap.CacheOption = BitmapCacheOption.OnLoad;
            Bitmap.EndInit();
            return Bitmap;
        }

        private void CreateThumbImage(string Path)
        {
            ProcessStartInfo Video_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-ss 5 -y -i {Path} -vframes 1 -s 320x240 -f image2 {ConvertedFile.ThumbFile}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(Video_config);
            Input.WaitForExit();
        }

        FileStream FileReader; 
        public void MediaFileSend(ulong channelid)
        {
            if (ConvertedFile.FilePath != "" || !File.Exists(ConvertedFile.FilePath))
            {
                FileReader = File.OpenRead(ConvertedFile.FilePath);
                _ = Builder.WithFile(GetFileName(FileName), FileReader);;
                _ = Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(Builder).ContinueWith(OnEvent);
                Builder.Clear();
            }
        }

        private void WebmCompress(string Path, string CacheFile)
        {
            ProcessStartInfo Video_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {FilePath} -y -c:v libvpx-vp9 -quality realtime -speed 15 -b:v 150K -maxrate 150K  -s 401x255 -r 22 -crf 4 -c:a libopus -b:a 96k {ConvertedFile.FilePath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(Video_config);
            Input.WaitForExit();
        }

        private void Mp3Compress(string Path, string CacheFile, string BitRate = "4")
        {
            ProcessStartInfo Music_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {FilePath} -y -vn -q:a {BitRate} -codec:a libmp3lame {ConvertedFile.FilePath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };
            Process Input = Process.Start(Music_config);
            Input.WaitForExit();
        }

        private string GetFileName(string path)
        {
            string[] Buffer = path.Split('\\');
            return $"{Buffer[^1]}";
        }

        private string GetFormat(string path)
        {
            string[] Buffer = path.Split('.');
            return $".{Buffer[^1]}";
        }

        private void OnEvent(Task t)
        {
            FileReader.Close();
            MediaSendingCompleted?.Invoke(true);
        }
    }
}
