using System;

namespace Client
{
    class Program
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {

            log.Debug("Press any key to begin connection");
            Console.ReadLine();

            var mgr = new Manager();
            mgr.LoadConfigs();
            mgr.ConnectToServer();
            mgr.Send();

            log.Debug("Press any key to exit");
            Console.ReadLine();

        }
    }
}
