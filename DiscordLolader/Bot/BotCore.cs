using DiscordLOLader.settings;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{

    public class BotCore
    {
        public DiscordClient Discord { get; set; }
        public DiscordGuild ConnectedGuild { get; set; }

        public string GuildName { get; set; }

        public ulong GuildId { get; set; }

        Config Config = new Config();

        public List<Guild> GuildsList = new List<Guild>();

        public ObservableCollection<Channel> ChannelList = new ObservableCollection<Channel>();


        public BotCore()
        {
            Debug.WriteLine("Bot object");
        }
        public void indication()
        {
            Debug.WriteLine("Bot object");
        }
        public bool Autorization(string token, string channel)
        {
            if(token == null || token.Length == 0)
            {
                token = "all";
            }

            var configuration = new DiscordConfiguration
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
                task.Wait(3000); //Wait for connection on             
                Thread.Sleep(1000);//Wait for connection give all information   
                if (GiveAllGuilds(channel) == true)
                {
                    GiveAllChannel();
                    GuildsList.Clear();
                    return true;
                }
                else
                {
                    Discord.DisconnectAsync();
                    GuildsList.Clear();
                    return false;
                }
            }
            catch 
            {
                GuildsList.Clear();         
                return false;
            }
        }

        public BitmapImage GiveBotThumb()
        {
            var image = new BitmapImage();
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(ConnectedGuild.IconUrl.ToString()), @$"{Environment.CurrentDirectory}\Cache\GuildIco.jpg)");
            }
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(@$"{Environment.CurrentDirectory}\Cache\GuildIco.jpg)");
            image.EndInit();
            

            return image;
        }

    


public bool GiveAllGuilds(string channel)
        {
            foreach(var guild in GuildsList)
            {
                if(guild.GuildId.ToString() == channel)
                {
                    GuildId = guild.GuildId;
                    GuildName = guild.GuildName;
                    return true;
                }
                else if (guild.GuildName == channel)
                {
                    GuildId = guild.GuildId;
                    GuildName = guild.GuildName;
                    return true;
                }
            }
            return false;
        }

        private void GiveAllChannel()
        {
            ConnectedGuild = Discord.GetGuildAsync(GuildId).Result;
            bool acess;
            Permissions check;
            ChannelType type;

            foreach (var channel in ConnectedGuild.Channels)
            {
                acess = false;
                string[] buffer = channel.Value.ToString().Split(" ");

                check = ConnectedGuild.GetMemberAsync(Discord.CurrentUser.Id).Result.PermissionsIn(channel.Value);
                type = Discord.GetChannelAsync(channel.Key).Result.Type;
                
                if (check.ToString().Contains("SendMessages") == true && check.ToString().Contains("AttachFiles") == true && check.ToString().Contains("EmbedLinks") == true)
                {
                     acess = true;
                }

                if (acess == true && buffer[1].Contains("Category") != true && type == ChannelType.Text)
                {
                    ChannelList.Add(new Channel(channel.Key, buffer[1]));
                }
            }            
        }


        private Task DiscordReady(DiscordClient sender, ReadyEventArgs e)
        {       
            return Task.CompletedTask;
        }
        private Task DiscordGuild(DiscordClient sender, GuildCreateEventArgs e)
        {
            GuildsList.Add(new Guild(e.Guild.Id, e.Guild.Name, e.Guild.BannerUrl));
            Debug.WriteLine(e.Guild.Id+ e.Guild.Name+e.Guild.IconUrl);
            return Task.CompletedTask;
        }
        public void CloseConnection()
        {
            Discord.DisconnectAsync();
        }
         

    }


}
