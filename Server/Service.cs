using API0Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Service : API0Protos.API0_Proto.API0_ProtoBase
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public override Task<Echo> Method0(Args request, ServerCallContext context)
        {
            log.Debug("Method0 Invoked!");

            return Task.FromResult(new Echo() { Answer = "Echo" });
        }
    }
}
