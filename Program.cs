using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Globalization;

namespace TimeCheck
{
    class Program
    {
        private static readonly IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

        static void Main(string[] args)
        {
            try
            {
                DateTime dateTimeToConvert;
                if(args.Any())
                {
                    string passedInTime = args.Length == 2 ? string.Concat(args[0], args[1]) : args[0];
                    
                    if (!TryParseDateTime(passedInTime, out dateTimeToConvert))
                    {
                        Console.WriteLine($"Failed to convert {passedInTime} into a valid time");
                        return;
                    }
                }
                else
                {
                    dateTimeToConvert = DateTime.Now;
                    Console.WriteLine($"No time was entered, checking current time {dateTimeToConvert:h:mm tt}");
                }

                foreach (var timeZone in GetTimeZones().TimeZones)
                {
                    Console.WriteLine("{0}: {1}", timeZone.DisplayName, GetZoneDateTime(dateTimeToConvert, timeZone.Id));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static bool TryParseDateTime(string time, out DateTime dateTimeOut)
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
            var timeZones = configurationRoot.GetSection("TimeZones")
                    .GetChildren()
                    .Select(x => new {
                        DisplayName = x.GetValue<string>("DisplayName"),
                        Id = x.GetValue<string>("Id")
                    });
            
            if(timeZones.Any())
            {
                return new { TimeZones = timeZones };
            }
            else
            {
                throw new Exception("A TimeZones section with an array of entries is required in the applications.json file. An example of TimeZones entries format: { \"DisplayName\": \"Central  \", \"Id\": \"America/Chicago\"}");
            }
        }

        private static string GetDateTimeDisplayFormat()
        {
            return configurationRoot.GetSection("DateTimeDisplayFormat").Get<string>();
        }
    }
}
