using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade
{
    public abstract class Log
    {
        private static Log logImplementation = new ConsoleLog();
        public static void SetImplementation(Log implementation)
        {
            logImplementation = implementation;
        }

        protected abstract void HandleLogInfo(string message);
        protected abstract void HandleLogError(Exception exception);
        protected abstract void HandleLogWarn(string message);

        public static void Info(string message) { logImplementation.HandleLogInfo(message); }
        public static void Error(Exception exception) { logImplementation.HandleLogError(exception); }
        public static void Warn(string message) { logImplementation.HandleLogWarn(message); }

        public static void LogWarning(string message) { Warn(message); }
        public static void LogError(Exception message) { Error(message); }
        public static void LogError(object message) { LogError(message.ToString()); }
        public static void LogError(string message) { Error(new Exception(message)); }
        public static void WriteLine(string message) { Info(message); }
    }
}
