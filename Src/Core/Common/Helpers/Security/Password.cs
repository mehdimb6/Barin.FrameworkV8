using System.Text.RegularExpressions;

namespace Barin.Framework.Common.Helpers.Security;

public static class Password
{
    public static string HashCode(string userName, string password)
    {
        var text = $"{userName.Trim().ToLower()}#{password.Trim()}!";
        return Hash.GetHashCode(text);
    }

    public static string HashSha256(string userName, string password)
    {
        var text = $"{userName.Trim().ToLower()}#{password.Trim()}!";
        return Hash.GetHashSha256(text);
    }

    public struct StrongPasswordModel
    {
        /// <summary>
        /// بررسی پیچیدگی پسورد
        /// </summary>
        public bool CheckNumber { get; set; }

        /// <summary>
        /// بررسی وجود حروف کوچک انگلیسی در متن
        /// </summary>
        public bool CheckLowercase { get; set; }

        /// <summary>
        /// بررسی وجود حروف بزرگ انگلیسی در متن
        /// </summary>
        public bool CheckUppercase { get; set; }

        /// <summary>
        /// بررسی وجود کاراکترهای خاص در متن
        /// </summary>
        public bool CheckSpecial { get; set; }

        /// <summary>
        /// بررسی طول متن
        /// </summary>
        public byte? Length { get; set; }
    }

    /// <summary>
    /// بررسی پیچیدگی پسورد
    /// </summary>
    public static string IsStrong(this string str, StrongPasswordModel config)
    {
        if (config.CheckNumber && !Regex.IsMatch(str, @"[\d]"))
            return "کلمه عبور باید شامل اعداد باشد";

        if (config.CheckLowercase && !Regex.IsMatch(str, @"[a-z]"))
            return "کلمه عبور باید شامل حروف انگلیسی کوچک باشد";

        if (config.CheckUppercase && !Regex.IsMatch(str, @"[A-Z]"))
            return "کلمه عبور باید شامل حروف انگلیسی بزرگ باشد";

        if (config.CheckSpecial && !Regex.IsMatch(str, @"[\s~!@#\$%\^&\*\(\)\{\}\|\[\]\\:;'?,.`+=<>\/]"))
            return "کلمه عبور باید شامل کاراکترهای خاص باشد";

        if (config.Length.HasValue && str.Length < config.Length)
            return $"حداقل طول کلمه عبور باید {config.Length} حرف باشد";

        return "";
    }
}
