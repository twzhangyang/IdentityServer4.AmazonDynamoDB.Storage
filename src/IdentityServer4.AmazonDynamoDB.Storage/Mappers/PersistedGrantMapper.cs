using Amazon.Util;
using AutoMapper;
using IdentityServer4.AmazonDynamoDB.Storage.Entities;
using IdentityServer4.Models;

namespace IdentityServer4.AmazonDynamoDB.Storage.Mappers
{
    public static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg =>cfg.AddProfile<PersistedGrantMapperProfile>())
                .CreateMapper();
        }

        private static IMapper Mapper { get; }

        public static PersistedGrant ToModel(this PersistedGrantEntity entity)
        {
            return entity == null ? null : Mapper.Map<PersistedGrant>(entity);
        }

        public static PersistedGrantEntity ToEntity(this PersistedGrant model)
        {
            var entity = model == null ? null : Mapper.Map<PersistedGrantEntity>(model);
            if (entity != null)
            {
                entity.TTL = AWSSDKUtils.ConvertToUnixEpochSeconds(model.Expiration.Value);
            }

            return entity;
        }

        public static void UpdateEntity(this PersistedGrant model, PersistedGrantEntity entity)
        {
            Mapper.Map(model, entity);
        }
    }
}