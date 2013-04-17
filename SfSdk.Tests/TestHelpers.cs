using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using SfSdk.Constants;
using Xunit;

namespace SfSdk.Tests
{
    public static class TestHelpers
    {
        public static void Connect()
        {
            SetDhcp();
        }

        public static void Disconnect()
        {
            SetIp("192.168.0.4", "255.255.255.0");
        }

        private static void SetIp(string ipAddress, string subnetMask)
        {
            var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc.Cast<ManagementObject>().Where(mo => (bool)mo["IPEnabled"]))
            {
                try
                {
                    ManagementBaseObject newIp = mo.GetMethodParameters("EnableStatic");
                    newIp["IPAddress"] = new[] { ipAddress };
                    newIp["SubnetMask"] = new[] { subnetMask };
                    mo.InvokeMethod("EnableStatic", newIp, null);
                }
                catch (Exception generatedExceptionName)
                {
                    Debug.WriteLine(generatedExceptionName.Message);
                    throw;
                }
            }
        }

        private static void SetDhcp()
        {
            var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc.Cast<ManagementObject>().Where(mo => (bool)mo["IPEnabled"]))
            {
                ManagementBaseObject newDns = mo.GetMethodParameters("SetDNSServerSearchOrder");
                newDns["DNSServerSearchOrder"] = null;
                mo.InvokeMethod("EnableDHCP", null, null);
                mo.InvokeMethod("SetDNSServerSearchOrder", newDns, null);
            }
        }

        public static string ToValidSavegameString(this string s, string dummyValue = null)
        {
            string result = s;
            while (result.Split('/').Length < (int) SF.SgServerTime)
                result += "/" + dummyValue;
            return result;
        }

        public static string ToInvalidSavegameString(this string s, string dummyValue = null)
        {
            string result = s;
            while (result.Split('/').Length < (int) SF.SgServerTime - 1)
                result += "/" + dummyValue;
            return result;
        }
    }
}