using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    class PictureSend
    {
        BotCore Bot;
    
        DiscordMessageBuilder Builder;

        public delegate void Handler(bool button);
        public event Handler MessageCompleted;


        private readonly string CacheFile = $@"{Environment.CurrentDirectory}\Cache\PictureDownload.png";
        private readonly string ThumbFile = $@"{Environment.CurrentDirectory}\Cache\PictureThumb.png";
        private string PathToSendFile = "";

        public long OriginalSize { get; private set; }
        public long ResultSize { get; private set; }

        public PictureSend(BotCore BotRecieved)
        {
            Bot = BotRecieved;
            Builder = new DiscordMessageBuilder();      
        }

        public Task PrepareImage(string Path)
        {
            const double Lock = 800000000; 
            OriginalSize = GetSize(Path);

            if (OriginalSize > Lock)
            {
                BitmapImage Bitmap = new BitmapImage();

                Bitmap.BeginInit();
                Bitmap.UriSource = new Uri(Path);
                Bitmap.CacheOption = BitmapCacheOption.None;
                Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                Bitmap.EndInit();

                double TargetPercent = 100 - (Lock / Convert.ToDouble(OriginalSize) * 100);
                int PixWidth = Bitmap.PixelWidth;
                int PixHeight = Bitmap.PixelHeight;
                double PixWidthPercent = PixWidth * TargetPercent / 100;
                double PixHeightPercent = PixHeight * TargetPercent / 100;
                int TargetWidth = PixWidth - (int)PixWidthPercent;
                int TargetHeight = PixHeight - (int)PixHeightPercent;

                BitmapImage BitmapOutput = new BitmapImage();
    
                BitmapOutput.BeginInit();
                BitmapOutput.UriSource = new Uri(Path);
                BitmapOutput.DecodePixelHeight = TargetHeight;
                BitmapOutput.DecodePixelWidth = TargetWidth;
                BitmapOutput.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                BitmapOutput.CacheOption = BitmapCacheOption.None;
                BitmapOutput.EndInit();

                BitmapEncoder Encoder = new PngBitmapEncoder();
                Encoder.Frames.Add(BitmapFrame.Create(BitmapOutput));

                    using (FileStream fileStream = new FileStream(CacheFile, FileMode.Create))
                    {
                        Encoder.Save(fileStream);
                        fileStream.Close();
                    }
       
 
                
                ResultSize = GetSize(CacheFile);
                PathToSendFile = CacheFile;
            }
            else
            {
                ResultSize = OriginalSize;
                PathToSendFile = Path;
            }
            CreateThumbPicture(Path);
            return Task.CompletedTask;
        }

        void CreateThumbPicture(string Path)
        {
            BitmapImage BitmapTemp = new BitmapImage();

            BitmapTemp.BeginInit();
            BitmapTemp.UriSource = new Uri(Path);
            BitmapTemp.DecodePixelHeight = 100;
            BitmapTemp.DecodePixelWidth = 100;
            BitmapTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            BitmapTemp.CacheOption = BitmapCacheOption.None;
            BitmapTemp.EndInit();

            BitmapEncoder EncoderTemp = new PngBitmapEncoder();
            EncoderTemp.Frames.Add(BitmapFrame.Create(BitmapTemp));
            using FileStream ThumbStream = new FileStream(ThumbFile, FileMode.Create);
            EncoderTemp.Save(ThumbStream);
        }

        public System.Windows.Media.ImageSource GetPictureThumb()
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = File.Exists(ThumbFile) ? new Uri(ThumbFile) : new Uri(@"pack://application:,,,/Resources/Imager.png");
            Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Bitmap.CacheOption = BitmapCacheOption.OnLoad;
            Bitmap.EndInit();
            return Bitmap;
        }

        private long GetSize(string path)
        {
            FileInfo file = new FileInfo(path);
            return file.Length * 100;
        }

        FileStream FileReader;
        public void RecieveImage(ulong channelid)
        {
             FileReader = File.OpenRead(PathToSendFile);
             Builder.WithFile(FileReader);   
             Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(Builder).ContinueWith(OnEvent);
             Builder.Clear();
        }

        private void OnEvent(Task t)
        {
            FileReader.Close();
            MessageCompleted?.Invoke(true);
        }
    }
}
