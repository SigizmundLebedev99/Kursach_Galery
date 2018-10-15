using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface ILoadClient
    {
        Task<HttpResponseMessage> LoadImage(byte[] file, string name);
        Task<HttpResponseMessage> LoadAvatar(byte[] file, string name);
    }
}
