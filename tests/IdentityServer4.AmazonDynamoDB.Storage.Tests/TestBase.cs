using System;
using IdentityServer4.AmazonDynamoDB.Storage.Configuration;
using IdentityServer4.AmazonDynamoDB.Storage.Options;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.AmazonDynamoDB.Storage.Tests
{
    public class TestBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        protected IPersistedGrantStore PersistedGrantStore { get; }

        public TestBase()
        {
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("settings.json", optional: false);
            var configurationRoot = configurationBuilder.Build();
            services.AddSingleton((IConfiguration) configurationRoot);
            
            services.Configure<DynamoDBOptions>(configurationRoot.GetSection("DynamoDB"));
            services.AddOperationalDynamoDBStore(configurationRoot,
                options =>
                {
                    var section = configurationRoot.GetSection("DynamoDB");
                    section.Bind(options);
                });

            ServiceProvider = services.BuildServiceProvider();
            PersistedGrantStore = ServiceProvider.GetService<IPersistedGrantStore>();
        }
    }
}