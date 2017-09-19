using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace DiscordBot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;
        Random rand;
        
        string[] roasts;

        public MyBot()
        {
            rand = new Random();
            roasts = new string[]
            {
                "Jr can't squat, but he wishes he could",
                "Ty can't carry in paragon, but he wishes he could",
                "Colby is god"
            };

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();

            SendRoastCommand();
            FuckCommand();

            discord.UserJoined += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();
                var user = e.User.Name;
                await channel.SendTTSMessage(string.Format("@" + user + " has begun bamboozling!"));

            };

            discord.UserJoined += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();
                var user = e.User.Name;
                await channel.SendTTSMessage(string.Format(user + " has begun bamboozling!"));

            };

            // Register a Hook into the UserUpdated event using a Lambda.
            discord.UserUpdated += async (s, e) => {

                // Check that the user is in a Voice channel
                if (e.After.VoiceChannel == null) return;
                
                var logChannel = e.Server.FindChannels("general").FirstOrDefault();

                // See if they changed Voice channels
                if (e.Before.VoiceChannel == e.After.VoiceChannel) return;

               await logChannel.SendTTSMessage($"User {e.After.Name} has begun bamboozling!");
            };



            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MzQ2MDgyNTU2MDk5Mjk3Mjky.DHEt3Q.XHOz8IW7GBya2v_m2tHSUVSRD50",TokenType.Bot);
            });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void SendRoastCommand()
        {
            commands.CreateCommand("roast")
                .Do(async (e) =>
                {
                    int randomRoastIndex = rand.Next(roasts.Length);
                    string roastToChat = roasts[randomRoastIndex];
                    await e.Channel.SendTTSMessage(roastToChat);
                });
        }

        private void FuckCommand()
        {
            commands.CreateCommand("Hello")
                .Do(async (e) =>
                {
                    
                    await e.Channel.SendTTSMessage($"Ello {e.User}!");
                });
        }
    }
}
