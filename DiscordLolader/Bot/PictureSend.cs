using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    class PictureSend
    {
        BotCore Bot;
    
        DiscordMessageBuilder Builder;

        public delegate void Handler(bool button);
        public event Handler MessageCompleted;

        public PictureSend(BotCore BotRecieved)
        {
            Bot = BotRecieved;
            Builder = new DiscordMessageBuilder();
           
        }

        public void PrepareImage(string path, ref string DownloadPath, ref string TempPath, ref string OriginalSize, ref string ResultSize)
        {
            FileInfo file = new FileInfo(path);
            long Size = file.Length; //Это 100%
            const double Lock = 800000000; //Это что то от 100 (8000000 * 100)
            OriginalSize = $"{Size} byte";

            Random rnd = new Random();

            string RandTempName = rnd.Next(0, 9).ToString() + rnd.Next(0, 9).ToString() + rnd.Next(0, 9).ToString() + rnd.Next(0, 9).ToString() + rnd.Next(0, 9).ToString() + rnd.Next(0, 9).ToString() + rnd.Next(0, 9).ToString() + rnd.Next(0, 9).ToString();
            TempPath = @$"{Environment.CurrentDirectory}\Cache\{RandTempName}_temp.png";
            DownloadPath = @$"{Environment.CurrentDirectory}\Cache\{RandTempName}.png";

            if (Size*100 > Lock)
            {
                var Bitmap = new BitmapImage();

                Bitmap.BeginInit();
                Bitmap.UriSource = new Uri(path);
                Bitmap.CacheOption = BitmapCacheOption.None;
                Bitmap.EndInit();

                double LockPercent = Lock / Convert.ToDouble(Size);
                double TargetPercent = 100 - LockPercent;
                int PixWidth = Bitmap.PixelWidth;
                int PixHeight = Bitmap.PixelHeight;
                double PixWidthPercent = (PixWidth * TargetPercent) / 100;
                double PixHeightPercent = (PixHeight * TargetPercent) / 100;
                int TargetWidth = PixWidth - (int)PixWidthPercent;
                int TargetHeight = PixHeight - (int)PixHeightPercent;

                var BitmapOutput = new BitmapImage();
                //Prepare for download
                BitmapOutput.BeginInit();
                BitmapOutput.UriSource = new Uri(path);
                BitmapOutput.DecodePixelHeight = (int)TargetHeight;
                BitmapOutput.DecodePixelWidth = (int)TargetWidth;
                BitmapOutput.CacheOption = BitmapCacheOption.None;
                BitmapOutput.EndInit();

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(BitmapOutput));

                using (var fileStream = new FileStream(DownloadPath, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                FileInfo fileResult = new FileInfo(DownloadPath);
                ResultSize = $"{fileResult.Length} byte"; //Это 100%                  
            }
            else
            {
                ResultSize = $"{Size} byte";
                file.CopyTo(DownloadPath, true);
           
            }

            var BitmapTemp = new BitmapImage();
            //Prepare for temp image
            BitmapTemp.BeginInit();
            BitmapTemp.UriSource = new Uri(path);
            BitmapTemp.DecodePixelHeight = 100;
            BitmapTemp.DecodePixelWidth = 100;
            BitmapTemp.CacheOption = BitmapCacheOption.None;
            BitmapTemp.EndInit();

            //Encode temp -----------------
            BitmapEncoder EncoderTemp = new PngBitmapEncoder();
            EncoderTemp.Frames.Add(BitmapFrame.Create(BitmapTemp));
            using (var fileStream2 = new FileStream(TempPath, FileMode.Create))
            {
                EncoderTemp.Save(fileStream2);
            }
            //-----------------
        }


        public void RecieveImage(string DownloadPath, ulong channelid)
        {
             Builder.WithFile(File.OpenRead(DownloadPath));
             Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(Builder).ContinueWith(OnEvent);
             Builder.Clear();        
        }

        private void OnEvent(Task t)
        {           
            MessageCompleted?.Invoke(true);
        }


   
    }
}
