using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.Extensions.Configuration;
using Thor.Hosting.AspNetCore;

namespace Demo
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();

            services.AddTracing(_configuration); //Thor tracing services

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddSubscriptionType<Subscription>()
                .AddThorLogging(); // adds exception capture an forwarding to the EventBus 

            //Add Hosted Service that listens to Events
            services.AddSingleton<IHostedService, EventHubReceiverService>();
            // Add in-memory event provider
            services.AddInMemorySubscriptions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseWebSockets()
                .UseEndpoints(endpoints => endpoints.MapGraphQL(path: "/")
                .WithOptions(new GraphQLServerOptions() { EnableSchemaRequests = true }));
        }
    }
}
