﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Flux
{
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8,
        All = 15
    }

    public static class Log
    {
        private static object Lck = "";
        private static string _filename;
        private static LogLevel _logLevel;
        private static StreamWriter _logWriter;

        /// <summary>
        /// Creates the log file stream and sets the initial log level.
        /// </summary>
        /// <param name="filename">The output filename. This file will be overwritten if 'clear' is set.</param>
        /// <param name="logLevel">The <see cref="LogLevel" /> value which sets the type of messages to output.</param>
        /// <param name="clear">Whether or not to clear the log file on initialization.</param>
        public static void Initialize(string filename, LogLevel logLevel, bool clear)
        {
            _filename = filename;
            _logLevel = logLevel;

            _logWriter = new StreamWriter(filename, !clear);
        }

        /// <summary>
        /// Internal method which writes a message directly to the log file.
        /// </summary>
        private static void Write(String message, LogLevel level)
        {
            lock (Lck) {
            StackTrace trace = new StackTrace();
            StackFrame frame = null;

            frame = trace.GetFrame(2);

            string caller = "";

            if (frame != null && frame.GetMethod().DeclaringType != null)
            {
                caller = frame.GetMethod().DeclaringType.Name + ": ";
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            _logWriter.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            switch (level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(" DBUG ");
                    _logWriter.Write(" DBUG ");
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" INFO ");
                    _logWriter.Write(" INFO ");
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" WARN ");
                    _logWriter.Write(" WARN ");
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" EROR ");
                    _logWriter.Write(" EROR ");
                    break;
            }

            /*try
            {
                _logWriter = new StreamWriter(_filename, true);
            }
            catch (IOException)
            {
                _logWriter = new StreamWriter(_filename + "." + Process.GetCurrentProcess().Id.ToString(), true);
            }*/
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(caller + " ");
            _logWriter.Write(caller + " ");
            Console.Write(message + "\n");
            _logWriter.Write(message + "\n");
            _logWriter.Flush();
            }
        }

        /// <summary>
        /// Checks whether the log level contains the specified flag.
        /// </summary>
        /// <param name="type">The <see cref="LogLevel" /> value to check.</param>
        private static bool MayWriteType(LogLevel type)
        {
            return ((_logLevel & type) == type);
        }

        /// <summary>
        /// Writes an error to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Error(String message)
        {
            if (!MayWriteType(LogLevel.Error))
            {
                return;
            }

            Write(message, LogLevel.Error);
        }

        /// <summary>
        /// Writes a warning to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Warn(String message)
        {
            if (!MayWriteType(LogLevel.Warning))
            {
                return;
            }

            Write(message, LogLevel.Warning);
        }

        /// <summary>
        /// Writes an informative string to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Info(String message)
        {
            if (!MayWriteType(LogLevel.Info))
            {
                return;
            }

            Write(message, LogLevel.Info);
        }

        /// <summary>
        /// Writes a debug string to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Debug(String message)
        {
            if (!MayWriteType(LogLevel.Debug))
            {
                return;
            }

            Write(message, LogLevel.Debug);
        }
    }
}
