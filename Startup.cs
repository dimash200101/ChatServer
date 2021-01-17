using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService8
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<ChatRoom>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<ChattingService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync();
                });
            });
        }
    }
}
