using System;

namespace F2xFullStackAssesment.Infraestructure.Resources
{
    public static class Extensions
    {
        /// <summary>
        /// Retorna la fecha y la hora actual de Colombia
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetColombiaDateNow(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(GetTimeZoneSO()));
        }    

        #region PrivateMethods
        private static string GetTimeZoneSO()
        {
            return Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? "SA Pacific Standard Time" : "America/Bogota";
        }

        #endregion

    }
}
