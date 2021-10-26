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
    class MainModelView : INotifyPropertyChanged
    {
        private BotCore BotCore;

        private EmbedSend EmbedSend;
        private PictureSend PictureSend;
        private AudioVideoSend AudioVideoSend;

        private bool isSending { get; set; } = false;
        private bool isChannelSelected{ get; set; } = false;
        public ObservableCollection<Channel> Chan { get; set; }

        public ObservableCollection<Colors> Color { get; set; }

        public MainModelView(BotCore BotRecieved)
        {
           
            BotCore = BotRecieved;
            Chan = BotCore.ChannelList;
            EmbedSend = new EmbedSend(BotCore);
            PictureSend = new PictureSend(BotCore);
            AudioVideoSend = new AudioVideoSend(BotCore);
            GuildName = BotCore.GuildName;
            GuildId = BotCore.GuildId.ToString();
            PictureSouse = (BitmapImage)Bitmap(new Uri(@"pack://application:,,,/Resources/Imager.png"));
            VideoSouse = (BitmapImage)Bitmap(new Uri(@"pack://application:,,,/Resources/Imager.png"));
            GuildImage = BotCore.GetGuildThumb();
            PictureSend.MessageCompleted += PictureCompleted;
            Color = EmbedSend.ColorsList;
            CountLabel = "0";
            ImageDragDrop = true;
            OverflowText = System.Windows.Visibility.Hidden;
            WaitImageLabel = System.Windows.Visibility.Hidden;
        }


        private BitmapImage _VideoSouse;
        public BitmapImage VideoSouse
        {
            get { return _VideoSouse; }

            set { _VideoSouse = value; OnPropertyChanged("VideoSouse"); }
        }


        private string _PathToAudVid;
        public string PathToAudVid
        {
            get { return _PathToAudVid; }
            set { _PathToAudVid = value; PrepareVidOrAud(); OnPropertyChanged("PathToAudVid"); }
        }

        void PrepareVidOrAud()
        {
            AudioVideoSend.PrepareAudioVideo(PathToAudVid);


        }


        private Channel _selChannel;
        public Channel SelChannel
        {
            get { return _selChannel; }
            set
            {
                _selChannel = value;
                Debug.WriteLine(_selChannel.ChannelName);

                isChannelSelected = true;
                ButtonSendWork = true;

                if (_PathToPicture != null && isSending == false)
                {
                    ButtonImageWork = true;

                }
                OnPropertyChanged("SelChannel");
            }
        }

        private Colors _SelColor;
        public Colors SelColor
        {
            get { return _SelColor; }
            set
            {
                _SelColor = value;
                Debug.WriteLine(_SelColor.ColorName);
                OnPropertyChanged("_SelColor");
            }
        }

        private string _GuildName;
        public string GuildName
        {
            get { return _GuildName; }
            set { _GuildName = value;  OnPropertyChanged("GuildName"); }
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


        //-------------===Image===-------------
        #region Image

        //=====================================
        // Prepare temp and picture to download
        //=====================================
        #region Image preparing
        private string RefDownloadPath = "";
        private string RefTempPath = "";
        private string RefSizeOriginal = "";
        private string RefSizeResult = "";

        private BitmapImage _PictureSouse;
        public BitmapImage PictureSouse
        {
            get { return _PictureSouse; }
            
            set { _PictureSouse = value; OnPropertyChanged("PictureSouse"); }
        }

        /// <summary>
        /// Control image cache
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static System.Windows.Media.ImageSource Bitmap(Uri path)
        {
            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = path;
            Bitmap.CacheOption = BitmapCacheOption.OnLoad;
            Bitmap.EndInit();
            return Bitmap;
        }

        /// <summary>
        /// Take image and prepare to sending
        /// </summary>
        /// <param name="path"></param>
        private void PreparePicture(string path)
        {
            //Preparing and take all necessary info about image 
            PictureSend.PrepareImage(path, ref RefDownloadPath, ref RefTempPath, ref RefSizeOriginal, ref RefSizeResult);
            //Create temp picture and show is in view panel 
            PictureSouse = (BitmapImage)Bitmap(new Uri(RefTempPath));
            //Data for labels
            OriginalSizeLabel = RefSizeOriginal;
            ResultSizeLabel = RefSizeResult;
            //If button send on embed working (is depend of channel selection) activating button of image sending
            if (ButtonSendWork == true) { ButtonImageWork = true; }
        }

        /// <summary>
        /// This parametr show path to will processing image and is an variable for operations with this image 
        /// </summary>
        private string _PathToPicture;
        public string PathToPicture
        {
            get { return _PathToPicture; }
            set { _PathToPicture = value; PreparePicture(_PathToPicture); OnPropertyChanged("PathToPicture"); }
        }
        #endregion
        //=====================================

        //=====================================
        #region Send process

        /// <summary>
        /// Sending button
        /// </summary>
        private RelayCommand _SendPicture;
        public RelayCommand SendPicture
        {
            get
            {
                return _SendPicture ??
                  (_SendPicture = new RelayCommand(obj =>
                  {
                      SendImage();
                  }));
            }
        }


        /// <summary>
        /// Block or unblock DragAndDrop zone of image 
        /// </summary>
        private bool _ImageDragDrop = false;
        public bool ImageDragDrop
        {
            get { return _ImageDragDrop; }
            set { _ImageDragDrop = value; OnPropertyChanged("ImageDragDrop"); }
        }

        /// <summary>
        ///  Send image to discord and block all buttons depended image tab (This make for send once and dont run many sendings)
        /// </summary>
        private void SendImage()
        {
            ButtonImageWork = false;
            ButtonOpenWork = false;
            ImageDragDrop = false;
            isSending = true;
            WaitImageLabel = System.Windows.Visibility.Visible;
            PictureSend.RecieveImage(RefDownloadPath, _selChannel.ChannelId);
        }

        /// <summary>
        /// Wait ending of sending and unblock buttons
        /// </summary>
        /// <param name="button"></param>
        private void PictureCompleted(bool button)
        {
            ButtonImageWork = button;
            ImageDragDrop = button;
            isSending = false;
            ButtonOpenWork = true;
            WaitImageLabel = System.Windows.Visibility.Hidden;
        }

        /// <summary>
        /// Block or unblock button of sending 
        /// </summary>
        private bool _ButtonImageWork = false;
        public bool ButtonImageWork
        {
            get { return _ButtonImageWork; }
            set { _ButtonImageWork = value; OnPropertyChanged("ButtonImageWork"); }
        }
        #endregion
        //=====================================

        //=====================================
        #region InfoLabels
        private string _OriginalSizeLabel;
        public string OriginalSizeLabel
        {
            get { return _OriginalSizeLabel; }
            set { _OriginalSizeLabel = value; OnPropertyChanged("OriginalSizeLabel"); }
        }

        private string _ResultSizeLabel;
        public string ResultSizeLabel
        {
            get { return _ResultSizeLabel; }
            set { _ResultSizeLabel = value; OnPropertyChanged("ResultSizeLabel"); }
        }

        private System.Windows.Visibility _WaitImageLabel;
        public System.Windows.Visibility WaitImageLabel
        {
            get { return _WaitImageLabel; }
            set { _WaitImageLabel = value; OnPropertyChanged("WaitImageLabel"); }
        }
        #endregion
        //=====================================

        //=====================================
        #region OpenDialogWindow
        private RelayCommand _OpenPathFinder;
        public RelayCommand OpenPathFinder
        {
            get
            {
                return _OpenPathFinder ??
                  (_OpenPathFinder = new RelayCommand(obj =>
                  {
                      PathToPicture = OpenDialog();
                  }));
            }
        }

        private string OpenDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Picture files (*.PNG)|*.png*|Picture files (*.JPG)|*.jpg*";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        /// <summary>
        /// Block or unblock button of open dialog
        /// </summary>
        private bool _ButtonOpenWork = true;
        public bool ButtonOpenWork
        {
            get { return _ButtonOpenWork; }
            set { _ButtonOpenWork = value; OnPropertyChanged("ButtonOpenWork"); }
        }
        #endregion
        //=====================================]

        #endregion
        //-------------===Image===-------------

        //-------------===EMBED===-------------
        #region embed

        #region Title Text
        //---------------------------------------------------
        private string _TitleText;
        public string TitleText
        {
            get { return _TitleText; }
            set { _TitleText = value; isTitleType(); counter(); OnPropertyChanged("TitleText"); }
        }

        private void isTitleType()
        {
            if (_TitleText.Length > 0)
            {  TitleLabel = System.Windows.Visibility.Hidden; }
            else if (_TitleText.Length == 0)
            { TitleLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _TitleLabel;
        public System.Windows.Visibility TitleLabel
        {
            get { return _TitleLabel; }
            set { _TitleLabel = value; OnPropertyChanged("TitleLabel"); }
        }
        //---------------------------------------------------------------
        #endregion
        //========================
        #region Author Text
        //---------------------------------------------------------------
        private string _AuthorText;
        public string AuthorText
        {
            get { return _AuthorText; }
            set { _AuthorText = value; isAuthorType(); counter(); OnPropertyChanged("AuthorText"); }
        }

        private void isAuthorType()
        {
            if (_AuthorText.Length > 0)
            { AuthorLabel = System.Windows.Visibility.Hidden; }
            else if (_AuthorText.Length == 0)
            { AuthorLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _AuthorLabel;
        public System.Windows.Visibility AuthorLabel
        {
            get { return _AuthorLabel; }
            set { _AuthorLabel = value; OnPropertyChanged("AuthorLabel"); }
        }
        //---------------------------------------------------------------
        #endregion
        //========================
        #region Descriprion Text
        //---------------------------------------------------------------
        private string _DescriptionText;
        public string DescriptionText
        {
            get { return _DescriptionText; }
            set { _DescriptionText = value; isDescriprionType(); counter(); OnPropertyChanged("DescriptionText");  }
        }

        private void isDescriprionType()
        {
            if (_DescriptionText.Length > 0)
            { DescriptionLabel = System.Windows.Visibility.Hidden; }
            else if (_DescriptionText.Length == 0)
            { DescriptionLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _DescriptionLabel;
        public System.Windows.Visibility DescriptionLabel
        {
            get { return _DescriptionLabel; }
            set { _DescriptionLabel = value; OnPropertyChanged("DescriptionLabel"); }
        }
        //---------------------------------------------------------------
        #endregion
        //========================
        #region Footer Text
        //---------------------------------------------------------------
        private string _FooterText;
        public string FooterText
        {
            get { return _FooterText; }
            set { _FooterText = value; isFooterType(); counter(); OnPropertyChanged("FooterText"); }
        }

        private void isFooterType()
        {
            if (_FooterText.Length > 0)
            { FooterLabel = System.Windows.Visibility.Hidden; }
            else if (_FooterText.Length == 0)
            { FooterLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _FooterLabel;
        public System.Windows.Visibility FooterLabel
        {
            get { return _FooterLabel; }
            set { _FooterLabel = value; OnPropertyChanged("FooterLabel"); }
        }
        //---------------------------------------------------------------
        #endregion
        //========================
        #region Image Url Text
        //---------------------------------------------------------------
        private string _ImageUrlText;
        public string ImageUrlText
        {
            get { return _ImageUrlText; }
            set
            { _ImageUrlText = value;  isImageURLType(); OnPropertyChanged("ImageUrlText"); }
        }

        private void isImageURLType()
        {
            if (_ImageUrlText.Length > 0)
            { ImageUrlLabel = System.Windows.Visibility.Hidden; }
            else if (_ImageUrlText.Length == 0)
            { ImageUrlLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _ImageUrlLabel;
        public System.Windows.Visibility ImageUrlLabel
        {
            get { return _ImageUrlLabel; }
            set { _ImageUrlLabel = value; OnPropertyChanged("ImageUrlLabel"); }
        }
        //---------------------------------------------------------------
        #endregion
        //========================
        #region Thumb Text
        //---------------------------------------------------------------
        private string _ThumbText;
        public string ThumbText
        {
            get { return _ThumbText; }
            set { _ThumbText = value; isThumbType(); OnPropertyChanged("ThumbText"); }
        }

        private void isThumbType()
        {
            if (_ThumbText.Length > 0)
            { ThumbLabel = System.Windows.Visibility.Hidden; }
            else if (_ThumbText.Length == 0)
            { ThumbLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _ThumbLabel;
        public System.Windows.Visibility ThumbLabel
        {
            get { return _ThumbLabel; }
            set
            { _ThumbLabel = value; OnPropertyChanged("ThumbLabel"); }
        }
        //---------------------------------------------------------------
        #endregion
        //========================
        #region Url Text
        //---------------------------------------------------------------
        private string _UrlText;
        public string UrlText
        {
            get { return _UrlText; }
            set { _UrlText = value; isURLType();  OnPropertyChanged("UrlText"); }
        }

        private void isURLType()
        {
            if (_UrlText.Length > 0)
            { UrlLabel = System.Windows.Visibility.Hidden; }
            else if (_UrlText.Length == 0)
            { UrlLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _UrlLabel;
        public System.Windows.Visibility UrlLabel
        {
            get { return _UrlLabel; }
            set { _UrlLabel = value; OnPropertyChanged("UrlLabel"); }
        }
        //---------------------------------------------------------------
        #endregion

        private string _CountLabel;
        public string CountLabel
        {
            get { return _CountLabel; }
            set { _CountLabel = value; OnPropertyChanged("CountLabel"); }
        }


        int sum = 0, AutText = 0, DescrText = 0, FootText = 0, TitText = 0;

        void counter()
        {
            if (_AuthorText != null)
            {
                AutText = _AuthorText.Length;
            }
            if (_DescriptionText != null)
            {
                DescrText = _DescriptionText.Length;
            }
            if (_FooterText != null)
            {
                FootText = _FooterText.Length;
            }
            if (_TitleText != null)
            {
                TitText = _TitleText.Length;
            }
            sum = AutText + DescrText + FootText + TitText;
            CountLabel = sum.ToString();
            if(sum>4000)
            {
                OverflowText = System.Windows.Visibility.Visible;
                if (isChannelSelected == true)
                {
                    ButtonSendWork = false;
                }
            }
            else if(sum < 4000)
            {
                OverflowText = System.Windows.Visibility.Hidden;
                if (isChannelSelected == true)
                {
                    ButtonSendWork = true;
                }
            }
        }


        private bool _TimeCheck;
        public bool TimeCheck
        {
            get { return _TimeCheck; }
            set { _TimeCheck = value; OnPropertyChanged("TimeCheck"); }
        }

        private bool _ButtonSendWork = false;
        public bool ButtonSendWork
        {
            get { return _ButtonSendWork; }
            set { _ButtonSendWork = value;  OnPropertyChanged("ButtonSendWork"); }
        }

        private System.Windows.Visibility _OverflowText;
        public System.Windows.Visibility OverflowText
        {
            get { return _OverflowText; }
            set { _OverflowText = value; OnPropertyChanged("OverflowText"); }
        }


        private RelayCommand _sendMessage;
        public RelayCommand SendMessage
        {
            get
            {
                return _sendMessage ??
                  (_sendMessage = new RelayCommand(obj =>
                  {
                      EmbedSend.SendMessage(_selChannel.ChannelId, _SelColor.color, _DescriptionText, _TitleText, _AuthorText, _FooterText, _ImageUrlText, _ThumbText,_UrlText,_TimeCheck);
                  }));
            }
        }
        #endregion
        //-------------===EMBED===-------------


        private RelayCommand _close;
        public RelayCommand Close
        {
            get
            {
                return _close ??
                  (_close = new RelayCommand(obj =>
                  {
                      BotCore.CloseConnection();
                      Environment.Exit(0);
                  }));
            }
        }


        private RelayCommand _RestartApp;
        public RelayCommand RestartApp
        {
            get
            {
                return _RestartApp ??
                  (_RestartApp = new RelayCommand(obj =>
                  {
                      Process.Start(@$"{Environment.CurrentDirectory}\DiscordLOLader.exe");
                      Environment.Exit(0);
                  }));
            }
        }

        //-------------===Minimized===-------------
        #region Minimized
        //===========================================
        private RelayCommand _Turn;
        public RelayCommand Turn
        {
            get
            {
                return _Turn ??
                  (_Turn = new RelayCommand(obj =>
                  {
                      MinimizedOn = WindowState.Minimized;
                  }));
            }
        }

        private WindowState _MinimizedOn;
        public WindowState MinimizedOn
        {
            get { return _MinimizedOn; }
            set { _MinimizedOn = value; OnPropertyChanged("MinimizedOn"); }
        }
        //===========================================
        #endregion
        //-------------===Minimized===-------------

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
