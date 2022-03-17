using DiscordLOLader.Bot;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace DiscordLOLader.Functions
{
    internal class BotControl
    {
        private readonly BotCore Bot;

        public BotControl(BotCore Bot)
        {
            this.Bot = Bot;
        }

        public async Task ChangeStateAsync(string State)
        {
            if (State == "В сети")
            {
                await Bot.Discord.UpdateStatusAsync(null, UserStatus.Online);
            }
            else if (State == "Неактивен")
            {
                await Bot.Discord.UpdateStatusAsync(null, UserStatus.Idle);
            }
            else if (State == "Не беспокоить")
            {
                await Bot.Discord.UpdateStatusAsync(null, UserStatus.DoNotDisturb);
            }
            else if (State == "Невидимый")
            {
                await Bot.Discord.UpdateStatusAsync(null, UserStatus.Invisible);
            }
        }
    }
}
