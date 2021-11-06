using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Functions
{
    class ThumbCreator
    {
        public void CreateThumbMedia(string FilePath, string PathTo)
        {
            ProcessStartInfo Video_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-ss 5 -y -i {FilePath} -vframes 1 -s 320x240 -f image2 {PathTo}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(Video_config);
            Input.WaitForExit();
        }

        public System.Windows.Media.ImageSource GetMediaThumb(string ThumbPath, string FileExtension)
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
                else if(FileExtension == ".wav")
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
