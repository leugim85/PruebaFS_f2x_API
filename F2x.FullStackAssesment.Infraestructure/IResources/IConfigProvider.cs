using System.Collections.Generic;

namespace F2xFullStackAssesment.Infraestructure.IResources
{
    public interface IConfigProvider
    {
        #region RetryPolicy
        int GetRetryCount();
        double GetRetrySeconds();
        #endregion

        #region ApiServices
        string Header { get; }
        string HeaderValue { get; }
        string TokenPath { get; }
        string UserName { get; }
        string Password { get; }

        string VehiclesCounterPath { get; }
        string VehiclesCounterValuePath { get; }
        #endregion


        List<int> ExecutionDays { get; }
        string ExecutionHour { get; }
    }
}
