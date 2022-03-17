using DiscordLOLader.Functions;
using DSharpPlus.Entities;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    internal class PictureSend : FileToConvert
    {
        private readonly BotCore Bot;
        private ConvertedFile ConvertedFile;
        private readonly ThumbCreator ThumbCreator;
        private readonly DiscordMessageBuilder Builder;
        public delegate void Handler(bool button);
        public event Handler MessageCompleted;

        private const double Lock = 8388608;

        public PictureSend(BotCore Bot, ConvertedFile ConvertedFile, ThumbCreator ThumbCreator)
        {
            this.Bot = Bot;
            this.ConvertedFile = ConvertedFile;
            this.ThumbCreator = ThumbCreator;
            Builder = new DiscordMessageBuilder();
        }

        public Task PrepareImage(string Path)
        {
            FileInitialization(Path);

            if (FileSize > Lock)
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.Image);
                BitmapImage Bitmap = GetImage();
                BitmapEncoder Encoder = new PngBitmapEncoder();
                Encoder.Frames.Add(BitmapFrame.Create(CompressedImage(GetTargetWidth(Bitmap), GetTargetHeight(Bitmap))));
                SaveCompressedFile(Encoder);
                ConvertedFile.GetSize();
            }
            else
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.None, FilePath);
                ConvertedFile.GetSize();
            }
            ThumbCreator.CreateThumbPicture(Path, ConvertedFile.ThumbFile);
            return Task.CompletedTask;
        }

        private void SaveCompressedFile(BitmapEncoder Encoder)
        {
            using FileStream fileStream = new FileStream(ConvertedFile.FilePath, FileMode.Create);
            Encoder.Save(fileStream);
            fileStream.Close();
        }

        private int GetTargetWidth(BitmapImage Bitmap)
        {
            int PixWidth = Bitmap.PixelWidth;
            double PixWidthPercent = PixWidth * GetTargetPercent() / 100;
            return PixWidth - (int)PixWidthPercent;
        }

        private int GetTargetHeight(BitmapImage Bitmap)
        {
            int PixHeight = Bitmap.PixelHeight;
            double PixHeightPercent = PixHeight * GetTargetPercent() / 100;
            return PixHeight - (int)PixHeightPercent;
        }

        private double GetTargetPercent()
        {
            return 100 - (Lock / Convert.ToDouble(FileSize) * 100);
        }

        private BitmapImage GetImage()
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = new Uri(FilePath);
            Bitmap.CacheOption = BitmapCacheOption.None;
            Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Bitmap.EndInit();
            return Bitmap;
        }

        private BitmapImage CompressedImage(int TargetWidth, int TargetHeight)
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = new Uri(FilePath);
            Bitmap.DecodePixelHeight = TargetHeight;
            Bitmap.DecodePixelWidth = TargetWidth;
            Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Bitmap.CacheOption = BitmapCacheOption.None;
            Bitmap.EndInit();
            return Bitmap;
        }

        private FileStream FileReader;
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
