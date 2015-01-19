using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISTRNGBeaconThingy
{
    public class Utilities
    {
        public static byte[] HexStringToBytes(string bytes)
        {
            if (string.IsNullOrWhiteSpace(bytes))
                return null;
            //if (!bytes.IsValidHexByteString())
            //    throw new ArgumentOutOfRangeException("Not a valid hex byte string.");
            byte[] results = new byte[bytes.Length / 2];
            for (int i = 0; i < bytes.Length; i += 2)
            {
                results[i / 2] = byte.Parse(bytes.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return results;
        }

        public static string BytesToHexString(byte[] data)
        {
            if (data == null)
                return null;
            if (data.Length == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("X2").ToLowerInvariant());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get the specific interval count in time to retrieve.  ROUNDED DOWN TO THE MINUTE.
        /// </summary>
        /// <param name="TheDateTime">A DateTime object with the specific date after 1970-01-01 at midnight.</param>
        /// <returns>A 64 bit integer representing the specific MINUTE in time.</returns>
        public static Int64 GetNISTTimeCode(DateTime TheDateTime)
        {
            if (DateTime.Compare(DateTime.Parse("1970/01/01 12:00:00AM"), TheDateTime) > 0)
                throw new InvalidOperationException("The DateTime object must represent a date after Jan 1, 1970 at midnight (UTC)!");
            DateTime minuteRounded = 
                new DateTime(TheDateTime.Year, TheDateTime.Month, TheDateTime.Day, 
                    TheDateTime.Hour, TheDateTime.Minute, 0,  // forget about the minute, set to 0
                    TheDateTime.Kind); // preserve the kind
            Int64 epoch = (Int64)((minuteRounded.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            return epoch;
        }

        public static DateTime GetDateFromNISTTimeCode(string TimeStamp)//, int Interval)
        {
            Int64 DateCode = Int64.Parse(TimeStamp);
            //Int64 tempTicks = (Int64)((DateCode * (ulong)Interval) * 10000000);
            Int64 tempTicks = (Int64)(DateCode * 10000000);
            return DateTime.MinValue.AddTicks(tempTicks + 621355968000000000).ToLocalTime();
        }
    }
}
