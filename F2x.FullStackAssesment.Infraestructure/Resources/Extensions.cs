using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        /// <summary>
        /// Calcula la fecha y hora actual de Colombia recibiendo una fecha UTC
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertToColombiaDateTimeFromUtc(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), TimeZoneInfo.FindSystemTimeZoneById(GetTimeZoneSO()));
        }

        /// <summary>
        /// Inicializa un arreglo de bytes con un valor por defecto
        /// </summary>
        /// <param name="array"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static byte[] InitializeByteArray(this byte[] array, byte defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }

            return array;
        }

        /// <summary>
        /// Remueve los caracteres especiales de un string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in str)
            {
                bool numbers = c >= '0' && c <= '9';
                bool capitalLetters = c >= 'A' && c <= 'Z';
                bool lowerLetters = c >= 'a' && c <= 'z';
                if (numbers || capitalLetters || lowerLetters)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string ReplaceTemplate(this string template, IList<(string field, string replace)> fieldReplace)
        {
            var templateFormatted = template;
            if (!fieldReplace?.Any() ?? true)
            {
                return template;
            }

            fieldReplace.ToList().ForEach(x => templateFormatted = templateFormatted.Replace(x.field, x.replace));
            return templateFormatted;
        }

        #region PrivateMethods
        private static string GetTimeZoneSO()
        {
            return Environment.OSVersion.Platform.Equals(PlatformID.Win32NT) ? "SA Pacific Standard Time" : "America/Bogota";
        }

        #endregion

    }
}
