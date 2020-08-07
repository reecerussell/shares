using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Shares.Core;
using Shares.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shares.Users.Providers
{
    internal class UserProvider : IUserProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMemoryCache _cache;
        private readonly ILogger<UserProvider> _logger;

        public UserProvider(
            IConnectionStringProvider connectionStringProvider,
            IMemoryCache cache,
            ILogger<UserProvider> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Maybe<UserDto>> GetAsync(string id)
        {
            _logger.LogDebug("Getting user from database with id '{0}'.", id);
            const string query = "CALL `get_user`(?id);";
            _logger.LogDebug("Query: {0}", query);
            await using var connection = await GetConnectionAsync();
            var user = await connection.QuerySingleOrDefaultAsync<UserDto>(query, new {id});
            _logger.LogDebug("User found: {0}", user != null);
            return user;
        }

        public async Task<IReadOnlyList<UserDto>> GetAsync()
        {
            _logger.LogDebug("Getting all users from the database.");
            const string query = "CALL `get_users`();";
            _logger.LogDebug("Query: {0}", query);
            await using var connection = await GetConnectionAsync();
            var results = (await connection.QueryAsync<UserDto>(query)).ToList();
            _logger.LogDebug("{0} Users found.", results.Count);
            return results;
        }

        private async Task<MySqlConnection> GetConnectionAsync()
        {
            if (!_cache.TryGetValue(Constants.ConnectionStringCacheKey, out string connectionString))
            {
                _logger.LogDebug("Getting connection string...");
                connectionString = await _connectionStringProvider.Get();
                var options = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(Constants.ConnectionStringCacheKey, connectionString, options);
                _logger.LogDebug("Cached connection string.");
                _logger.LogDebug("Connection string: {0}", connectionString);
            }
            else
            {
                _logger.LogDebug("Using cached connection string: {0}", connectionString);
            }

            return new MySqlConnection(connectionString);
        }
    }
}
