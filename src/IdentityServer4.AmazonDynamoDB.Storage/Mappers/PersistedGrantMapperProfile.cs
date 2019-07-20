﻿using AutoMapper;

namespace IdentityServer4.AmazonDynamoDB.Storage.Mappers
{
    /// <summary>
    /// Defines entity/model mapping for persisted grants.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class PersistedGrantMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="PersistedGrantMapperProfile">
        /// </see>
        /// </summary>
        public PersistedGrantMapperProfile()
        {
            CreateMap<Entities.PersistedGrantEntity, Models.PersistedGrant>(MemberList.Destination)
                .ReverseMap();
        }
    }
}