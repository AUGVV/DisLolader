using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Functions
{
    class ThumbCreator
    {
        public void CreateThumbMedia(string FilePath, string PathTo)
        {
            try
            {
                ProcessStartInfo VideoConfig = new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = $@"-ss 5 -y -i {FilePath} -vframes 1 -s 320x240 -f image2 {PathTo}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                Process Input = Process.Start(VideoConfig);
                Input.WaitForExit();
            }
            catch
            {
                CreateThumbPicture("pack://application:,,,/Resources/Mp3Thumb.png", PathTo);
            }
        }

        public void CreateThumbPicture(string Path, string PathTo)
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
            using FileStream ThumbStream = new FileStream(PathTo, FileMode.Create);
            EncoderTemp.Save(ThumbStream);
        }

        public ImageSource GetThumb(string ThumbPath, string FileExtension)
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = GetThumbSource(ThumbPath, FileExtension);
            Bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Bitmap.CacheOption = BitmapCacheOption.OnLoad;
            Bitmap.EndInit();
            return Bitmap;
        }

        private Uri GetThumbSource(string ThumbPath, string FileExtension)
        {
            if (FileExtension == ".mp3")
            {
                return new Uri(@"pack://application:,,,/Resources/Mp3Thumb.png");
            }
            else if (FileExtension == ".wav")
            {
                return new Uri(@"pack://application:,,,/Resources/WavThumb.png");
            }
            else
            {
                if (File.Exists(ThumbPath))
                {
                    return new Uri(ThumbPath);
                }
            }
            return new Uri(@"pack://application:,,,/Resources/Mp3Thumb.png");
        }
    }
}
