using DiscordLOLader.Functions;
using DSharpPlus.Entities;
using System;
using System.Collections.ObjectModel;

namespace DiscordLOLader.Bot
{
    internal class EmbedSend
    {
        private readonly BotCore Bot;
        public ObservableCollection<Colors> ColorsList { get; private set; }

        public string MainText { get; set; } = "";
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string Footer { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Thumbnail { get; set; } = "";
        public string WithUrl { get; set; } = "";
        public bool Time { get; set; } = false;

        public EmbedSend(BotCore BotRecieved)
        {
            Bot = BotRecieved;
            ColorsList = new ColorsToList().GetColorList();
        }

        public void SendMessage(ulong channelid, DiscordColor DisColor)
        {
            DiscordEmbedBuilder Embed = new DiscordEmbedBuilder();

            if (MainText != null) { _ = Embed.WithDescription(MainText); }

            if (Title != null) { _ = Embed.WithTitle(Title); }

            if (Author != null) { _ = Embed.WithAuthor(Author); }

            if (Footer != null) { _ = Embed.WithFooter(Footer); }

            if (ImageUrl != null) { try { _ = Embed.WithImageUrl(ImageUrl); } catch { ImageUrl = "Ошибка"; } }

            if (Thumbnail != null) { try { _ = Embed.WithThumbnail(Thumbnail); } catch { Thumbnail = "Ошибка"; } }

            if (WithUrl != null) { try { _ = Embed.WithUrl(WithUrl); } catch { WithUrl = "Ошибка"; } }

            if (Time) { _ = Embed.WithTimestamp(DateTime.Now); }

            _ = Embed.WithColor(DisColor);

            if (Embed != null)
            {
                _ = Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(Embed);
            }
        }
    }
}
