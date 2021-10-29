using DiscordLOLader.Bot;
using DiscordLOLader.MVVM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DiscordLOLader.MainCore
{
    public partial class MainModelView : INotifyPropertyChanged
    {

        private EmbedSend EmbedSend;
        public ObservableCollection<Colors> Color { get; set; }

        private void InitEmbedPartial()
        {
            EmbedSend = new EmbedSend(BotCore);
            Color = EmbedSend.ColorsList;
            CountLabel = "0";
            OverflowText = Visibility.Hidden;
        }

        private string _TitleText;
        public string TitleText
        {
            get { return _TitleText; }
            set { _TitleText = value; IsTitleType(); EmbedSend.Title = value; Counter(); OnPropertyChanged("TitleText"); }
        }

        private void IsTitleType()
        {
            if (_TitleText.Length > 0)
            { TitleLabel = Visibility.Hidden; }
            else if (_TitleText.Length == 0)
            { TitleLabel = Visibility.Visible; }
        }

        private Visibility _TitleLabel;
        public Visibility TitleLabel
        {
            get { return _TitleLabel; }
            set { _TitleLabel = value; OnPropertyChanged("TitleLabel"); }
        }

        private string _AuthorText;
        public string AuthorText
        {
            get { return _AuthorText; }
            set { _AuthorText = value; isAuthorType(); EmbedSend.Author = value; Counter(); OnPropertyChanged("AuthorText"); }
        }

        private void isAuthorType()
        {
            if (_AuthorText.Length > 0)
            { AuthorLabel = Visibility.Hidden; }
            else if (_AuthorText.Length == 0)
            { AuthorLabel = Visibility.Visible; }
        }

        private Visibility _AuthorLabel;
        public Visibility AuthorLabel
        {
            get { return _AuthorLabel; }
            set { _AuthorLabel = value; OnPropertyChanged("AuthorLabel"); }
        }

        private string _DescriptionText;
        public string DescriptionText
        {
            get { return _DescriptionText; }
            set { _DescriptionText = value; IsDescriprionType(); EmbedSend.MainText = value; Counter(); OnPropertyChanged("DescriptionText"); }
        }

        private void IsDescriprionType()
        {
            if (_DescriptionText.Length > 0)
            { DescriptionLabel = Visibility.Hidden; }
            else if (_DescriptionText.Length == 0)
            { DescriptionLabel = Visibility.Visible; }
        }

        private Visibility _DescriptionLabel;
        public Visibility DescriptionLabel
        {
            get { return _DescriptionLabel; }
            set { _DescriptionLabel = value; OnPropertyChanged("DescriptionLabel"); }
        }

        private string _FooterText;
        public string FooterText
        {
            get { return _FooterText; }
            set { _FooterText = value; IsFooterType(); EmbedSend.Footer = value; Counter(); OnPropertyChanged("FooterText"); }
        }

        private void IsFooterType()
        {
            if (_FooterText.Length > 0)
            { FooterLabel = Visibility.Hidden; }
            else if (_FooterText.Length == 0)
            { FooterLabel = Visibility.Visible; }
        }

        private Visibility _FooterLabel;
        public Visibility FooterLabel
        {
            get { return _FooterLabel; }
            set { _FooterLabel = value; OnPropertyChanged("FooterLabel"); }
        }

        private string _ImageUrlText;
        public string ImageUrlText
        {
            get { return _ImageUrlText; }
            set
            { _ImageUrlText = value; IsImageURLType(); EmbedSend.ImageUrl = value; OnPropertyChanged("ImageUrlText"); }
        }

        private void IsImageURLType()
        {
            if (_ImageUrlText.Length > 0)
            { ImageUrlLabel = Visibility.Hidden; }
            else if (_ImageUrlText.Length == 0)
            { ImageUrlLabel = Visibility.Visible; }
        }

        private Visibility _ImageUrlLabel;
        public Visibility ImageUrlLabel
        {
            get { return _ImageUrlLabel; }
            set { _ImageUrlLabel = value; OnPropertyChanged("ImageUrlLabel"); }
        }

        private string _ThumbText;
        public string ThumbText
        {
            get { return _ThumbText; }
            set { _ThumbText = value; isThumbType(); EmbedSend.Thumbnail = value; OnPropertyChanged("ThumbText"); }
        }

        private void isThumbType()
        {
            if (_ThumbText.Length > 0)
            { ThumbLabel = Visibility.Hidden; }
            else if (_ThumbText.Length == 0)
            { ThumbLabel = Visibility.Visible; }
        }

        private Visibility _ThumbLabel;
        public Visibility ThumbLabel
        {
            get { return _ThumbLabel; }
            set
            { _ThumbLabel = value; OnPropertyChanged("ThumbLabel"); }
        }

        private string _UrlText;
        public string UrlText
        {
            get { return _UrlText; }
            set { _UrlText = value; IsURLType(); EmbedSend.WithUrl = value; OnPropertyChanged("UrlText"); }
        }

        private void IsURLType()
        {
            if (_UrlText.Length > 0)
            { UrlLabel = Visibility.Hidden; }
            else if (_UrlText.Length == 0)
            { UrlLabel = Visibility.Visible; }
        }

        private Visibility _UrlLabel;
        public Visibility UrlLabel
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


        int Sum = 0, AutText = 0, DescrText = 0, FootText = 0, TitText = 0;

        void Counter()
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
            Sum = AutText + DescrText + FootText + TitText;
            CountLabel = Sum.ToString();
            if (Sum > 4000)
            {
                OverflowText = Visibility.Visible;
                if (isChannelSelected)
                    ButtonSendWork = false;
            }
            else if (Sum < 4000)
            {
                OverflowText = Visibility.Hidden;
                if (isChannelSelected)
                    ButtonSendWork = true;
            }
        }

        private bool _TimeCheck;
        public bool TimeCheck
        {
            get => _TimeCheck;
            set { _TimeCheck = value; EmbedSend.Time = value; OnPropertyChanged("TimeCheck"); }
        }

        private bool _ButtonSendWork = false;
        public bool ButtonSendWork
        {
            get { return _ButtonSendWork; }
            set { _ButtonSendWork = value; OnPropertyChanged("ButtonSendWork"); }
        }

        private Visibility _OverflowText;
        public Visibility OverflowText
        {
            get { return _OverflowText; }
            set { _OverflowText = value; OnPropertyChanged("OverflowText"); }
        }

        private RelayCommand _sendMessage;
        public RelayCommand SendMessage => _sendMessage ??= new RelayCommand(obj => 
        {
            EmbedSend.SendMessage(_SelChannel.ChannelId, _SelColor.color); 
            UrlText = EmbedSend.WithUrl; 
            ImageUrlText = EmbedSend.ImageUrl;
            ThumbText = EmbedSend.Thumbnail;
        });
    }
}
