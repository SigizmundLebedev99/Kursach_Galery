using AutoMapper;
using Dapper;
using Galery.Common.DTO.User;
using Galery.Common.Models;
using Galery.Server.DAL.Helpers;
using Galery.Server.DAL.Models;
using Galery.Server.Service.Exceptions;
using Galery.Server.Service.Infrostructure;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Services
{
    public class SubscribeService : ISubscribeService
    {
        readonly string _connectionString;
        readonly DbProviderFactory _factory;
        readonly UserManager<User> _userManager;
        readonly IMapper _mapper;

        public SubscribeService(
            DbProviderFactory factory,
            IConfiguration configuration,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _userManager = userManager;
            _mapper = mapper;
        }

        private Task Connect(DbConnection connection)
        {
            connection.ConnectionString = _connectionString;
            return connection.OpenAsync();
        }

        public async Task Desubscribing(int fromId, int toId)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);

                    var sub = await connection.QueryFirstAsync<Subscribe>($"select * from [{nameof(Subscribe)}] where [{nameof(Subscribe.FromUserId)}] = @{nameof(fromId)} and" +
                        $"[{nameof(Subscribe.ToUserId)}] = @{nameof(toId)}", new { fromId, toId });
                    if (sub == null)
                        throw new NotFoundException("Не удалось найти запись о подписке");
                    await connection.ExecuteAsync($"delete from [{nameof(Subscribe)}] where [{nameof(Subscribe.FromUserId)}] = @{nameof(fromId)} and" +
                        $"[{nameof(Subscribe.ToUserId)}] = @{nameof(toId)}", new { fromId, toId});
                }
            }
            catch (NotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось удалить данные", ex.Message);
            }
        }

        public async Task<IEnumerable<UserDTO>> GetSubscribes(int userId)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await connection.QueryAsync<UserDTO>("GetSubscribesForUser", new { userId }, null, null, System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<UserInfoDTO> GetUserInfo(int userId)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    var res = await connection.QueryFirstOrDefaultAsync<UserInfoDTO>("GetUserInfo", new { userId }, null, null, System.Data.CommandType.StoredProcedure);
                    res = res ?? throw new NotFoundException($"Не удалось найти пользователя с id = {userId}");
                    return res;
                }
            }
            catch (NotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<OperationResult<UserDTO>> Subscribing(Subscribe model)
        {
            try
            {
                var operRes = new OperationResult<UserDTO>(true);
                var toUser = await _userManager.FindByIdAsync(model.ToUserId.ToString());
                if (toUser == null)
                    operRes.AddErrorMessage("FromUserId", $"Не удалось найти пользователя с id = {model.ToUserId}");
                var fromUser = await _userManager.FindByIdAsync(model.FromUserId.ToString());
                if (fromUser == null)
                    operRes.AddErrorMessage("FromUserId", $"Не удалось найти пользователя с id = {model.FromUserId}");
                if (model.FromUserId == model.ToUserId)
                    operRes.AddErrorMessage(null, "id пользователей не должны совпадать");

                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);

                    var sub = await connection.QueryFirstOrDefaultAsync<Subscribe>($"select * from [{nameof(Subscribe)}] where [{nameof(Subscribe.FromUserId)}] = @{nameof(model.FromUserId)} and" +
                        $"[{nameof(Subscribe.ToUserId)}] = @{nameof(model.ToUserId)}", model);
                    if (sub != null)
                        operRes.AddErrorMessage(null, $"пользователь {model.FromUserId} уже подписан на {model.ToUserId}");

                    if (!operRes.Succeeded)
                        return operRes;
                   
                    await connection.ExecuteAsync(QueryBuilder.CreateQuery(model), model);
                    var res = new UserDTO { Avatar = toUser.Avatar, Id = toUser.Id, UserName = toUser.UserName };
                    operRes.Results.Add(res);
                    return operRes;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task<IEnumerable<UserDTO>> GetSubscribers(int userId)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await connection.QueryAsync<UserDTO>("GetSubscribersForUser", new { userId }, null, null, System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task<IEnumerable<UserDTO>> UserSearch(string name)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await connection.QueryAsync<UserDTO>("UserSearch", new { search = name }, null, null, System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }
    }
}
