using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using IdentityServer4.AmazonDynamoDB.Storage.Migration;
using IdentityServer4.AmazonDynamoDB.Storage.Options;
using IdentityServer4.AmazonDynamoDB.Storage.Stores;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.AmazonDynamoDB.Storage.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IIdentityServerBuilder AddOperationalDynamoDBStore(
            this IIdentityServerBuilder builder,
            IConfiguration configuration,
            Action<DynamoDBOptions> actionConfigure = null)
        {
            builder.Services.AddOperationalDynamoDBStore(configuration, actionConfigure);

            return builder;
        }
        
        public static IServiceCollection AddOperationalDynamoDBStore(this IServiceCollection services,
            IConfiguration configuration,
            Action<DynamoDBOptions> actionConfigure = null)
        {
            var dynamoDBOptions = new DynamoDBOptions();
            actionConfigure?.Invoke(dynamoDBOptions);

            var awsOptions = configuration.GetAWSOptions();
            awsOptions.DefaultClientConfig.ServiceURL = dynamoDBOptions.ServiceURL;

            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>(awsOptions);
            services.AddSingleton(x => new DynamoDBContextConfig {TableNamePrefix = dynamoDBOptions.TablePrefix});
            services.AddTransient(x =>
            {
                var client = x.GetService<IAmazonDynamoDB>();
                var config = x.GetService<DynamoDBContextConfig>();
                return new DynamoDBContext(client, config);
            });
            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            services.AddTransient<PersistedGrantTableBuilder>();

            return services;
        }
    }
}