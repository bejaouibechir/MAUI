using Grpc.Core;
using MyServiceGrpc;

namespace MyServiceGrpc.Services
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
        public override async Task SayHelloStream(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 5; i++)
            {
                await responseStream.WriteAsync(new HelloReply
                {
                    Message = $"Hello {request.Name} - {i + 1}"
                });
                _logger.Log(LogLevel.Information, $"Hello {request.Name} - {i + 1} recieved");
                await Task.Delay(1000); // Attendre 1 seconde entre les messages
            }
        }

        public override async Task<HelloReply> SayHelloClientStream(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            string nameList = "";
            while (await requestStream.MoveNext())
            {
                var name = requestStream.Current.Name;
                nameList += name + ", ";
            }

            return new HelloReply
            {
                Message = "Hello " + nameList.TrimEnd(',', ' ')
            };
            
        }
        public override async Task SayHelloBidirectionalStream(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var name = requestStream.Current.Name;
                await responseStream.WriteAsync(new HelloReply
                {
                    Message = "Hello " + name
                });
                _logger.Log(LogLevel.Information, $"Hello {name} recieved {DateTime.Now.Hour} : {DateTime.Now.Minute}");
            }
        }
    }
}
