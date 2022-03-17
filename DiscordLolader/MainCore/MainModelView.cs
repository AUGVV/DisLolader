using DiscordLOLader.Bot;
using DiscordLOLader.Functions;
using DiscordLOLader.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.MainCore
{
    public partial class MainModelView : INotifyPropertyChanged
    {
        public ObservableCollection<Channel> Chan { get; set; }

        private readonly BotCore BotCore;
        private bool isChannelSelected = false;
        private readonly BotControl BotsControl;
        private readonly ConvertedFile ConvertedFile;
        private readonly ThumbCreator ThumbCreator;

        public MainModelView(BotCore BotRecieved)
        {
            BotCore = BotRecieved;
            BotsControl = new BotControl(BotCore);
            ConvertedFile = new ConvertedFile();
            ThumbCreator = new ThumbCreator();
            InitMediaPartial();
            InitEmbedPartial();
            InitPicturePartial();
            Chan = BotCore.ChannelList;

            GuildName = BotCore.GuildName;
            GuildId = BotCore.GuildId.ToString();
            GuildImage = BotCore.GetGuildThumb();
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
            get => _SelChannel;
            set
            {
                _SelChannel = value;
                isChannelSelected = true;
                ButtonSendWork = true;
                isSendButtonWork = true;

                if (_PathToPicture != null && !isPictureSending)
                {
                    ButtonImageWork = true;
                }
                if (_MediaPath != null && !isMediaSending)
                {
                    MediaSendButtonWork = true;
                }
                OnPropertyChanged("SelChannel");
            }
        }

        private TextBlock _SelMode;
        public TextBlock SelMode
        {
            get => _SelMode;
            set { _SelMode = value; BotsControl.ChangeStateAsync(SelMode.Text); OnPropertyChanged("_SelColor"); }
        }


        private Colors _SelColor;
        public Colors SelColor
        {
            get => _SelColor;
            set { _SelColor = value; OnPropertyChanged("_SelColor"); }
        }

        private string _GuildName;
        public string GuildName
        {
            get => _GuildName;
            set { _GuildName = value; OnPropertyChanged("GuildName"); }
        }

        private string _GuildId;
        public string GuildId
        {
            get => _GuildId;
            set { _GuildId = value; OnPropertyChanged("GuildName"); }
        }

        private BitmapImage _GuildImage;
        public BitmapImage GuildImage
        {
            get => _GuildImage;
            set { _GuildImage = value; OnPropertyChanged("GuildImage"); }
        }

        private RelayCommand _Close;
        public RelayCommand Close => _Close ??= new RelayCommand(async obj => { await BotCore.CloseConnectionAsync(); Environment.Exit(0); });

        private RelayCommand _RestartApp;
        public RelayCommand RestartApp => _RestartApp ??= new RelayCommand(obj => { _ = Process.Start(@$"{Environment.CurrentDirectory}\{AppDomain.CurrentDomain.FriendlyName}.exe"); Environment.Exit(0); });

        private RelayCommand _Turn;
        public RelayCommand Turn => _Turn ??= new RelayCommand(obj => { MinimizedOn = WindowState.Minimized; });

        private WindowState _MinimizedOn;
        public WindowState MinimizedOn
        {
            get => _MinimizedOn;
            set { _MinimizedOn = value; OnPropertyChanged("MinimizedOn"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}