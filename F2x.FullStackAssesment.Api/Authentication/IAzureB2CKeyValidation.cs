using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Api.Authentication
{
    public interface IAzureB2CKeyValidation
    {
        Task<IEnumerable<SecurityKey>> GetKeysAsync();
        void InvalidateKeys();
    }
}
