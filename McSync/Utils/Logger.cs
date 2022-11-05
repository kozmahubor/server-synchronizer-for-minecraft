﻿using System;
using System.Linq;
using System.Text;
using McSync.Server.Info;

namespace McSync.Utils
{
    public class Logger
    {
        private Logger() { }

        public static Logger Instance { get; } = new Logger();
        private static readonly object ConsoleColorLock = new object();

        internal void Error(Exception exception)
        {
            lock (ConsoleColorLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] " + exception.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal void Error(string message)
        {
            Error(message, Array.Empty<object>());
        }

        internal void Error(string message, object innerContents)
        {
            Error(message, new[] { innerContents });
        }

        private void Error(string message, object[] innerContents)
        {
            lock (ConsoleColorLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                LogPad("[ERROR]", message, innerContents);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal void Local(string message)
        {
            Local(message, Array.Empty<object>());
        }

        internal void Local(string message, object innerContent)
        {
            Local(message, new[] { innerContent });
        }

        private void Local(string message, object[] innerContents)
        {
            lock (ConsoleColorLock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                LogPad("[LOCAL]", message, innerContents);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal void Drive(string message, object innerContent)
        {
            lock (ConsoleColorLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                LogPad("[DRIVE]", message, new[] { innerContent });
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal void DriveWarn(string message, object innerContent)
        {
            lock (ConsoleColorLock)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                LogPad("[DRIVE]", message, new[] { innerContent });
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal void Server(CalculatedStatus calculatedStatus)
        {
            lock (ConsoleColorLock)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                LogPad("[SERVER]", "Server status: {}", new object[] { calculatedStatus.ToString() });
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal void Info(string message)
        {
            Info(message, Array.Empty<object>());
        }

        internal void Info(string message, object innerContent)
        {
            Info(message, new[] { innerContent });
        }

        private void Info(string message, object[] innerContents)
        {
            lock (ConsoleColorLock)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                LogPad("[INFO]", message, innerContents);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private void LogPad(string prefix, string message, object[] innerContents)
        {
            message = $"{prefix,-8} {message}";
            var messageParts = message.Split(new[] { "{}" }, StringSplitOptions.None);
            if (messageParts.Length != innerContents.Length + 1)
            {
                Error("Log error: {}", new object[] { message });
                return;
            }

            var result = new StringBuilder();

            for (int i = 0; i < innerContents.Length; i++)
            {
                result.Append(messageParts[i].PadRight(30))
                    .Append(innerContents[i]);
            }
            result.Append(messageParts.Last());

            Console.WriteLine(result.ToString());
        }
    }
}
