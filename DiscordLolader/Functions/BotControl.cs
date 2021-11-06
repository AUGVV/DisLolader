using DiscordLOLader.Bot;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DiscordLOLader.Functions
{
    class BotControl
    {

        BotCore Bot;

        public BotControl(BotCore Bot)
        {
            this.Bot = Bot;
        }
            
        public void ChangeState(string State)
        {
            if(State == "В сети")
            {
                _ = Bot.Discord.UpdateStatusAsync(null, UserStatus.Online);
            }
            else if(State == "Неактивен")
            {
                _ = Bot.Discord.UpdateStatusAsync(null, UserStatus.Idle);
            }
            else if(State == "Не беспокоить")
            {
                _ = Bot.Discord.UpdateStatusAsync(null, UserStatus.DoNotDisturb);
            }
            else if(State == "Невидимый")
            {
                _ = Bot.Discord.UpdateStatusAsync(null, UserStatus.Invisible);
            }
        }
    }
}
