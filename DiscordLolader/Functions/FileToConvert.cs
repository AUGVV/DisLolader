﻿using System.IO;

namespace DiscordLOLader.Functions
{
    internal abstract class FileToConvert
    {
        protected internal string FilePath { get; set; }

        protected internal long FileSize { get; set; }

        protected internal string FileName { get; set; }

        protected internal string FileExtension { get; set; }

        protected internal void FileInitialization(string Path)
        {
            FileInfo File = new FileInfo(Path);
            FilePath = File.FullName;
            FileSize = File.Length;
            FileName = File.Name;
            FileExtension = File.Extension;
        }
    }
}
