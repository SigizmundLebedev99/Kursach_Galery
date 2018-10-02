using Dapper;
using Galery.Server.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static Galery.Server.DAL.Helpers.QueryBuilder;

namespace Galery.Server.DAL.Identity
{
    public class UserStore : IUserStore<User>, IUserEmailStore<User>, IUserPasswordStore<User>, IUserRoleStore<User>
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _factory;

        public UserStore(IConfiguration connStr, DbProviderFactory factory)
        {
            _connectionString = connStr.GetConnectionString("DefaultConnection");
            _factory = factory;
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                var normalizedName = roleName.ToUpper();
                var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT [{nameof(Role.Id)}] FROM [{nameof(Role)}] WHERE [{nameof(Role.NormalizedName)}] = @{nameof(normalizedName)}", new { normalizedName });
                if (!roleId.HasValue)
                {
                    var role = new Role { Name = roleName, NormalizedName = normalizedName };
                    roleId = await connection.ExecuteAsync(CreateQuery(role), role);
                }
                var userRole = new UserRole { RoleId = roleId.Value, UserId = user.Id };
                await connection.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM [{nameof(UserRole)}] " +
                    $"WHERE [{nameof(UserRole.UserId)}] = @{nameof(UserRole.UserId)} AND [{nameof(UserRole.RoleId)}] = @{nameof(UserRole.RoleId)}) " +
                    CreateQuery(userRole), userRole);
            }
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                user.Id = await connection.QuerySingleAsync<int>(CreateQuery(user), user);
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [{nameof(User)}] WHERE [Id] = @{nameof(User.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<User>($@"SELECT * FROM [ApplicationUser]
                WHERE [NormalizedEmail] = @{nameof(normalizedEmail)}", new { normalizedEmail });
            }
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                int id = Convert.ToInt32(userId);
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<User>($@"SELECT * FROM [{nameof(User)}]
                WHERE [Id] = @{nameof(id)}", new { id });
            }
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<User>($@"SELECT * FROM [{nameof(User)}]
                WHERE [{nameof(User.NormalizedUserName)}] = @{nameof(normalizedUserName)}", new { normalizedUserName });
            }
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                int userId = user.Id;
                var queryResults = await connection.QueryAsync<Role>(
                    m2mJoinQuery<Role, UserRole>(e=>e.Id, e=>e.RoleId, e=>e.UserId, nameof(userId)), new { userId });
                return queryResults.Select(e=>e.Name).ToList();
            }
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                var queryResults = await connection.QueryAsync<User>($"SELECT u.* FROM [{nameof(User)}] u " +
                    $"INNER JOIN [{nameof(UserRole)}] ur ON ur.[{nameof(UserRole.UserId)}] = u.[{nameof(User.Id)}] " +
                    $"INNER JOIN [{nameof(Role)}] r ON r.[{nameof(Role.Id)}] = ur.[{nameof(UserRole.RoleId)}] " +
                    $"WHERE r.[{nameof(Role.NormalizedName)}] = @normalizedName",
                    new { normalizedName = roleName.ToUpper() });

                return queryResults.ToList();
            }
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT [{nameof(Role.Id)}] " +
                    $"FROM [{nameof(Role)}] " +
                    $"WHERE [{nameof(Role.NormalizedName)}] = @normalizedName", 
                    new { normalizedName = roleName.ToUpper() });
                if (roleId == default(int)) return false;
                var matchingRoles = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) " +
                    $"FROM [{nameof(UserRole)}] " +
                    $"WHERE [{nameof(UserRole.UserId)}] = @userId " +
                    $"AND [{nameof(UserRole.RoleId)}] = @{nameof(roleId)}",
                    new { userId = user.Id, roleId });

                return matchingRoles > 0;
            }
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT [Id] " +
                    $"FROM [{nameof(Role)}] " +
                    $"WHERE [{nameof(Role.NormalizedName)}] = @normalizedName", 
                    new { normalizedName = roleName.ToUpper() });
                if (!roleId.HasValue)
                    await connection.ExecuteAsync($"DELETE FROM [{nameof(UserRole)}] " +
                        $"WHERE [{nameof(UserRole.UserId)}] = @userId AND [{nameof(UserRole.RoleId)}] = @{nameof(roleId)}", new { userId = user.Id, roleId });
            }
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(UpdateQuery(user), user);
            }

            return IdentityResult.Success;
        }
    }
}
