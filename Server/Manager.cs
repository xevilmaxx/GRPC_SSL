using API0Protos;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Manager
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public bool StartOwnServer() 
        {
            try
            {

                var cacert = File.ReadAllText(@"SSL/ca.crt");
                var servercert = File.ReadAllText(@"SSL/server.crt");
                var serverkey = File.ReadAllText(@"SSL/server.key");
                var keypair = new KeyCertificatePair(servercert, serverkey);
                var sslCredentials = new SslServerCredentials(new List<KeyCertificatePair>() { keypair }, cacert, false);
                
                var server = new Grpc.Core.Server
                {
                    Services = { API0_Proto.BindService(new Service()).Intercept(new RequestsLoggerInterceptor()) },
                    Ports = { new ServerPort("[::]", 555, sslCredentials) }
                };
                server.Start();

                log.Debug("Server started on -> '127.0.0.1:555'");

                return true;

            }
            catch(Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

    }
}
