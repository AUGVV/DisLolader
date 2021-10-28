using DiscordLOLader.Bot;
using DiscordLOLader.MVVM;
using DiscordLOLader.settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Windows.Input;

namespace DiscordLOLader.AutorizationCore
{
    class AutorizationViewModel : INotifyPropertyChanged
    {
        private BotCore BotCore;
        private MainWindow Main;
        Config Configs = new Config();


        public AutorizationViewModel(BotCore BotRecieved, MainWindow MainRecieved)
        {
            BotCore = BotRecieved;
            Main = MainRecieved;
            ErrorLabel = System.Windows.Visibility.Hidden;

            _login = Configs.Channel;
            _token = Configs.Token;
            _loginEnable = true;
            isBotTokenType();
            isBotLoginType();
        }

        private RelayCommand _logIn;
        public RelayCommand LogIn
        {
            get
            {
                return _logIn ??
                  (_logIn = new RelayCommand(obj =>
                  {
                      TryLogin();
                  }));
            }
        }

        private RelayCommand _close;
        public RelayCommand Close
        {
            get
            {
                return _close ??
                  (_close = new RelayCommand(obj =>
                  {
                      Environment.Exit(0);
                  }));
            }
        }


        private bool _loginEnable;
        public bool LoginEnable
        {
            get { return _loginEnable; }
            set
            {
                _loginEnable = value;
                Debug.WriteLine(_loginEnable);
                OnPropertyChanged("LoginEnable");
            }
        }


        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                Debug.WriteLine(_checked);
                OnPropertyChanged("Checked");
            }
        }

        private string _token;
        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                isBotTokenType();
                OnPropertyChanged("Token");
            }
        }

        private void isBotTokenType()
        {
            if (_token.Length > 0)
            {
                BotToken = System.Windows.Visibility.Hidden;
            }
            else if (_token.Length == 0)
            {
                BotToken = System.Windows.Visibility.Visible;
            }
        }

        private void isBotLoginType()
        {
            if (_login.Length > 0)
            {
                ChannelText = System.Windows.Visibility.Hidden;
            }
            else if (_login.Length == 0)
            {
                ChannelText = System.Windows.Visibility.Visible;
            }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                isBotLoginType();
                OnPropertyChanged("Token");
            }
        }

        private System.Windows.Visibility _channelText;
        public System.Windows.Visibility ChannelText
        {
            get { return _channelText; }
            set
            {
                _channelText = value;
                OnPropertyChanged("ChannelText");
            }
        }

        private System.Windows.Visibility _errorLabel;
        public System.Windows.Visibility ErrorLabel
        {
            get { return _errorLabel; }
            set
            {
                _errorLabel = value;
                OnPropertyChanged("ErrorLabel");
            }
        }

   


        private System.Windows.Visibility _botToken;
        public System.Windows.Visibility BotToken
        {
            get { return _botToken; }
            set
            {
                _botToken = value;
                OnPropertyChanged("BotToken");
            }
        }


        public void TryLogin()
        {
           Debug.WriteLine($"Try connect token={Token} Login={Login}");
           _loginEnable = false;
           bool isGood = BotCore.Autorization(Token, Login);
           if(!isGood)
           {
                ErrorLabel = System.Windows.Visibility.Visible;
           }
           else
           {
                if (_checked)
                {
                    Configs.WriteNewData(Token, Login);

                }
                ErrorLabel = System.Windows.Visibility.Hidden;
                Main.AutorizationSucsses();            
           }
            _loginEnable = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
