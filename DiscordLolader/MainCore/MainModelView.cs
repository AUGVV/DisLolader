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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.MainCore
{
    public partial class MainModelView : INotifyPropertyChanged
    {
        private BotCore BotCore;


        private PictureSend PictureSend;
        private MediaSend MediaSend;

        private bool isSending = false;
        private bool isChannelSelected = false;
        public ObservableCollection<Channel> Chan { get; set; }



        public MainModelView(BotCore BotRecieved)
        {
            BotCore = BotRecieved;
            InitMediaPartial();
            InitEmbedPartial();
            Chan = BotCore.ChannelList;

            PictureSend = new PictureSend(BotCore);

            GuildName = BotCore.GuildName;
            GuildId = BotCore.GuildId.ToString();
            PictureSouse = (BitmapImage)Bitmap(new Uri(@"pack://application:,,,/Resources/Imager.png"));


            GuildImage = BotCore.GetGuildThumb();
            PictureSend.MessageCompleted += PictureCompleted;


            ImageDragDrop = true;

            WaitImageLabel = System.Windows.Visibility.Hidden;
        }

        public static System.Windows.Media.ImageSource Bitmap(Uri path)
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = path;
            Bitmap.CacheOption = BitmapCacheOption.OnLoad;
            Bitmap.EndInit();
            return Bitmap;
        }


        private Channel _SelChannel;
        public Channel SelChannel
        {
            get { return _SelChannel; }
            set
            {
                _SelChannel = value;
                isChannelSelected = true;
                ButtonSendWork = true;
                isSendButtonWork = true;

                if (_PathToPicture != null && !isSending)
                {
                    ButtonImageWork = true;
                }
                Debug.WriteLine(_MediaPath);
                if (_MediaPath != null && !isMediaSending)
                {
                    MediaSendButtonWork = true;
                }
                OnPropertyChanged("SelChannel");
            }
        }

        private Colors _SelColor;
        public Colors SelColor
        {
            get { return _SelColor; }
            set { _SelColor = value; OnPropertyChanged("_SelColor"); }
        }

        private string _GuildName;
        public string GuildName
        {
            get { return _GuildName; }
            set { _GuildName = value; OnPropertyChanged("GuildName"); }
        }

        private string _GuildId;
        public string GuildId
        {
            get { return _GuildId; }
            set { _GuildId = value; OnPropertyChanged("GuildName"); }
        }

        private BitmapImage _GuildImage;
        public BitmapImage GuildImage
        {
            get { return _GuildImage; }
            set { _GuildImage = value; OnPropertyChanged("GuildImage"); }
        }

        private RelayCommand _Close;
        public RelayCommand Close => _Close ?? (_Close = new RelayCommand(obj => { BotCore.CloseConnection(); Environment.Exit(0); }));

        private RelayCommand _RestartApp;
        public RelayCommand RestartApp => _RestartApp ?? (_RestartApp = new RelayCommand(obj => { Process.Start(@$"{Environment.CurrentDirectory}\DiscordLOLader.exe"); Environment.Exit(0); }));

        private RelayCommand _Turn;
        public RelayCommand Turn
        {
            get { return _Turn ??= new RelayCommand(obj => { MinimizedOn = WindowState.Minimized; }); }
        }

        private WindowState _MinimizedOn;
        public WindowState MinimizedOn
        {
            get { return _MinimizedOn; }
            set { _MinimizedOn = value; OnPropertyChanged("MinimizedOn"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}