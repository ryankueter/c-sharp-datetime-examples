/*
 * Author: Ryan Kueter
 * Copyright 2019 Ryan Kueter.
 */
using System;
using System.Text;
using System.IO;

namespace _DateTime
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(GetTimeZones());
            //Console.WriteLine(AddTimeSpan());
            //CompareUnixTimes();
            //OlderThanDays();

            Console.WriteLine(GetTimeZones());
            Console.ReadKey();
        }

        static string GetTimeZones()
        {
            var s = new StringBuilder();

            TimeZone t = TimeZone.CurrentTimeZone;

            s.AppendLine($"Your time zone is {t.GetUtcOffset(DateTime.Now).Hours.ToString()} from UTC (Coordinated Universal Time).");
            if (t.IsDaylightSavingTime(DateTime.Now) == false)
            {
                s.AppendLine($"You are on Standard Time (ST).");
            }
            else
            {
                s.AppendLine($"You are on Daylight Time (DT).");
            }
                            
            return s.ToString();
        }

        static string AddTimeSpan()
        {
            var firstSpan = new TimeSpan(12, 30, 0);
            var secondSpan = new TimeSpan(1, 30, 30);
            TimeSpan combinedSpan = firstSpan + secondSpan;
            return $"{firstSpan.ToString()} + {secondSpan.ToString()} = {combinedSpan.ToString()}";
        }

        static string AddDays()
        {
            return DateTime.Now.AddDays(-31).ToString();
        }

        static DateTime StringToDate()
        {
            string Runtime = "19/05/2015 07:44 PM";
            return DateTime.ParseExact(Runtime, "dd/MM/yyyy hh:mm tt", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        static void CompareUnixTimes()
        {
            FileInfo theFileInfo1 = new FileInfo(Path.GetTempPath());
            double writeTime1 = DateTimeToUNIXTime(theFileInfo1.LastWriteTime);

            FileInfo theFileInfo2 = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            double writeTime2 = DateTimeToUNIXTime(theFileInfo2.LastWriteTime);

            //The following computation is necessary due to the precision of what appears to be equal numbers
            //Reference: http://msdn.microsoft.com/en-us/library/ya2zha7s.aspx
            double difference = Math.Abs(writeTime1 * 0.00001);
            if (Math.Abs(writeTime1 - writeTime2) > difference)
            {
                Console.WriteLine("The temp file is newer than the application executable file.");
            }
            else
            {
                Console.WriteLine("The temp file is older than the application executable file.");
            }
        }

        public static DateTime? UNIXTimeToDateTime(double unixTime)
        {
            DateTime theTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTime).ToLocalTime();

            //Adjust for daylight savings time
            //This works for daylight time, not sure about transitioning between standard and daylight
            if (TimeZoneInfo.Local.IsDaylightSavingTime(theTime) == true & TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) == true)
            {
                theTime = theTime.AddHours(-1);
            }
            else if (TimeZoneInfo.Local.IsDaylightSavingTime(theTime) == false & TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) == true)
            {
                theTime = theTime.AddHours(1);
            }

            return theTime;
        }

        public static double DateTimeToUNIXTime(DateTime currDate)
        {
            try
            {
                //create Timespan by subtracting the value provided from the Unix Epoch
                TimeSpan span = (currDate - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

                //return the total seconds (which is a UNIX timestamp)
                return span.TotalMilliseconds;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        static void OlderThanDays()
        {
            DateTime timestamp = Convert.ToDateTime("3/12/2019");

            int numDays = (DateTime.Now - timestamp).Days;
            //Evalutation expired
            if (numDays > 30)
            {
                Console.WriteLine("Yes, more than 30 days.");
            }
            else
            {
                Console.WriteLine("No, not more than 30 days.");
            }
        }
    }
}
