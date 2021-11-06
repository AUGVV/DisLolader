using DiscordLOLader.Functions;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiscordLOLader.Bot
{
    class MediaSend : FileToConvert
    {
        private BotCore Bot;
        private ConvertedFile ConvertedFile;
        private DiscordMessageBuilder Builder;
        private AudioCompress AudioCompress;
        private MediaCompress MediaCompress;
        private ThumbCreator ThumbCreator;

        public delegate void Handler(bool button);
        public event Handler MediaSendingCompleted;

        private const double Lock = 8388608;

        public MediaSend(BotCore Bot, ConvertedFile ConvertedFile, ThumbCreator ThumbCreator)
        {
            this.Bot = Bot;
            this.ConvertedFile = ConvertedFile;
            AudioCompress = new AudioCompress();
            MediaCompress = new MediaCompress();
            this.ThumbCreator = ThumbCreator;
            Builder = new DiscordMessageBuilder();
        }

        public Task PrepareMedia(string Path)
        {
            FileInitialization(Path);
            if (FileSize > Lock)
            {
                if (FileExtension == ".mp3" || FileExtension == ".wav")
                {
                    InitAudio();
                    AudioCompress.Mp3Compress(FilePath, ConvertedFile.FilePath);
                    ConvertedFile.GetSize();
                }
                else if (FileExtension == ".webm" || FileExtension == ".mp4")
                {
                    ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.Media);
                    ThumbCreator.CreateThumbMedia(FilePath, ConvertedFile.ThumbFile);
                    MediaCompress.WebmCompress(FilePath, ConvertedFile.FilePath);
                    ConvertedFile.GetSize();
                }
            }
            else
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.None, FilePath);
                ConvertedFile.GetSize();
                ThumbCreator.CreateThumbMedia(FilePath, ConvertedFile.ThumbFile);
            }
            return Task.CompletedTask;
        }

        private void InitAudio()
        {
            if (FileExtension == ".mp3")
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.SoundMp3);
            }
            else if (FileExtension == ".wav")
            {
                ConvertedFile.ConvertedFileInitialization(ConvertedFile.FileType.SoundWav);
            }
        }

        FileStream FileReader; 
        public void MediaFileSend(ulong channelid)
        {
            if (ConvertedFile.FilePath != "" || !File.Exists(ConvertedFile.FilePath))
            {
                FileReader = File.OpenRead(ConvertedFile.FilePath);
                _ = Builder.WithFile(FileName, FileReader);;
                _ = Bot.ConnectedGuild.GetChannel(channelid).SendMessageAsync(Builder).ContinueWith(OnEvent);
                Builder.Clear();
            }
        }

        private void OnEvent(Task t)
        {
            FileReader.Close();
            MediaSendingCompleted?.Invoke(true);
        }
    }
}
