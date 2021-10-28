using DiscordLOLader.Bot;
using DiscordLOLader.MVVM;
using DiscordLOLader.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.MainCore
{
    public partial class MainModelView : INotifyPropertyChanged
    {

        private bool isMediaSending;
        private bool isSendButtonWork;
        private System.Windows.Threading.DispatcherTimer MediaTimer;

        private void InitMediaPartial()
        {
            MediaSend = new MediaSend(BotCore);
            MediaSourse = (BitmapImage)Bitmap(new Uri(@"pack://application:,,,/Resources/Imager.png"));

            MediaTimer = new System.Windows.Threading.DispatcherTimer();

            MediaTimer.Tick += ProgressBarIncrement;
            MediaTimer.Interval = new TimeSpan(0, 0, 1);

            MediaSend.MediaSendingCompleted += MediaSendingCompleted;
            WaitMediaLabel = Visibility.Hidden;
            isMediaSending = false;
            isSendButtonWork = false;
        }

        private string _MediaPath;
        public string MediaPath
        {
            get => _MediaPath;
            set { _MediaPath = value; OnPropertyChanged("MediaPath"); PrepareMedia(); }
        }

        private string _MediaOriginalSize = "0 byte";
        public string MediaOriginalSize
        {
            get => _MediaOriginalSize;
            set { _MediaOriginalSize = value; OnPropertyChanged("MediaOriginalSize"); }
        }

        private string _MediaNewSize = "0 byte";
        public string MediaNewSize
        {
            get => _MediaNewSize;
            set { _MediaNewSize = value; OnPropertyChanged("MediaNewSize"); }
        }

        private async void PrepareMedia()
        {
            BlockMediaButtons();
            MediaTimer.Start();
            await Task.Run(() => MediaSend.PrepareMedia(MediaPath).Wait());
            MediaSourse = MediaSend.GetMediaThumb();
            ShowFileData();
            UnlockMediaButtons();
            WaitMediaLabel = Visibility.Hidden;
            if (isSendButtonWork) { MediaSendButtonWork = true; }
        }

        private void ShowFileData()
        {
            MediaOriginalSize = MediaSend.CurrentSize.ToString();
            MediaNewSize = MediaSend.NewSize.ToString();
        }




        private Visibility _WaitMediaLabel;
        public Visibility WaitMediaLabel
        {
            get => _WaitMediaLabel;
            set { _WaitMediaLabel = value; OnPropertyChanged("WaitMediaLabel"); }
        }



        private bool _MediaSendButtonWork = false;
        public bool MediaSendButtonWork
        {
            get => _MediaSendButtonWork;
            set { _MediaSendButtonWork = value; OnPropertyChanged("MediaSendButtonWork"); }
        }

        private RelayCommand _SendMedia;
        public RelayCommand SendMedia => _SendMedia ?? (_SendMedia = new RelayCommand(obj => { Debug.WriteLine("AudioVideoSend"); MediaFileSend(); }));

        private void MediaSendingCompleted(bool button)
        {
            UnlockMediaButtons();
            isMediaSending = false;
        }

        private void MediaFileSend()
        {
            BlockMediaButtons();
            isMediaSending = true;
            MediaSend.AudVidSend(_SelChannel.ChannelId);
        }

        private void BlockMediaButtons()
        {
            MediaTimer.Start();
            WaitMediaLabel = Visibility.Visible;
            isMediaSending = true;
            MediaSendButtonWork = false;
            MediaOpenButtonWork = false;
        }
        private void UnlockMediaButtons()
        {
            MediaTimer.Stop();
            MediaProgress = 0;
            WaitMediaLabel = Visibility.Hidden;
            isMediaSending = false;
            MediaSendButtonWork = true;
            MediaOpenButtonWork = true;
        }

        private int _MediaProgress = 0;
        public int MediaProgress
        {
            get => _MediaProgress;
            set { _MediaProgress = value; OnPropertyChanged("MediaProgress"); }
        }

        private void ProgressBarIncrement(object sender, EventArgs e)
        {
            MediaProgress++;
        }





        private bool _MediaOpenButtonWork = true;
        public bool MediaOpenButtonWork
        {
            get => _MediaOpenButtonWork;
            set { _MediaOpenButtonWork = value; OnPropertyChanged("MediaOpenButtonWork"); }
        }

        private RelayCommand _MediaOpenDialog;
        public RelayCommand MediaOpenDialog => _MediaOpenDialog ??= new RelayCommand(obj => { MediaPath = MediaDialog(); });

        private string MediaDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Audio files (*.MP3)|*.mp3*|Video files (*.MP4)|*.mp4*|Video files (*.WEBM)|*.webm*",
                Multiselect = false
            };
            return dialog.ShowDialog() == true ? dialog.FileName : "";
        }





        private ImageSource _MediaSourse;
        public ImageSource MediaSourse
        {
            get => _MediaSourse;
            set { _MediaSourse = value; OnPropertyChanged("MediaSourse"); }
        }
    }
}
