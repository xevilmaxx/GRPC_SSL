using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RequestsLoggerInterceptor : Interceptor
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private const string MessageTemplate =
          "{RequestMethod} responded {StatusCode} in {Elapsed:0.0000} ms";

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var sw = Stopwatch.StartNew();

            var response = await base.UnaryServerHandler(request, context, continuation);

            sw.Stop();
            log.Trace(MessageTemplate,
                  context.Method,
                  context.Status.StatusCode,
                  sw.Elapsed.TotalMilliseconds);

            return response;
        }
    }
}
