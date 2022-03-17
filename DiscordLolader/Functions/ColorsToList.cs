using DiscordLOLader.Bot;
using DSharpPlus.Entities;
using System.Collections.ObjectModel;

namespace DiscordLOLader.Functions
{
    internal class ColorsToList
    {
        public ObservableCollection<Colors> GetColorList()
        {
            ObservableCollection<Colors> ColorsList = new ObservableCollection<Colors>
            {
                new Colors("Aquamarine", DiscordColor.Aquamarine),
                new Colors("Azure", DiscordColor.Azure),
                new Colors("Black", DiscordColor.Black),
                new Colors("Blue", DiscordColor.Blue),
                new Colors("Blurple", DiscordColor.Blurple),
                new Colors("Brown", DiscordColor.Brown),
                new Colors("Chartreuse", DiscordColor.Chartreuse),
                new Colors("CornflowerBlue", DiscordColor.CornflowerBlue),
                new Colors("Cyan", DiscordColor.Cyan),
                new Colors("DarkBlue", DiscordColor.DarkBlue),
                new Colors("DarkButNotBlack", DiscordColor.DarkButNotBlack),
                new Colors("DarkGray", DiscordColor.DarkGray),
                new Colors("DarkGreen", DiscordColor.DarkGreen),
                new Colors("DarkRed", DiscordColor.DarkRed),
                new Colors("Gold", DiscordColor.Gold),
                new Colors("Goldenrod", DiscordColor.Goldenrod),
                new Colors("Gray", DiscordColor.Gray),
                new Colors("Grayple", DiscordColor.Grayple),
                new Colors("Green", DiscordColor.Green),
                new Colors("HotPink", DiscordColor.HotPink),
                new Colors("IndianRed", DiscordColor.IndianRed),
                new Colors("LightGray", DiscordColor.LightGray),
                new Colors("Lilac", DiscordColor.Lilac),
                new Colors("Magenta", DiscordColor.Magenta),
                new Colors("MidnightBlue", DiscordColor.MidnightBlue),
                new Colors("None", DiscordColor.None),
                new Colors("NotQuiteBlack", DiscordColor.NotQuiteBlack),
                new Colors("AquaOrangemarine", DiscordColor.Orange),
                new Colors("PhthaloBlue", DiscordColor.PhthaloBlue),
                new Colors("PhthaloGreen", DiscordColor.PhthaloGreen),
                new Colors("Purple", DiscordColor.Purple),
                new Colors("Red", DiscordColor.Red),
                new Colors("Rose", DiscordColor.Rose),
                new Colors("SapGreen", DiscordColor.SapGreen),
                new Colors("Sienna", DiscordColor.Sienna),
                new Colors("SpringGreen", DiscordColor.SpringGreen),
                new Colors("Teal", DiscordColor.Teal),
                new Colors("Turquoise", DiscordColor.Turquoise),
                new Colors("VeryDarkGray", DiscordColor.VeryDarkGray),
                new Colors("Violet", DiscordColor.Violet),
                new Colors("Wheat", DiscordColor.Wheat),
                new Colors("White", DiscordColor.White),
                new Colors("Yellow", DiscordColor.Yellow)
            };
            return ColorsList;
        }
    }
}
