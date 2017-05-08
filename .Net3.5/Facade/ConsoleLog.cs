using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade
{
    public class ConsoleLog : Log
    {
        protected override void HandleLogError(Exception exception)
        {
            Console.WriteLine("ERROR");
            Console.WriteLine(exception);
        }

        protected override void HandleLogInfo(string message)
        {
            Console.WriteLine(message);
        }

        protected override void HandleLogWarn(string message)
        {
            Console.WriteLine("WARN:" + message);
        }
    }
}
