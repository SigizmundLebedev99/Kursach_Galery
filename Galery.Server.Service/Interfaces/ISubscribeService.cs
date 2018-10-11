using Galery.Common.DTO.User;
using Galery.Common.Models;
using Galery.Server.Service.Infrostructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galery.Server.Service.Interfaces
{
    public interface ISubscribeService
    {
        Task<UserInfoDTO> GetUserInfo(int userId);
        Task<OperationResult<UserDTO>> Subscribing(Subscribe model);
        Task Desubscribing(int fromId, int toId);
        Task<IEnumerable<UserDTO>> GetSubscribes(int userId);
        Task<IEnumerable<UserDTO>> GetSubscribers(int userId);
    }
}
