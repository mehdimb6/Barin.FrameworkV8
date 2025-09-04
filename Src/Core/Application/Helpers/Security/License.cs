using Barin.Framework.Application.Enums;
using Barin.Framework.Common.Helpers;
using System.Configuration;

namespace Barin.Framework.Application.Helpers.Security;

public static class License
{
    // اگر تاریخ استفاده آزاد از تاریخ جاری بزرگتر بود استفاده معمولی و گرنه استفاده فقط خواندنی

    private static int FreeDate;
    private static int LockDate;
    private static int LastDate;
    private static int CurrentDate;

    private static LicenseType _status = LicenseType.None;
    public static LicenseType Status
    {
        get
        {
            if (_status == LicenseType.None)
                Read(LicensePath);

            return Check();
        }
    }

    private static string BaseSecurityPath
    {
        get
        {
            var path = ConfigurationManager.AppSettings["BaseSecurityPath"];
            if (path.IsNullOrWhiteSpace())
                return @"c:\windows\system32\";

            path = path.Trim();
            return path.EndsWith(@"\") ? path : $@"{path}\";
        }
    }

    private static string LicensePath
    {
        get
        {
            var expireKey = ConfigurationManager.AppSettings["ExpireKey"];
            var path = $@"{BaseSecurityPath}{expireKey}.dll";

            return path;
        }
    }

    private static void Read(string pathLicenseFile)
    {
        try
        {
            var fs = new FileStream(pathLicenseFile, FileMode.Open);
            var br = new BinaryReader(fs);

            FreeDate = br.ReadInt32();
            LockDate = br.ReadInt32();
            LastDate = br.ReadInt32();

            var validateFreeDate = FreeDate.IsValidDate();
            var validateLockDate = LockDate.IsValidDate();
            var validateLastDate = LastDate.IsValidDate();

            if (!validateFreeDate || !validateLockDate || !validateLastDate)
                _status = LicenseType.Lock;

            br.Close();
            fs.Close();
        }
        catch (Exception ex)
        {
            LogHelper.Fatal("Expire: " + ex.Message);
            _status = LicenseType.Lock;
        }
    }

    private static void Write(string pathLicenseFile)
    {
        try
        {
            var fs = new FileStream(pathLicenseFile, FileMode.Create);
            var bw = new BinaryWriter(fs);

            bw.Write(FreeDate);
            bw.Write(LockDate);
            bw.Write(LastDate);

            bw.Close();
            fs.Close();
        }
        catch (Exception ex)
        {
            LogHelper.Fatal("Expire: " + ex.Message);
        }
    }

    private static LicenseType Check()
    {
        string strCurrentDate = DateTimeHelper.DateFormat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, "");
        CurrentDate = int.Parse(strCurrentDate);

        if (LastDate > CurrentDate)//Hack
            _status = LicenseType.Lock;

        if (LockDate < CurrentDate)//Lock
            _status = LicenseType.Lock;
        else if (FreeDate >= CurrentDate)//Free for use
            _status = LicenseType.Free;
        else // end license
            _status = LicenseType.ReadOnly;

        if (LastDate < CurrentDate)//New day
            Write(LicensePath);

        return _status;
    }
}
