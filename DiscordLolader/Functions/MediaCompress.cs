using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DiscordLOLader.Functions
{
    class MediaCompress
    {
        public void WebmCompress(string FilePath, string CacheFile)
        {
            ProcessStartInfo Video_config = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i {FilePath} -y -c:v libvpx-vp9 -quality realtime -speed 15 -b:v 150K -maxrate 150K  -s 401x255 -r 22 -crf 4 -c:a libopus -b:a 96k {CacheFile}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process Input = Process.Start(Video_config);
            Input.WaitForExit();
        }
    }
}
