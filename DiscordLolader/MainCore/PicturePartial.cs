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

        private string _PathToPicture;
        public string PathToPicture
        {
            get { return _PathToPicture; }
            set { _PathToPicture = value; PreparePicture(_PathToPicture); OnPropertyChanged("PathToPicture"); }
        }

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

        private bool _ImageDragDrop = false;
        public bool ImageDragDrop
        {
            get { return _ImageDragDrop; }
            set { _ImageDragDrop = value; OnPropertyChanged("ImageDragDrop"); }
        }

        private void SendImage()
        {
            ButtonImageWork = false;
            ButtonOpenWork = false;
            ImageDragDrop = false;
            isSending = true;
            WaitImageLabel = System.Windows.Visibility.Visible;
            PictureSend.RecieveImage(RefDownloadPath, _SelChannel.ChannelId);
        }

        private void PictureCompleted(bool button)
        {
            ButtonImageWork = button;
            ImageDragDrop = button;
            isSending = false;
            ButtonOpenWork = true;
            WaitImageLabel = System.Windows.Visibility.Hidden;
        }

        private bool _ButtonImageWork = false;
        public bool ButtonImageWork
        {
            get { return _ButtonImageWork; }
            set { _ButtonImageWork = value; OnPropertyChanged("ButtonImageWork"); }
        }


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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Picture files (*.PNG)|*.png*|Picture files (*.JPG)|*.jpg*";
            dialog.Multiselect = false;
            return dialog.ShowDialog() == true ? dialog.FileName : "";
        }

        private bool _ButtonOpenWork = true;
        public bool ButtonOpenWork
        {
            get { return _ButtonOpenWork; }
            set { _ButtonOpenWork = value; OnPropertyChanged("ButtonOpenWork"); }
        }
    }
}
