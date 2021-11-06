using DiscordLOLader.Functions;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    class PictureSend : FileToConvert
    {
        BotCore Bot;
        private ConvertedFile ConvertedFile;

        DiscordMessageBuilder Builder;
        
        public delegate void Handler(bool button);
        public event Handler MessageCompleted;

        public PictureSend(BotCore Bot, ConvertedFile ConvertedFile)
        {
            this.Bot = Bot;
            this.ConvertedFile = ConvertedFile;
            Builder = new DiscordMessageBuilder();      
        }

        public Task PrepareImage(string Path)
        {
            FileInitialization(Path);

            const double Lock = 8000000; 

            if (FileSize > Lock)
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.Image);

                BitmapImage Bitmap = new BitmapImage();

                Bitmap.BeginInit();
                Bitmap.UriSource = new Uri(FilePath);
                Bitmap.CacheOption = BitmapCacheOption.None;
                Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                Bitmap.EndInit();

                double TargetPercent = 100 - (Lock / Convert.ToDouble(FileSize)*100);
                int PixWidth = Bitmap.PixelWidth;
                int PixHeight = Bitmap.PixelHeight;
                double PixWidthPercent = PixWidth * TargetPercent / 100;
                double PixHeightPercent = PixHeight * TargetPercent / 100;
                int TargetWidth = PixWidth - (int)PixWidthPercent;
                int TargetHeight = PixHeight - (int)PixHeightPercent;

                BitmapImage BitmapOutput = new BitmapImage();
    
                BitmapOutput.BeginInit();
                BitmapOutput.UriSource = new Uri(FilePath);
                BitmapOutput.DecodePixelHeight = TargetHeight;
                BitmapOutput.DecodePixelWidth = TargetWidth;
                BitmapOutput.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                BitmapOutput.CacheOption = BitmapCacheOption.None;
                BitmapOutput.EndInit();

                BitmapEncoder Encoder = new PngBitmapEncoder();
                Encoder.Frames.Add(BitmapFrame.Create(BitmapOutput));

                    using (FileStream fileStream = new FileStream(ConvertedFile.FilePath, FileMode.Create))
                    {
                        Encoder.Save(fileStream);
                        fileStream.Close();
                    }
                ConvertedFile.GetSize();
            }
            else
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.None, FilePath);
                ConvertedFile.GetSize();
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
            using FileStream ThumbStream = new FileStream(ConvertedFile.ThumbFile, FileMode.Create);
            EncoderTemp.Save(ThumbStream);
        }

        public System.Windows.Media.ImageSource GetPictureThumb()
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = File.Exists(ConvertedFile.ThumbFile) ? new Uri(ConvertedFile.ThumbFile) : new Uri(@"pack://application:,,,/Resources/Imager.png");
            Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Bitmap.CacheOption = BitmapCacheOption.OnLoad;
            Bitmap.EndInit();
            return Bitmap;
        }


        FileStream FileReader;
        public void RecieveImage(ulong channelid)
        {
             FileReader = File.OpenRead(ConvertedFile.FilePath);
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
