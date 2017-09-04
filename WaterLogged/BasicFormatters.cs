using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WaterLogged
{
    public class BasicFormatters
    {
        public Process CurrentProcess { get; private set; }
        public Assembly CurrentAssembly { get; private set; }
        public  Dictionary<string, int> Counters { get; private set; }
        
        //MISC
        //Call site
        //Stack trace
        //GC info
        //Thread info
        //NewLine

        public BasicFormatters()
        {
            CurrentProcess = Process.GetCurrentProcess();
            CurrentAssembly = Assembly.GetEntryAssembly();
            Counters = new Dictionary<string, int>();
        }

        public string GetMemoryUsage(FormatData data)
        {
            double bytes = CurrentProcess.WorkingSet64;
            string sizeSuffix = "";
            string arg = data.Argument.ToLower();
            if (arg == "a")
            {
                arg = "b";
                sizeSuffix = "bytes";
                if (bytes > 1024)
                {
                    sizeSuffix = "KB";
                    bytes /= 1024;
                }
                if (bytes > 1024)
                {
                    sizeSuffix = "MB";
                    bytes /= 1024;
                }
                if (bytes > 1024)
                {
                    sizeSuffix = "GB";
                    bytes /= 1024;
                }
                if (bytes > 1024)
                {
                    sizeSuffix = "TB";
                    bytes /= 1024;
                }
            }
            else if (arg == "k")
            {
                sizeSuffix = "KB";
                bytes /= 1024;
            }
            else if (arg == "m")
            {
                sizeSuffix = "MB";
                bytes /= 1048576;
            }
            else if (arg == "g")
            {
                sizeSuffix = "GB";
                bytes /= 1073741824;
            }
            else
            {
                arg = "b";
                sizeSuffix = "bytes";
            }

            return string.Format("{0} {1}", bytes, sizeSuffix);
        }

        public string GetCpuUsage(FormatData data)
        {
            return string.Format("{0:" + data.Argument + "}", CurrentProcess.TotalProcessorTime);
        }

        public string GetProcessName(FormatData data)
        {
            return CurrentProcess.ProcessName;
        }

        public string GetProcessDir(FormatData data)
        {
            return CurrentProcess.StartInfo.WorkingDirectory;
        }

        public string GetProcessId(FormatData data)
        {
            return string.Format("{0:" + data.Argument + "}", CurrentProcess.Id);
        }

        public string GetAssemblyName(FormatData data)
        {
            return CurrentAssembly.GetName().Name;
        }

        public string GetAssemblyVersion(FormatData data)
        {
            return string.Format("{0}", CurrentAssembly.ImageRuntimeVersion);
        }

        //Source: https://stackoverflow.com/a/1600990
        public string GetBuildDate(FormatData data)
        {
            var filePath = CurrentAssembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(-secondsSince1970);

            var tz = TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTime(linkTimeUtc, TimeZoneInfo.Utc, TimeZoneInfo.Local);

            return string.Format("{0:" + data.Argument + "}", localTime);
        }

        public string GetAssembly(FormatData data)
        {
            return CurrentAssembly.Location;
        }

        public string GetMachineName(FormatData data)
        {
            return Environment.MachineName;
        }

        public string GetUserName(FormatData data)
        {
            throw new NotImplementedException("Waiting for .net CORE to support this.");
        }

        public string GetEnvironmentVar(FormatData data)
        {
            return Environment.GetEnvironmentVariable(data.Argument);
        }

        public string GetOS(FormatData data)
        {
            return RuntimeInformation.OSDescription;
        }

        public string GetArchitecture(FormatData data)
        {
            return RuntimeInformation.OSArchitecture.ToString();
        }

        public string GetRegistry(FormatData data)
        {
            throw new NotImplementedException("Waiting for me to implement this.");
        }

        public string GetNetworkConnected(FormatData data)
        {
            throw new NotImplementedException("Waiting for me to implement this.");
        }

        public string GetInternetConnected(FormatData data)
        {
            throw new NotImplementedException("Waiting for me to implement this.");
        }

        public string GetIpAddress(FormatData data)
        {
            throw new NotImplementedException("Waiting for me to implement this.");
        }

        public string GetHostName(FormatData data)
        {
            return System.Net.Dns.GetHostName();
        }

        public string GetCounter(FormatData data)
        {
            string counter = data.Argument;
            if (string.IsNullOrEmpty(counter))
            {
                counter = "&all";
            }
            if (Counters.ContainsKey(counter))
            {
                Counters[counter] += 1;
            }
            else
            {
                Counters.Add(counter, 0);
            }
            return Counters[counter].ToString();
        }
    }
}
