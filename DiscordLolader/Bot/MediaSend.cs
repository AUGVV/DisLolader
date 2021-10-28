using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    class MediaSend
    {

        BotCore Bot;

        DiscordMessageBuilder Builder;

        public delegate void Handler(bool button);
        public event Handler MediaSendingCompleted;

        public long CurrentSize { get; private set; } = 0;
        public long NewSize { get; private set; } = 0;

        private string CacheFile = $@"{Environment.CurrentDirectory}\Cache\Download";
        private string ThumbFile = $@"{Environment.CurrentDirectory}\Cache\Thumb.png";
        private string CurrentFormat = "";

        private const double Lock = 800000000;
        private string PathToSendFile = "";

        public MediaSend(BotCore BotRecieved)
        {
            Bot = BotRecieved;
            Builder = new DiscordMessageBuilder();
        }

        public Task PrepareMedia(string path)
        {
            GetFileData(path);

            if (CurrentSize > Lock)
            {
                if (CurrentFormat.ToLower() == ".mp3")
                {
                    AddThumbImage(path);
                    Mp3Compress(path, CacheFile);
                    if (GetSize(CacheFile + CurrentFormat) > Lock)
                    {
                        Mp3Compress(path, CacheFile, "9");
                    }
                    PathToSendFile = $"{CacheFile}{CurrentFormat}";
                    NewSize = GetSize($"{CacheFile}{CurrentFormat}");
                }
                else if(CurrentFormat.ToLower() == ".webm" || CurrentFormat.ToLower() == ".mp4")
                {
                    AddThumbImage(path);
                    WebmCompress(path, CacheFile);
                    PathToSendFile = $"{CacheFile}.webm";
                    NewSize = GetSize($"{CacheFile}.webm");
                }
            }
            else
            {
                AddThumbImage(path);
                PathToSendFile = path;
                CurrentSize = NewSize = GetSize(PathToSendFile);
            }
            return Task.CompletedTask;
        }

        public System.Windows.Media.ImageSource GetMediaThumb()
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            if (File.Exists(ThumbFile))
            {
                Bitmap.UriSource = new Uri(ThumbFile);
            }
            else
            {
                Bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/Imager.png");
            }
            Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Bitmap.CacheOption = BitmapCacheOption.OnLoad;
            Bitmap.EndInit();
            return Bitmap;
        }

        private void AddThumbImage(string Path)
        {
            ProcessStartInfo Video_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-ss 5 -y -i {Path} -vframes 1 -s 320x240 -f image2 {ThumbFile}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(Video_config);
            Input.WaitForExit();
        }

        private void GetFileData(string path)
        {
            CurrentFormat = GetFormat(path);
            CurrentSize = GetSize(path);
        }

        public void AudVidSend(ulong channelid)
        {
            if (PathToSendFile != "" || !File.Exists(PathToSendFile))
            {
                Builder.WithFile(File.OpenRead(PathToSendFile));
                Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(Builder).ContinueWith(OnEvent);
                Builder.Clear();
            }
        }

        private void WebmCompress(string Path, string CacheFile)
        {
            ProcessStartInfo Video_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {Path} -y -c:v libvpx-vp9 -b:v 256K -s 640x360 -c:a libopus -b:a 22k {CacheFile}.webm",
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
                Arguments = $@"-i {Path} -y -vn -q:a {BitRate} -codec:a libmp3lame {CacheFile}.mp3",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(Music_config);
            Input.WaitForExit();
        }

        private long GetSize(string path)
        {
            FileInfo file = new FileInfo(path);
            long Size = file.Length;
            return Size * 100;
        }

        private string GetFormat(string path)
        {
            string[] Buffer = path.Split('.');
            string Last = "";
            foreach(string part in Buffer)
            {
                Last = part;
            }
            return $".{Last}";
        }

        private void OnEvent(Task t)
        {
            MediaSendingCompleted?.Invoke(true);
        }
    }
}
