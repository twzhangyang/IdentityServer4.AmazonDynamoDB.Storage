using System;
using System.Reflection;
using IdentityServer4.AmazonDynamoDB.Storage.Configuration;
using IdentityServer4.AmazonDynamoDB.Storage.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Samples.ResourceOwnerClient.Server
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            var builder = services.AddIdentityServer()
//                .AddOperationalDynamoDBStore(Configuration,"DynamoDB")
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to support static files
            //app.UseStaticFiles();

//            app.UseIdentityServer();

            // uncomment, if you wan to add an MVC-based UI
            //app.UseMvcWithDefaultRoute();
        }
    }
}