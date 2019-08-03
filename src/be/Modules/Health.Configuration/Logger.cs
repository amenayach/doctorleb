//------------------------------------------------------------------
// <copyright file="Logger.cs" company="atDoctor">
//     Copyright (c) atDoctor.  All rights reserved.
// </copyright>
//
// <summary>
// Used to log messages.
// </summary>
//
// <remarks/>
//------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace Health.Configuration
{
    /// <summary>
    /// Used to log messages.
    /// </summary>
    public static class Logger
    {
        public static void Log(Exception exception, string note = "")
        {
            try
            {
                var logPath = GetLogFilePath();

                var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Note: {note}
Message: {exception?.Message}
StackTrace: {exception?.StackTrace}";

                File.AppendAllText(logPath, logText);
            }
            catch
            {
                // Ignored
            }
        }

        public static void Log(string logText, string type = "INFO")
        {
            try
            {
                var logPath = GetLogFilePath();

                var logTextInfo = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: INFO
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Message: {logText}";

                File.AppendAllText(logPath, logTextInfo);
            }
            catch
            {
                // Ignored
            }
        }

        public static void Log(ExceptionContext context)
        {
            try
            {
                var logPath = GetLogFilePath();

                var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Url: {context?.HttpContext?.Request?.Method} {context?.HttpContext?.Request?.GetUri()}
Payload: {GetBody(context)}
Message: {context?.Exception?.Message}
StackTrace: {context?.Exception?.StackTrace}
InnerMessage: {context?.Exception?.InnerException?.Message}
InnerStackTrace: {context?.Exception?.InnerException?.StackTrace}";

                File.AppendAllText(logPath, logText);
            }
            catch
            {
                // Ignored
            }
        }

        private static string GetLogFilePath()
        {
            try
            {
                var folder = Directory.GetCurrentDirectory() + "\\logs";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                return Path.Combine(folder, $"Log_{DateTime.Now:yyyyMMdd}.txt");
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        private static string GetBody(ExceptionContext context)
        {
            var result = "";
            try
            {
                var req = context.HttpContext.Request;

                // Allows using several time the stream in ASP.Net Core
                req.EnableRewind();

                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                if (req.Body != null)
                {
                    using (StreamReader reader
                        = new StreamReader(req.Body, System.Text.Encoding.UTF8, true, 1024, true))
                    {
                        result = reader.ReadToEnd();
                    }

                    // Rewind, so the core is not lost when it looks the body for the request
                    req.Body.Position = 0;
                }

                // Do your work with bodyStr
            }
            catch
            {
                // Ignored
            }
            return result;
        }
    }
}
