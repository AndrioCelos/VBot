﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using IRC;

namespace CBot {
    internal class ConsoleConnection : IRCClient {
        private static object consoleLock = new object();

        internal ConsoleConnection() {
            this.Address = "!Console";
            this.NetworkName = "!Console";
            this.Port = 0;
            this.Nickname = Bot.dNicknames[0];
        }

        public override void Connect() {
            this.LastSpoke = DateTime.Now;
            this.IsConnected = true;
            this.IsRegistered = true;

            this.ReceivedLine(":" + this.Nickname + "!*@* JOIN #");
            this.ReceivedLine(":User!User@console JOIN #");
        }

        public override void Disconnect() {
        }

        public override void Send(string t) {
            string Prefix; string Command; string[] Parameters; string Trail = null;
            IRCClient.ParseIRCLine(t, out Prefix, out Command, out Parameters, out Trail, true);

            if ((Command.Equals("PRIVMSG", StringComparison.OrdinalIgnoreCase) || Command.Equals("NOTICE", StringComparison.OrdinalIgnoreCase)) && (Parameters[0] == "#" || IRCStringComparer.RFC1459.Equals(Parameters[0], "User"))) {
                // Emulate a channel message to # or PM to 'User' by sticking it on the console.
                ConsoleConnection.writeMessage(Parameters[1]);
            }
        }

        public static void writeMessage(string message) {
            lock (ConsoleConnection.consoleLock) {
                ConsoleColor originalBackground; ConsoleColor originalForeground;
                originalBackground = Console.BackgroundColor;
                originalForeground = Console.ForegroundColor;

                short colour = -1; short backgroundColour = -2; bool bold = false; bool italic = false; bool underline = false; bool strikethrough = false;
                bool colourChanged; bool backgroundColourChanged;
                int i = 0;

                while (true) {
                    colourChanged = false;
                    backgroundColourChanged = false;

                    for (; i < message.Length; ++i) {
                        char c = message[i];
                        if (c == '\u0002') {  // Bold
                            bold = !bold;
                            break;
                        } else if (c == '\u001C') {  // Italic
                            italic = !italic;
                            break;
                        } else if (c == '\u001F') {  // Underline
                            underline = !underline;
                            break;
                        } else if (c == '\u0013') {  // Strikethrough
                            strikethrough = !strikethrough;
                            break;
                        } else if (c == '\u0016') {  // Reverse
                            short num = colour;
                            backgroundColour = colour;
                            colour = num;
                            break;
                        } else if (c == '\u000F') {  // Reset
                            colourChanged = (colour != -1);
                            colour = -1;
                            backgroundColourChanged = (backgroundColour != -2);
                            backgroundColour = -2;
                            bold = false;
                            italic = false;
                            underline = false;
                            strikethrough = false;
                            break;
                        } else if (c == '\u0003') {  // Colour
                            Match match = Regex.Match(message.Substring(i), @"^\x03(\d\d?)(?:,(\d\d?))?");
                            if (match.Success) {
                                colour = short.Parse(match.Groups[1].Value);
                                colourChanged = true;
                                if (match.Groups[2].Success) {
                                    backgroundColour = short.Parse(match.Groups[2].Value);
                                    if (backgroundColour == 99) backgroundColour = -2;
                                    backgroundColourChanged = true;
                                }
                                i += match.Length - 1;
                            } else {
                                colourChanged = (colour != -1);
                                colour = -1;
                                backgroundColourChanged = (backgroundColour != -2);
                                backgroundColour = -2;
                            }
                            break;
                        } else {
                            Console.Write(c);
                        }
                    }
                    if (i >= message.Length) break;
                    if (colourChanged) {
                        switch (colour % 16) {
                            case -2: Console.ForegroundColor = originalBackground; break;
                            case -1: case 99: Console.ForegroundColor = originalForeground; break;
                            case 0: Console.ForegroundColor = ConsoleColor.White; break;
                            case 1: Console.ForegroundColor = ConsoleColor.Black; break;
                            case 2: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                            case 3: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                            case 4: Console.ForegroundColor = ConsoleColor.Red; break;
                            case 5: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                            case 6: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
                            case 7: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                            case 8: Console.ForegroundColor = ConsoleColor.Yellow; break;
                            case 9: Console.ForegroundColor = ConsoleColor.Green; break;
                            case 10: Console.ForegroundColor = ConsoleColor.DarkCyan; break;
                            case 11: Console.ForegroundColor = ConsoleColor.Cyan; break;
                            case 12: Console.ForegroundColor = ConsoleColor.Blue; break;
                            case 13: Console.ForegroundColor = ConsoleColor.Magenta; break;
                            case 14: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                            case 15: Console.ForegroundColor = ConsoleColor.Gray; break;
                        }
                    }
                    if (backgroundColourChanged) {
                        switch (backgroundColour % 16) {
                            case -2: case 99: Console.BackgroundColor = originalBackground; break;
                            case -1: Console.BackgroundColor = originalForeground; break;
                            case 0: Console.BackgroundColor = ConsoleColor.White; break;
                            case 1: Console.BackgroundColor = ConsoleColor.Black; break;
                            case 2: Console.BackgroundColor = ConsoleColor.DarkBlue; break;
                            case 3: Console.BackgroundColor = ConsoleColor.DarkGreen; break;
                            case 4: Console.BackgroundColor = ConsoleColor.Red; break;
                            case 5: Console.BackgroundColor = ConsoleColor.DarkRed; break;
                            case 6: Console.BackgroundColor = ConsoleColor.DarkMagenta; break;
                            case 7: Console.BackgroundColor = ConsoleColor.DarkYellow; break;
                            case 8: Console.BackgroundColor = ConsoleColor.Yellow; break;
                            case 9: Console.BackgroundColor = ConsoleColor.Green; break;
                            case 10: Console.BackgroundColor = ConsoleColor.DarkCyan; break;
                            case 11: Console.BackgroundColor = ConsoleColor.Cyan; break;
                            case 12: Console.BackgroundColor = ConsoleColor.Blue; break;
                            case 13: Console.BackgroundColor = ConsoleColor.Magenta; break;
                            case 14: Console.BackgroundColor = ConsoleColor.DarkGray; break;
                            case 15: Console.BackgroundColor = ConsoleColor.Gray; break;
                        }
                    }
                    if (bold && System.Environment.OSVersion.Platform != PlatformID.Unix) {
                        if (Console.ForegroundColor >= ConsoleColor.DarkBlue && Console.ForegroundColor <= ConsoleColor.DarkYellow)
                            Console.ForegroundColor += 8;
                        else if (Console.ForegroundColor == ConsoleColor.DarkGray)
                            Console.ForegroundColor = ConsoleColor.Gray;
                        else if (Console.ForegroundColor == ConsoleColor.Gray)
                            Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (System.Environment.OSVersion.Platform == PlatformID.Unix) {
                        Console.Write("\u001B[0");
                        switch (colour % 16) {
                            case -2: case -1: case 99: Console.Write(";39"); break;
                            case  0: Console.Write(";97"); break;
                            case  1: Console.Write(";30"); break;
                            case  2: Console.Write(";34"); break;
                            case  3: Console.Write(";32"); break;
                            case  4: Console.Write(";91"); break;
                            case  5: Console.Write(";31"); break;
                            case  6: Console.Write(";35"); break;
                            case  7: Console.Write(";33"); break;
                            case  8: Console.Write(";93"); break;
                            case  9: Console.Write(";92"); break;
                            case 10: Console.Write(";36"); break;
                            case 11: Console.Write(";96"); break;
                            case 12: Console.Write(";94"); break;
                            case 13: Console.Write(";95"); break;
                            case 14: Console.Write(";90"); break;
                            case 15: Console.Write(";37"); break;
                        }
                        switch (backgroundColour % 16) {
                            case -2: case -1: case 99: Console.Write(";49"); break;
                            case  0: Console.Write(";107"); break;
                            case  1: Console.Write( ";40"); break;
                            case  2: Console.Write( ";44"); break;
                            case  3: Console.Write( ";42"); break;
                            case  4: Console.Write(";101"); break;
                            case  5: Console.Write( ";41"); break;
                            case  6: Console.Write( ";45"); break;
                            case  7: Console.Write( ";43"); break;
                            case  8: Console.Write(";103"); break;
                            case  9: Console.Write(";102"); break;
                            case 10: Console.Write( ";46"); break;
                            case 11: Console.Write(";106"); break;
                            case 12: Console.Write(";104"); break;
                            case 13: Console.Write(";105"); break;
                            case 14: Console.Write(";100"); break;
                            case 15: Console.Write( ";47"); break;
                        }
                        if (bold) Console.Write(";1");
                        if (underline) Console.Write(";4");
                        if (italic || strikethrough) Console.Write(";7");
                        Console.Write("m");
                    }
                    ++i;
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        internal void Put(string Text) {
            this.ReceivedLine(":User!User@console PRIVMSG " + this.Nickname + " :" + Text);
        }
    }
}
