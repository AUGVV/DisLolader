using DiscordLOLader.Bot;
using DiscordLOLader.MVVM;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DiscordLOLader.MainCore
{
    public partial class MainModelView : INotifyPropertyChanged
    {
        private PictureSend PictureSend;
        private bool isPictureSending = false;

        private System.Windows.Threading.DispatcherTimer PictureTimer;

        private void InitPicturePartial()
        {

            PictureTimer = new System.Windows.Threading.DispatcherTimer();
            PictureTimer.Tick += PictureBarIncrement;
            PictureTimer.Interval = new TimeSpan(0, 0, 1);

            PictureSend = new PictureSend(BotCore, ConvertedFile, ThumbCreator);
            PictureSouse = Bitmap(new Uri(@"pack://application:,,,/Resources/Imager.png"));
            ImageDragDrop = true;
            WaitImageLabel = Visibility.Hidden;
            PictureSend.MessageCompleted += PictureCompleted;
        }

        private void PictureBarIncrement(object sender, EventArgs e)
        {
            PictureProgress++;
        }

        private int _PictureProgress = 0;
        public int PictureProgress
        {
            get => _PictureProgress;
            set { _PictureProgress = value; OnPropertyChanged("PictureProgress"); }
        }


        private string _PathToPicture;
        public string PathToPicture
        {
            get => _PathToPicture;
            set { _PathToPicture = value; OnPropertyChanged("PathToPicture"); if (PathToPicture != "") PreparePictureAsync(); }
        }

        private async void PreparePictureAsync()
        {
            BlockPictureButtons();
            await Task.Run(() => PictureSend.PrepareImage(PathToPicture).Wait());
            PictureSouse = ThumbCreator.GetThumb(ConvertedFile.ThumbFile, ".png");
            GetPictureData();
            UnblockPictureButtons();
        }

        private void GetPictureData()
        {
            OriginalSizeLabel = PictureSend.FileSize.ToString();
            ResultSizeLabel = ConvertedFile.FileSize.ToString();
        }


        private RelayCommand _SendPicture;
        public RelayCommand SendPicture => _SendPicture ??= new RelayCommand(obj => { SendImage(); });

        private bool _ImageDragDrop = false;
        public bool ImageDragDrop
        {
            get => _ImageDragDrop;
            set { _ImageDragDrop = value; OnPropertyChanged("ImageDragDrop"); }
        }

        private void BlockPictureButtons()
        {
            PictureTimer.Start();
            WaitImageLabel = Visibility.Visible;
            isPictureSending = true;
            ButtonImageWork = false;
            ButtonOpenWork = false;
            ImageDragDrop = false;
        }

        private void UnblockPictureButtons()
        {
            PictureTimer.Stop();
            PictureProgress = 0;
            WaitImageLabel = Visibility.Hidden;
            isPictureSending = false;
            ButtonOpenWork = true;
            ImageDragDrop = true;
            if (ButtonSendWork) { ButtonImageWork = true; }
        }

        private void SendImage()
        {
            BlockPictureButtons();
            PictureSend.RecieveImage(_SelChannel.ChannelId);
        }

        private void PictureCompleted(bool button)
        {
            UnblockPictureButtons();
        }

        private bool _ButtonImageWork = false;
        public bool ButtonImageWork
        {
            get => _ButtonImageWork;
            set { _ButtonImageWork = value; OnPropertyChanged("ButtonImageWork"); }
        }

        private string _OriginalSizeLabel = "0 byte";
        public string OriginalSizeLabel
        {
            get => _OriginalSizeLabel;
            set { _OriginalSizeLabel = value; OnPropertyChanged("OriginalSizeLabel"); }
        }

        private string _ResultSizeLabel = "0 byte";
        public string ResultSizeLabel
        {
            get => _ResultSizeLabel;
            set { _ResultSizeLabel = value; OnPropertyChanged("ResultSizeLabel"); }
        }

        private Visibility _WaitImageLabel;
        public Visibility WaitImageLabel
        {
            get => _WaitImageLabel;
            set { _WaitImageLabel = value; OnPropertyChanged("WaitImageLabel"); }
        }

        private RelayCommand _OpenPathFinder;
        public RelayCommand OpenPathFinder => _OpenPathFinder ??
                  (_OpenPathFinder = new RelayCommand(obj =>
                  {
                      PathToPicture = OpenDialog();
                  }));

        private string OpenDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Picture files (*.PNG)|*.png*|Picture files (*.JPG)|*.jpg*";
            dialog.Multiselect = false;
            return dialog.ShowDialog() == true ? dialog.FileName : "";
        }

        private bool _ButtonOpenWork = true;
        public bool ButtonOpenWork
        {
            get => _ButtonOpenWork;
            set { _ButtonOpenWork = value; OnPropertyChanged("ButtonOpenWork"); }
        }

        private ImageSource _PictureSouse;
        public ImageSource PictureSouse
        {
            get => _PictureSouse;
            set { _PictureSouse = value; OnPropertyChanged("PictureSouse"); }
        }
    }
}
