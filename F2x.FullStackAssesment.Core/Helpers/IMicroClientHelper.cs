using System.Net.Http;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Core.Helpers
{
    public interface IMicroClientHelper
    {
        Task<T> GetDataAsync<T>(string path, string token = "", string schema = "Bearer") where T : class, new();
        Task<string> GetTokenMicroServiceAsync();
    }
}
