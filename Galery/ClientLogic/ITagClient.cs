using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface ITagClient
    {
        Task<HttpResponseMessage> CreateTag(string name);

        Task<HttpResponseMessage> GetAllTags();

        Task<HttpResponseMessage> UpdateTag(int id, string name);

        Task<HttpResponseMessage> DeleteTag(int id);
    }
}
