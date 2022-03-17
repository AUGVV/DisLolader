using DiscordLOLader.Bot;
using DiscordLOLader.MVVM;
using DiscordLOLader.settings;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiscordLOLader.AutorizationCore
{
    internal class AutorizationViewModel : INotifyPropertyChanged
    {
        private readonly BotCore BotCore;
        private readonly MainWindow Main;
        private readonly Config Configs = new Config();

        public AutorizationViewModel(BotCore BotRecieved, MainWindow MainRecieved)
        {
            BotCore = BotRecieved;
            Main = MainRecieved;
            ErrorLabel = System.Windows.Visibility.Hidden;

            _login = Configs.Channel;
            _token = Configs.Token;
            _loginEnable = true;
            IsBotTokenType();
            IsBotLoginType();
        }

        private RelayCommand _logIn;
        public RelayCommand LogIn => _logIn ??= new RelayCommand(obj =>
                  {
                      TryLogin();
                  });

        private RelayCommand _close;
        public RelayCommand Close => _close ??= new RelayCommand(obj =>
                  {
                      Environment.Exit(0);
                  });

        private bool _loginEnable;
        public bool LoginEnable
        {
            get => _loginEnable;
            set
            {
                _loginEnable = value;
                OnPropertyChanged("LoginEnable");
            }
        }

        private bool _checked;
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                OnPropertyChanged("Checked");
            }
        }

        private string _token;
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                IsBotTokenType();
                OnPropertyChanged("Token");
            }
        }

        private void IsBotTokenType()
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

        private void IsBotLoginType()
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
            get => _login;
            set
            {
                _login = value;
                IsBotLoginType();
                OnPropertyChanged("Token");
            }
        }

        private System.Windows.Visibility _channelText;
        public System.Windows.Visibility ChannelText
        {
            get => _channelText;
            set
            {
                _channelText = value;
                OnPropertyChanged("ChannelText");
            }
        }

        private System.Windows.Visibility _errorLabel;
        public System.Windows.Visibility ErrorLabel
        {
            get => _errorLabel;
            set
            {
                _errorLabel = value;
                OnPropertyChanged("ErrorLabel");
            }
        }

        private System.Windows.Visibility _botToken;
        public System.Windows.Visibility BotToken
        {
            get => _botToken;
            set
            {
                _botToken = value;
                OnPropertyChanged("BotToken");
            }
        }

        public void TryLogin()
        {
           _loginEnable = false;
           bool isGood = BotCore.AutorizationAsync(Token, Login);
           if (!isGood)
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
