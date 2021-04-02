using API0Protos;
using Client.DTO;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client
{
    public class Manager
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private API0_Proto.API0_ProtoClient Client = null;

        private BasicConfigs cfgs = new BasicConfigs();

        public void LoadConfigs()
        {
            try
            {
                // deserialize JSON directly from a file
                //using (StreamReader file = File.OpenText(@"AppSettings.json"))
                //{
                //    JsonSerializer serializer = new JsonSerializer();
                //    cfgs = (BasicConfigs)serializer.Deserialize(file, typeof(BasicConfigs));
                //}

                cfgs = JsonConvert.DeserializeObject<BasicConfigs>(File.ReadAllText(@"AppSettings.json"));

            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }

        public bool ConnectToServer()
        {
            try
            {

                var cacert = File.ReadAllText(@"SSL/ca.crt");
                var clientcert = File.ReadAllText(@"SSL/client.crt");
                var clientkey = File.ReadAllText(@"SSL/client.key");
                var ssl = new SslCredentials(cacert, new KeyCertificatePair(clientcert, clientkey));

                /*var PcName = Environment.MachineName;
                var channelOptions = new List<ChannelOption>
                {
                    new ChannelOption(ChannelOptions.SslTargetNameOverride, PcName)
                };*/
                //var channel = new Channel("127.0.0.1", 555, ssl, channelOptions);

                //its better to be setted in order to ensure arrival at destination
                List<ChannelOption> channelOptions = new List<ChannelOption>()
                {
                    new ChannelOption("grpc.ssl_target_name_override", "127.0.0.1"),
                };

                var channel = new Channel(cfgs.DestinationIp, cfgs.DestinationPort, ssl, channelOptions);

                Client = new API0_Proto.API0_ProtoClient(channel);

                log.Debug($"Opened channel to: {cfgs.DestinationIp}:{cfgs.DestinationPort}");

                return true;

            }
            catch(Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public void Send()
        {
            try
            {

                log.Debug("Send Invoked!");

                if (Client == null)
                {
                    log.Error("No client");
                    return;
                }

                var result = Client.Method0(new Args() 
                { 
                    Arg0 = "Hello",
                    Arg1 = "World"
                });

                log.Info("Send Went Ok!");

            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }

    }
}
