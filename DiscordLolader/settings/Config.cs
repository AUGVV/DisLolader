using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace DiscordLOLader.settings
{
    public class Config
    {
        public string Token { private set; get; }

        public string Channel { private set; get; }


        public Config()
        {
            try
            {
                string json = string.Empty;
                json = File.ReadAllText(@$"{Environment.CurrentDirectory}\Settings\config.json");
                JsonConfig Result = JsonConvert.DeserializeObject<JsonConfig>(json);
                Token = Result.Token;
                Channel = Result.Channel;
            }
            catch
            {
                MessageBox.Show("Json not found");
            }
        }

        public void WriteNewData(string token, string channel)
        {
            try
            {
                JsonConfig json = new JsonConfig();
                json.Token = token;
                json.Channel = channel;
                string forFile = JsonConvert.SerializeObject(json);
                File.WriteAllText(@$"{Environment.CurrentDirectory}\Settings\config.json", forFile);

            }
            catch
            {
                MessageBox.Show("Json not found");
            }
        }
    }

    public struct JsonConfig
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("Channel")]
        public string Channel { get; set; }
    }
}
