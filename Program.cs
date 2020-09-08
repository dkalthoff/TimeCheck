using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Globalization;

namespace TimeCheck
{
    class Program
    {
        private static readonly IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();

        static void Main(string[] args)
        {
            try
            {
                string passedInTime = args.Length == 2 ? args[0] + args[1] : args[0];
                DateTime dateTimeToConvert;
                if (ParseDateTime(passedInTime, out dateTimeToConvert))
                {
                    foreach (var timeZone in GetTimeZones().TimeZones)
                    {
                        Console.WriteLine("{0}: {1}", timeZone.DisplayName, GetZoneDateTime(dateTimeToConvert, timeZone.Id));
                    }
                }
                else
                {
                    Console.WriteLine("Couldn't convert time entered {0}", passedInTime);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static bool ParseDateTime(string time, out DateTime dateTimeOut)
        {
            return DateTime.TryParseExact(time, "h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeOut) ||
                    DateTime.TryParseExact(time, "hmmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeOut);
        }

        private static string GetZoneDateTime(DateTime dateTimeToConvert, string timeZoneId)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime convertedDateTime = TimeZoneInfo.ConvertTime(dateTimeToConvert, timeZoneInfo);
            return string.Format("{0} Zone: {1}", convertedDateTime.ToString(GetDateTimeDisplayFormat()), timeZoneInfo.DisplayName);
        }

        private static dynamic GetTimeZones()
        {
            var timeZones = config.GetSection("TimeZones")
                    .GetChildren()
                    .ToList()
                    .Select(x => new {
                        DisplayName = x.GetValue<string>("DisplayName"),
                        Id = x.GetValue<string>("Id")
                    });

            return new { TimeZones = timeZones };
        }

        private static string GetDateTimeDisplayFormat()
        {
            return config.GetSection("DateTimeDisplayFormat").Get<string>();
        }
    }
}
