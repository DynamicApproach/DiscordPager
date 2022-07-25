using System;
using System.Threading.Tasks;
using Discord.Commands;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using CommandContext = DSharpPlus.CommandsNext.CommandContext;
using DSharpPlus.CommandsNext.Attributes;
using CommandAttribute = DSharpPlus.CommandsNext.Attributes.CommandAttribute;
using DSharpPlus.Interactivity.Extensions;
using System.Linq;
using System.Runtime.InteropServices;
using Emzi0767.Utilities;
using DSharpPlus.Entities;

namespace DiscordTest.Modules
{
    
    public class CommandeMaker : BaseCommandModule
    {
        public DiscordMessage m { get; private set; }

        [Command("test")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.RespondAsync("Reply Sent");
        }
        [Command("connect")]
        public async Task Connect(CommandContext ctx)
        {
            await ctx.RespondAsync("Connecting..");
            SendToArd.ConnectToArduino("pls");
            await ctx.RespondAsync("Connected to port");
        }
        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");
        }
        [Command("disconnect")]
        public async Task Disconnect(CommandContext ctx)
        {
            await ctx.RespondAsync("Connected, disconnecting from Ard..");
            if (SendToArd.disconnectFromArduino() == -1)
            {
                await ctx.RespondAsync("Disconnected :)");
            }
            else
                await ctx.RespondAsync("failed");
        }
        [Command("write")]
        public async Task WriteTo(CommandContext ctx)
        {
            //ENTER METHOD TO READ FROM DISCORD AND SEND TO PROGRAM WRITE FUCT
            await SendToArd.Write(ctx.RawArgumentString);
        }

        [Command("message")]
        public async Task Interact(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("What would you like to msg Tom? lol?");
            var result = await ctx.Message.GetNextMessageAsync( m =>
            {
                return m.Content.ToLower() != "confirm";
            });
            if (result.TimedOut) await ctx.RespondAsync("Action confirmed.");
            var n = result;
            var b = n.Result;
            await SendToArd.Write( b.Content);
            await ctx.RespondAsync("Setting Ard to say " + b.Content);
        }
    }


}
