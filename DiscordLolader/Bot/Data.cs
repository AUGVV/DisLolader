using DSharpPlus.Entities;
using System.Windows.Media;

namespace DiscordLOLader.Bot
{
    public struct Guild
    {
        public Guild(ulong GuildId, string GuildName, string IconPath)
        {
            this.GuildId = GuildId;
            this.IconPath = IconPath;
            this.GuildName = GuildName;
        }

        public ulong GuildId;
        public string GuildName;
        public string IconPath;
    }

    public struct Channel
    {
        public Channel(ulong ChannelId, string ChannelName)
        {
            this.ChannelId = ChannelId;
            this.ChannelName = ChannelName;
        }

        public ulong ChannelId { get; set; }
        public string ChannelName { get; set; }
    }

    public struct Colors
    {
        public SolidColorBrush ColorFill { get; set; }
        public DiscordColor Color { get; set; }
        public string ColorName  { get; set; }

        public Colors(string ColorName,  DiscordColor color)
        {
            this.ColorName = ColorName;
            Color = color;
            ColorFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
        }
    }
}
