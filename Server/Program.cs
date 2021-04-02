using System;

namespace Server
{
    class Program
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            log.Debug("Starting Server");

            var mgr = new Manager();
            mgr.StartOwnServer();

            log.Debug("Press any key to Exit");
            Console.ReadLine();
        }
    }
}
