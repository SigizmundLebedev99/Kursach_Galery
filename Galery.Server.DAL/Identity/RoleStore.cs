using Dapper;
using static Galery.Server.DAL.Helpers.QueryBuilder;
using Galery.Server.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Galery.Server.DAL.Identity
{
    public class RoleStore : IRoleStore<Role>
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _factory;

        public RoleStore(IConfiguration configuration, DbProviderFactory factory)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                role.Id = await connection.QuerySingleAsync<int>(CreateQuery(role), role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(UpdateQuery(role), role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [{nameof(Role)}] WHERE [Id] = @{nameof(Role.Id)}", role);
            }

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<Role>($@"SELECT * FROM [{nameof(Role)}]
                WHERE [Id] = @{nameof(roleId)}", new { roleId });
            }
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<Role>($@"SELECT * FROM [{nameof(Role)}]
                WHERE [{nameof(Role.NormalizedName)}] = @{nameof(normalizedRoleName)}", new { normalizedRoleName });
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
