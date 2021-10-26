using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace DiscordLOLader.Bot
{
    class EmbedSend
    {
        BotCore Bot;
        DiscordChannel Channel;
        public ObservableCollection<Colors> ColorsList = new ObservableCollection<Colors>();

        public EmbedSend(BotCore BotRecieved)
        {
            Bot = BotRecieved;
            PrepareColors();
        }

        private void PrepareColors()
        {
            ColorsList.Add(new Colors("Aquamarine", DiscordColor.Aquamarine));
            ColorsList.Add(new Colors("Azure", DiscordColor.Azure));
            ColorsList.Add(new Colors("Black", DiscordColor.Black));
            ColorsList.Add(new Colors("Blue", DiscordColor.Blue));
            ColorsList.Add(new Colors("Blurple", DiscordColor.Blurple));
            ColorsList.Add(new Colors("Brown", DiscordColor.Brown));
            ColorsList.Add(new Colors("Chartreuse", DiscordColor.Chartreuse));
            ColorsList.Add(new Colors("CornflowerBlue", DiscordColor.CornflowerBlue));
            ColorsList.Add(new Colors("Cyan", DiscordColor.Cyan));
            ColorsList.Add(new Colors("DarkBlue", DiscordColor.DarkBlue));
            ColorsList.Add(new Colors("DarkButNotBlack", DiscordColor.DarkButNotBlack));
            ColorsList.Add(new Colors("DarkGray", DiscordColor.DarkGray));
            ColorsList.Add(new Colors("DarkGreen", DiscordColor.DarkGreen));
            ColorsList.Add(new Colors("DarkRed", DiscordColor.DarkRed));
            ColorsList.Add(new Colors("Gold", DiscordColor.Gold));
            ColorsList.Add(new Colors("Goldenrod", DiscordColor.Goldenrod));
            ColorsList.Add(new Colors("Gray", DiscordColor.Gray));
            ColorsList.Add(new Colors("Grayple", DiscordColor.Grayple));
            ColorsList.Add(new Colors("Green", DiscordColor.Green));
            ColorsList.Add(new Colors("HotPink", DiscordColor.HotPink));
            ColorsList.Add(new Colors("IndianRed", DiscordColor.IndianRed));
            ColorsList.Add(new Colors("LightGray", DiscordColor.LightGray));
            ColorsList.Add(new Colors("Lilac", DiscordColor.Lilac));
            ColorsList.Add(new Colors("Magenta", DiscordColor.Magenta));
            ColorsList.Add(new Colors("MidnightBlue", DiscordColor.MidnightBlue));
            ColorsList.Add(new Colors("None", DiscordColor.None));
            ColorsList.Add(new Colors("NotQuiteBlack", DiscordColor.NotQuiteBlack));
            ColorsList.Add(new Colors("AquaOrangemarine", DiscordColor.Orange));
            ColorsList.Add(new Colors("PhthaloBlue", DiscordColor.PhthaloBlue));
            ColorsList.Add(new Colors("PhthaloGreen", DiscordColor.PhthaloGreen));
            ColorsList.Add(new Colors("Purple", DiscordColor.Purple));
            ColorsList.Add(new Colors("Red", DiscordColor.Red));
            ColorsList.Add(new Colors("Rose", DiscordColor.Rose));
            ColorsList.Add(new Colors("SapGreen", DiscordColor.SapGreen));
            ColorsList.Add(new Colors("Sienna", DiscordColor.Sienna));
            ColorsList.Add(new Colors("SpringGreen", DiscordColor.SpringGreen));
            ColorsList.Add(new Colors("Teal", DiscordColor.Teal));
            ColorsList.Add(new Colors("Turquoise", DiscordColor.Turquoise));
            ColorsList.Add(new Colors("VeryDarkGray", DiscordColor.VeryDarkGray));
            ColorsList.Add(new Colors("Violet", DiscordColor.Violet));
            ColorsList.Add(new Colors("Wheat", DiscordColor.Wheat));
            ColorsList.Add(new Colors("White", DiscordColor.White));
            ColorsList.Add(new Colors("Yellow", DiscordColor.Yellow));
        }

        public void SendMessage(ulong channelid, DiscordColor DisColor, string MainText="", string Title="", string Author = "", string Footer = "", string ImageUrl = "", string Thumbnail = "", string WithUrl = "", bool Time = false)
        {
            var embed = new DiscordEmbedBuilder();

            if (MainText != null) { embed.WithDescription(MainText); }

            if (Title != null) { embed.WithTitle(Title); }

            if (Author != null) { embed.WithAuthor(Author); }

            if (Footer != null) { embed.WithFooter(Footer); }

            if (ImageUrl != null) { try { embed.WithImageUrl(ImageUrl); } catch { } }

            if (Thumbnail != null) { try { embed.WithThumbnail(Thumbnail); } catch { } }

            if (WithUrl != null) { try { embed.WithUrl(WithUrl); } catch { } }

            if (Time == true) { embed.WithTimestamp(DateTime.Now); }

            embed.WithColor(DisColor);

            if (embed != null)
            {
                Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(embed);
            }
        }
    }
}
