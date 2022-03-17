using DiscordLOLader.Functions;
using DSharpPlus.Entities;
using System.IO;
using System.Threading.Tasks;

namespace DiscordLOLader.Bot
{
    internal class MediaSend : FileToConvert
    {
        private const double Lock = 8388608;

        private readonly BotCore Bot;
        private readonly ConvertedFile ConvertedFile;
        private readonly DiscordMessageBuilder Builder;
        private readonly AudioCompress AudioCompress;
        private readonly MediaCompress MediaCompress;
        private readonly ThumbCreator ThumbCreator;

        public delegate void Handler(bool button);
        public event Handler MediaSendingCompleted;

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

        private FileStream FileReader;
        public void MediaFileSend(ulong channelid)
        {
            if (ConvertedFile.FilePath != "" || !File.Exists(ConvertedFile.FilePath))
            {
                FileReader = File.OpenRead(ConvertedFile.FilePath);
                _ = Builder.WithFile(FileName, FileReader);
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
