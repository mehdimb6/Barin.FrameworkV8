using System;
using System.Diagnostics;
using System.IO;

namespace Barin.Framework.Common.Helpers;

public static class FileHelper
{
    #region Private Const Variables

    private const long KiloByte = 1024;
    private const long MegaByte = 1024 * 1024;
    private const long GigaByte = 1024 * 1024 * 1024;

    private const string ByteText = " (B)";
    private const string KiloByteText = " (KB)";
    private const string MegaByteText = " (MB)";
    private const string GigaByteText = " (GB)";

    private static long _counter;

    #endregion

    public static string FileSizeText(long fileSize)
    {
        var volume = fileSize / GigaByte;
        if (volume > 0)
        {
            return volume + GigaByteText;
        }
        volume = fileSize / MegaByte;
        if (volume > 0)
        {
            return volume + MegaByteText;
        }
        volume = fileSize / KiloByte;
        if (volume > 0)
        {
            return volume + KiloByteText;
        }
        return fileSize + ByteText;
    }

    public static bool SaveToFile(ref string fullFilePath)
    {
        try
        {
            var b = new byte[0];
            return SaveToFile(b, ref fullFilePath);
        }
        catch
        {
            return false;
        }
    }

    public static bool SaveToFile(byte[] content, ref string fullFilePath)
    {
        try
        {
            var dir = Path.GetDirectoryName(fullFilePath);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                if (dir != null)
                    foreach (var fileName in Directory.GetFiles(dir))
                    {
                        try
                        {
                            File.Delete(fileName);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
            }
            if (File.Exists(fullFilePath))
            {
                fullFilePath = Path.GetDirectoryName(fullFilePath) + @"\" + GetTempName() + Path.GetExtension(fullFilePath);
            }
            var fs = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            if (!fs.CanWrite)
                return false;
            var bw = new BinaryWriter(fs);
            bw.Write(content);
            bw.Close();
            fs.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static byte[] LoadFromFile(string fullFilePath)
    {
        try
        {
            FileStream fs;

            try
            {
                fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (IOException)
            {
                File.Copy(fullFilePath, fullFilePath + "2", true);
                fs = new FileStream(fullFilePath + "2", FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            if (!fs.CanRead)
                return null;

            var br = new BinaryReader(fs);
            var content = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();

            if (File.Exists(fullFilePath + "2"))
                File.Delete(fullFilePath + "2");

            return content;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public static bool DeleteDocumentFile(string fullFilePath)
    {
        try
        {
            File.Delete(fullFilePath);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GetTempName()
    {
        _counter++;
        return _counter.ToString();
    }
}
