using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerService8
{

public class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    public static void Main(string[] args)
    {

        CreateHostBuilder(args).Build().Run();
        var userName = Console.ReadLine();
        var port = args.Length > 0 ? args[0] : "50051";

        var channel = new Channel("localhost:" + port, ChannelCredentials.Insecure);
        var client = new ChatRoom.ChatRoomClient(channel);

        using (var chat = client.join())
        {
            _ = Task.Run(async () =>
            {
                while (await chat.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                {
                    var response = chat.ResponseStream.Current;
                    Console.WriteLine($"{response.User}: {response.Text}");
                }
            });

            await chat.RequestStream.WriteAsync(new Message { User = userName, Text = $"{userName} has joined the room" });

            string line;
            while ((line = Console.ReadLine()) != null)
            {
                if (line.ToLower() == "bye")
                {
                    break;
                }
                await chat.RequestStream.WriteAsync(new Message { User = userName, Text = line });
            }
            await chat.RequestStream.CompleteAsync();
        }

    }
}
}
}
