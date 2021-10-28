using DiscordLOLader.settings;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{

    public class BotCore
    {
        public DiscordClient Discord { get; set; }

        public DiscordGuild ConnectedGuild { get; set; }

        public string GuildName { get; set; }

        public ulong GuildId { get; set; }


        public List<Guild> GuildsList = new List<Guild>();


        public ObservableCollection<Channel> ChannelList = new ObservableCollection<Channel>();


        public BotCore()
        {
            Debug.WriteLine("Bot object");
        }
        public void Indication()
        {
            Debug.WriteLine("Bot object");
        }
        public bool Autorization(string token, string channel)
        {
            if(token == null || token.Length == 0)
            {
                token = "all";
            }

            DiscordConfiguration configuration = new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };
            Discord = new DiscordClient(configuration);
            Discord.Ready += DiscordReady;
            Discord.GuildAvailable += DiscordGuild;
            Task task = Discord.ConnectAsync();
            try
            {
                task.Wait();
                Thread.Sleep(2000);
                    if (isGuildReal(channel))
                    {
                        GetAllChannels();
                        GuildsClear();
                        return true;
                    }
                    else
                    {
                        Discord.DisconnectAsync();
                        GuildsClear();
                        return false;
                    }
            }
            catch
            {
                GuildsClear();
                return false;
            }
        }

        private void GuildsClear()
        {
            GuildsList.Clear();
        }

        public BitmapImage GetGuildThumb()
        {
            using (WebClient Client = new WebClient())
            {
                Client.DownloadFile(new Uri(ConnectedGuild.IconUrl.ToString()), @$"{Environment.CurrentDirectory}\Cache\GuildIco.jpg)");
            }
            return GetGuildPicture(new BitmapImage());
        }

        private BitmapImage GetGuildPicture(BitmapImage GuildImage)
        {
            GuildImage.BeginInit();
            GuildImage.CacheOption = BitmapCacheOption.OnLoad;
            GuildImage.UriSource = new Uri(@$"{Environment.CurrentDirectory}\Cache\GuildIco.jpg)");
            GuildImage.EndInit();
            return GuildImage;
        }

        public bool isGuildReal(string Channel)
        {
            foreach(Guild Guild in GuildsList)
            {
                if(isGuildId(Channel, Guild))
                {
                    return true;
                }
                else if (isGuildName(Channel, Guild))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isGuildId(string Channel, Guild Guild)
        {
            if (Guild.GuildId.ToString() == Channel)
            {
                SetGuildName(Guild);
                return true;
            }
            return false;
        }

        private bool isGuildName(string Channel, Guild Guild)
        {
            if (Guild.GuildName == Channel)
            {
                SetGuildName(Guild);
                return true;
            }
            return false;
        }

        private void SetGuildName(Guild Guild)
        {
            GuildId = Guild.GuildId;
            GuildName = Guild.GuildName;
        }

        private void GetAllChannels()
        {
            ConnectedGuild = Discord.GetGuildAsync(GuildId).Result;
            foreach (KeyValuePair<ulong, DiscordChannel> Channel in ConnectedGuild.Channels)
            {
                AddChanelToList(Channel);
            }
        }

        private void AddChanelToList(KeyValuePair<ulong, DiscordChannel> Channel)
        {
            string ChannelPermissons = GetChannelPermission(Channel);
            ChannelType ChannelType = GetChannelType(Channel);

            if (ChannelType == ChannelType.Text && ChannelPermissons.Contains("Send messages") && ChannelPermissons.Contains("Attach files") && ChannelPermissons.Contains("Use embeds"))
            {
                ChannelList.Add(new Channel(Channel.Key, Channel.Value.Name));
            }
        }

        private ChannelType GetChannelType(KeyValuePair<ulong, DiscordChannel> Channel)
        {
            return Discord.GetChannelAsync(Channel.Key).Result.Type;
        }

        private string GetChannelPermission(KeyValuePair<ulong, DiscordChannel> Channel)
        {
            return ConnectedGuild.GetMemberAsync(Discord.CurrentUser.Id).Result.PermissionsIn(Channel.Value).ToPermissionString();
        }

        private Task DiscordReady(DiscordClient sender, ReadyEventArgs e)
        {       
            return Task.CompletedTask;
        }

        private Task DiscordGuild(DiscordClient sender, GuildCreateEventArgs e)
        {
            GuildsList.Add(new Guild(e.Guild.Id, e.Guild.Name, e.Guild.BannerUrl));
            return Task.CompletedTask;
        }

        public void CloseConnection()
        {
            Discord.DisconnectAsync();
        }
    }
}
