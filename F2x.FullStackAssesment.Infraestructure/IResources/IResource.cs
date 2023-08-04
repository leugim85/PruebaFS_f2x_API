using Polly.Retry;
using System;

namespace F2xFullStackAssesment.Infraestructure.IResources
{
    public interface IResource
    {
        int GetCurentDateInFormatyyyyMMdd(DateTime date);
        AsyncRetryPolicy GetRetryPolicy(string method);
    }
}
