using System;
using System.Threading.Tasks;
using System.IO.Ports;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DiscordTest.Modules;
using System.Reflection;
using Microsoft.Extensions.Logging;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;

namespace DiscordTest
{
    class DiscordTester
    {
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = "",
                TokenType = DSharpPlus.TokenType.Bot,
                    LogTimestampFormat = "MMM dd yyyy - hh:mm:ss tt",
                        MinimumLogLevel = LogLevel.Debug
            });
            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });
            discord.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromSeconds(30)
            });

            commands.RegisterCommands<CommandeMaker>();
            // commands.RegisterCommands(Assembly.GetExecutingAssembly());
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
    
    static class SendToArd
    {
        static bool isConnected = false;
        private static String[] ports;
        public static SerialPort port;
        public static string Port => ports[1];
        public static bool getisConnected() => isConnected;

        public static void ConnectToArduino(string argus)
        {

            // check each avalible port 
            getAvailableComPorts();
            string selectedPort = null;
            foreach (var portOptions in ports)
            {

                Console.WriteLine(portOptions);
            }
            if (selectedPort == null)
            {
                if (ports.Length > 1)
                {
                    selectedPort = ports[1];
                }
            } 
           port = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
            if (!port.IsOpen)
            {
                port.Open();
                port.Write("#TEXT" + "OPEN" + "#\n");
                isConnected = true;
            }
        }
        static void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }
        public static int disconnectFromArduino()
        {
            isConnected = false;
            if (!isConnected) port.Write("#TEXT" + "#STOP" + "#\n");
            port.Close();
            return -1;
        }
        public static Task Write(string word)
        {
            _ = Task.Run(() =>
            {
                // Write String to Ard
                 port.Write("#TEXT" + word.ToUpper() + "#\n");
            });
            return Task.CompletedTask;
        }
    }
    }