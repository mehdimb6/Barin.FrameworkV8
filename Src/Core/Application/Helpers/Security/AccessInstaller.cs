using Microsoft.Win32;
using System.Configuration;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Barin.Framework.Application.Helpers.Security;

public static class AccessInstaller
{
    private static string BaseSecurityPath
    {
        get
        {
            var path = ConfigurationManager.AppSettings["BaseSecurityPath"];
            if (string.IsNullOrWhiteSpace(path))
                return @"c:\windows\system32\";

            path = path.Trim();
            return path.EndsWith(@"\") ? path : $@"{path}\";
        }
    }

    private static string ActivatorPath
    {
        get
        {
            var expireKey = ConfigurationManager.AppSettings["ActivatorKey"];
            var path = $@"{BaseSecurityPath}{expireKey}.lic";

            return path;
        }
    }

    private static string _applicationName = "";
    private static string _applicationVersion = "";

    public static bool CheckForUse(string applicationName, string applicationVersion, out string message)
    {
        var result = File.Exists(ActivatorPath);
        message = "Application Activated";

        _applicationName = applicationName;
        _applicationVersion = applicationVersion;

        try
        {
            if (result)
            {
                var activator = $"activator:{CalculateActivator(GetHardwareHash())}";
                var lines = File.ReadAllLines(ActivatorPath);

                if (lines[1].ToLower() != activator.ToLower())
                {
                    result = false;
                    message = "Invalid Activator";
                }
            }
            else //یعنی فایل نیست و برنامه تازه نصب شده
            {
                message = "Activator not found";
                var fs = new FileStream(ActivatorPath, FileMode.Create);
                var sw = new StreamWriter(fs);

                var codeRequest = $"codeRequest:{GetHardwareHash()}";
                var activator = "activator:";
                sw.WriteLine(codeRequest);
                sw.WriteLine(activator);

                sw.Close();
                fs.Close();
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            result = false;
        }

        return result;
    }

    #region Private Methods

    private static string GetHardwareHash() => CalculateMD5Hash($"{_applicationName}{GetMotherBoardId()}{GetProccessId()}{_applicationVersion}");

    private static string CalculateActivator(string hardwareHash) => CalculateMD5Hash($"{_applicationName}{hardwareHash}{_applicationVersion}");

    private static string GetMotherBoardId()
    {
        string motherBoardInfo = string.Empty;
        ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
        scope.Connect();
        ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

        foreach (PropertyData propData in wmiClass.Properties)
        {
            if (propData.Name == "SerialNumber")
                motherBoardInfo = string.Format("{0,-25}{1}", propData.Name, Convert.ToString(propData.Value));
        }

        return motherBoardInfo;
    }

    private static string GetProccessId()
    {
        var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
        ManagementObjectCollection mbsList = mbs.Get();
        string id = "";
        foreach (ManagementObject mo in mbsList)
        {
            id = mo["ProcessorId"].ToString();
            break;
        }

        return id;
    }

    private static string GetMachineGuid()
    {
        string keyValue = "";

        try
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography");
            keyValue = (string)registryKey.GetValue("MachineGuid");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return keyValue;
    }

    private static string CalculateMD5Hash(string input)
    {
        // step 1, calculate MD5 hash from input
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
            sb.Append(hash[i].ToString());

        return sb.ToString();
    }

    #endregion
}
