using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DiscordLOLader.Functions
{
    class ConvertedFile
    {
        public enum FileType
        {
            Image,
            Media,
            Sound,
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
            else if (FileType == FileType.Sound)
            {
                Mp3Extension();
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

        private void GetFullPath(string FileExtension)
        {
            FilePath = $"{CacheFile}{FileExtension}";
            Debug.WriteLine(FilePath);
        }
    }
}
