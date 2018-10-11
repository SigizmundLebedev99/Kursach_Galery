using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface ILoadClient
    {
        Task<HttpResponseMessage> LoadImage(byte[] file);
        Task<HttpResponseMessage> LoadAvatar(byte[] file);
    }
}
