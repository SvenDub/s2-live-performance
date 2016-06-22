using System;

namespace Util
{
    /// <summary>
    ///     Utility class for logging to console.
    /// </summary>
    public static class Log
    {
        /// <summary>
        ///     Log on INFO level.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="msg">Message.</param>
        public static void I(string tag, string msg) => PrintLn(tag, "INFO ", msg, ConsoleColor.DarkGreen);

        /// <summary>
        ///     Log on DEBUG level.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="msg">Message.</param>
        public static void D(string tag, string msg) => PrintLn(tag, "DEBUG", msg, ConsoleColor.Blue);

        /// <summary>
        ///     Log on WARN level.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="msg">Message.</param>
        public static void W(string tag, string msg) => PrintLn(tag, "WARN ", msg, ConsoleColor.Yellow);

        /// <summary>
        ///     Log on ERROR level.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="msg">Message.</param>
        public static void E(string tag, string msg) => PrintLn(tag, "ERROR", msg, ConsoleColor.Red);

        /// <summary>
        ///     Write a colored message to console with a timestamp.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="level">Level.</param>
        /// <param name="msg">Message.</param>
        /// <param name="color">Color.</param>
        public static void PrintLn(string tag, string level, string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"{DateTime.Now.ToString("HH:mm:ss:fff")} [{tag.PadRight(10).Substring(0, 10)}] [{level}] ");
            Console.ResetColor();
            Console.WriteLine(msg);
        }
    }
}