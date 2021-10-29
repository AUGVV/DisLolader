using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    class MediaSend
    {
        public long CurrentSize { get; private set; } = 0;
        public long NewSize { get; private set; } = 0;


        private BotCore Bot;
        private DiscordMessageBuilder Builder;

        public delegate void Handler(bool button);
        public event Handler MediaSendingCompleted;

        private readonly string CacheFile = $@"{Environment.CurrentDirectory}\Cache\Download";
        private readonly string ThumbFile = $@"{Environment.CurrentDirectory}\Cache\Thumb.png";
        private string OldPath = "";
        private string CurrentFormat = "";

        private const double Lock = 800000000;
        private string PathToSendFile = "";

        public MediaSend(BotCore BotRecieved)
        {
            Bot = BotRecieved;
            Builder = new DiscordMessageBuilder();
        }

        public Task PrepareMedia(string Path)
        {
            GetFileData(Path);
            OldPath = Path;
            if (CurrentSize > Lock)
            {
                if (CurrentFormat.ToLower() == ".mp3")
                {
                    CreateThumbImage(Path);
                    Mp3Compress(Path, CacheFile);
                    if (GetSize(CacheFile + CurrentFormat) > Lock)
                    {
                        Mp3Compress(Path, CacheFile, "9");
                    }
                    PathToSendFile = $"{CacheFile}{CurrentFormat}";
                    NewSize = GetSize($"{CacheFile}{CurrentFormat}");
                }
                else if (CurrentFormat.ToLower() == ".webm" || CurrentFormat.ToLower() == ".mp4")
                {
                    CreateThumbImage(Path);
                    WebmCompress(Path, CacheFile);
                    PathToSendFile = $"{CacheFile}.webm";
                    NewSize = GetSize($"{CacheFile}.webm");
                }
            }
            else
            {
                CreateThumbImage(Path);
                PathToSendFile = Path;
                CurrentSize = NewSize = GetSize(PathToSendFile);
            }
            return Task.CompletedTask;
        }

        public System.Windows.Media.ImageSource GetMediaThumb()
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = File.Exists(ThumbFile) && CurrentFormat.ToLower() != ".mp3" ? new Uri(ThumbFile) : new Uri(@"pack://application:,,,/Resources/Mp3Thumb.png");
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


        FileStream FileReader; 
        public void MediaFileSend(ulong channelid)
        {
            if (PathToSendFile != "" || !File.Exists(PathToSendFile))
            {
                FileReader = File.OpenRead(PathToSendFile);
                _ = Builder.WithFile(GetFileName(OldPath), FileReader);
                _ = Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(Builder).ContinueWith(OnEvent);
                Builder.Clear();
            }
        }

        private void WebmCompress(string Path, string CacheFile)
        {
            ProcessStartInfo Video_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {Path} -y -c:v libvpx-vp9 -quality realtime -speed 15 -b:v 150K -maxrate 150K  -s 401x255 -r 22 -crf 4 -c:a libopus -b:a 96k {CacheFile}.webm",
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
            return file.Length * 100;
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
