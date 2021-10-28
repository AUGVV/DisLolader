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
        private string _TitleText;
        public string TitleText
        {
            get { return _TitleText; }
            set { _TitleText = value; isTitleType(); counter(); OnPropertyChanged("TitleText"); }
        }

        private void isTitleType()
        {
            if (_TitleText.Length > 0)
            { TitleLabel = System.Windows.Visibility.Hidden; }
            else if (_TitleText.Length == 0)
            { TitleLabel = System.Windows.Visibility.Visible; }
        }

        private System.Windows.Visibility _TitleLabel;
        public System.Windows.Visibility TitleLabel
        {
            get { return _TitleLabel; }
            set { _TitleLabel = value; OnPropertyChanged("TitleLabel"); }
        }

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

        private string _DescriptionText;
        public string DescriptionText
        {
            get { return _DescriptionText; }
            set { _DescriptionText = value; isDescriprionType(); counter(); OnPropertyChanged("DescriptionText"); }
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

        private string _ImageUrlText;
        public string ImageUrlText
        {
            get { return _ImageUrlText; }
            set
            { _ImageUrlText = value; isImageURLType(); OnPropertyChanged("ImageUrlText"); }
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

        private string _UrlText;
        public string UrlText
        {
            get { return _UrlText; }
            set { _UrlText = value; isURLType(); OnPropertyChanged("UrlText"); }
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
            if (sum > 4000)
            {
                OverflowText = System.Windows.Visibility.Visible;
                if (isChannelSelected == true)
                {
                    ButtonSendWork = false;
                }
            }
            else if (sum < 4000)
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
            set { _ButtonSendWork = value; OnPropertyChanged("ButtonSendWork"); }
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
                      EmbedSend.SendMessage(_SelChannel.ChannelId, _SelColor.color, _DescriptionText, _TitleText, _AuthorText, _FooterText, _ImageUrlText, _ThumbText, _UrlText, _TimeCheck);
                  }));
            }
        }
    }
}
