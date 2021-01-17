using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService8.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<HelloReply2> SayHello2(HelloRequest request, ServerCallContext context)
        {
            HelloReply2 reply = new HelloReply2();
            reply.Replies.Add(new HelloReply { Message = "Hello " + request.Name + " 1" });
            reply.Replies.Add(new HelloReply { Message = "Hello " + request.Name + " 2" });
            reply.Replies.Add(new HelloReply { Message = "Hello " + request.Name + " 3" });

            return Task.FromResult(reply);
        }
    }
}
