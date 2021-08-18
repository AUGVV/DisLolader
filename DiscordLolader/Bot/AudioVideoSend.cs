using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DiscordLOLader.Bot
{
    class AudioVideoSend
    {

        BotCore Bot;

        DiscordMessageBuilder Builder;

        public delegate void Handler(bool button);
        public event Handler MessageCompleted;

        public AudioVideoSend(BotCore BotRecieved)
        {
            Bot = BotRecieved;
            Builder = new DiscordMessageBuilder();

        }



        public void PrepareAudioVideo(string path)
        {



            var Music_configs = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {path} -c:v libvpx -b:v 1450 {Environment.CurrentDirectory}\Cache\Download.webm",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var Input = Process.Start(Music_configs);
            Input.WaitForExit();


          //  ffmpeg - ss 00:00:02 - i "file.flv" - f image2 - vframes 1 "file_out.jpg" картинка из видоса



        }



























    }
}
