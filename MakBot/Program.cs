using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MakBot
{
    class Program
    {

        public static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("botconfig.json");

            configuration = builder.Build();

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();

            client.Log += Log;

            client.MessageReceived += MessageReceived;

            string myToken = configuration["tokens:discord"];
            await client.LoginAsync(TokenType.Bot, myToken);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage m)
        {
            Console.WriteLine(m.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage msg)
        {
            if (msg.Content.Equals("!ping"))
            {
                await msg.Channel.SendMessageAsync("Pong!");
            }
            else if (msg.Content.Equals("!status"))
            {
                TwitchAccess twitchAccess = new TwitchAccess(configuration["tokens:twitchClientID"]); // ULTRA TODO: hide the client-id
                var stream = await twitchAccess.GetStreamInfoAsync();
                var isUp = twitchAccess.IsStreamUp(stream);

                if (isUp)
                    await msg.Channel.SendMessageAsync("MAK IS ONLINE AND DOMINATING");
                else
                    await msg.Channel.SendMessageAsync("Mak isn't online :(");

            }
            else if (msg.Content.Contains("!clip"))
            {
                await msg.Channel.SendMessageAsync("Fetching clip...");

                ClipAccess clip = new ClipAccess(msg.Content);
                var selectedClip = clip.GetClip();

                await msg.Channel.SendMessageAsync($"Clip: {selectedClip.Filename}\nSummary: {selectedClip.Summary}");
            }
        }
    }
}
