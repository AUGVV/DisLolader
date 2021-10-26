using DiscordLOLader.Bot;
using DiscordLOLader.MainCore;
using DiscordLOLader.settings;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiscordLOLader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BotCore Bot { get; set; }

        private Autorization Autorization;

        public MainWindow()
        {
            InitializeComponent();
            autorization();
        }

        private void autorization()
        {
            Bot = new BotCore();
            MainWindow1.Visibility = Visibility.Hidden;
            Autorization = new Autorization(Bot, MainWindow1);
            Autorization.Show();
        }
        public void AutorizationSucsses()
        {
            Autorization.Close();
            DataContext = new MainModelView(Bot);
            MainWindow1.Visibility = Visibility.Visible;

        }


        private void MainWindow1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex) { Debug.WriteLine("Drag&Move problem: " + ex); }
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            string[] folderPath = (string[])e.Data.GetData(DataFormats.FileDrop);
            string[] Check = folderPath[0].Split(@".");
            if (folderPath[0] != TxtBox.Text)
            {
                if (Check[Check.Length - 1].ToLower() == "png" || Check[Check.Length - 1].ToLower() == "jpg")
                {
                    TxtBox.Text = folderPath[0];
                }
            }
        }

        private void Image_Drop_1(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            string[] folderPath = (string[])e.Data.GetData(DataFormats.FileDrop);
            string[] Check = folderPath[0].Split(@".");
            if (folderPath[0] != TxtBox.Text)
            {
                if (Check[Check.Length - 1].ToLower() == "webm" || Check[Check.Length - 1].ToLower() == "mp4" || Check[Check.Length - 1].ToLower() == "mp3")
                {
                    TextVidAudBox.Text = folderPath[0];
                }
            }
        }
    }
}

