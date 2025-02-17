﻿using System;
using System.IO;

namespace DiscordLOLader.Functions
{
    internal class ConvertedFile
    {
        public enum FileType
        {
            Image,
            Media,
            SoundMp3,
            SoundWav,
            None
        }

        public string FilePath { get; set; }
        public long FileSize { get; set; }

        public string FileExtension { get; set; }

        public readonly string ThumbFile = $@"{Environment.CurrentDirectory}\Cache\Thumb.png";

        private readonly string CacheFile = $@"{Environment.CurrentDirectory}\Cache\Download";

        public void GetSize()
        {
            FileInfo File = new FileInfo(FilePath);
            FileSize = File.Length;
        }

        public void ConvertedFileInitialization(FileType FileType, string FilePath = "")
        {
            if (FileType == FileType.Image)
            {
                PngExtension();
            }
            else if (FileType == FileType.Media)
            {
                WebmExtension();
            }
            else if (FileType == FileType.SoundMp3)
            {
                Mp3Extension();
            }
            else if (FileType == FileType.SoundWav)
            {
                WavExtension();
            }
            else if (FileType == FileType.None)
            {
                this.FilePath = FilePath;
            }
        }

        private void PngExtension()
        {
            FileExtension = ".png";
            GetFullPath(FileExtension);
        }

        private void WebmExtension()
        {
            FileExtension = ".webm";
            GetFullPath(FileExtension);
        }

        private void Mp3Extension()
        {
            FileExtension = ".mp3";
            GetFullPath(FileExtension);
        }

        private void WavExtension()
        {
            FileExtension = ".wav";
            GetFullPath(FileExtension);
        }

        private void GetFullPath(string FileExtension)
        {
            FilePath = $"{CacheFile}{FileExtension}";
        }
    }
}
