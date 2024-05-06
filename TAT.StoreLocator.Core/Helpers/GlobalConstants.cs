namespace TAT.StoreLocator.Core.Helpers
{
    public static class GlobalConstants
    {

        public const string RoleAdminId = "4D921CC6-5349-46FB-B62C-EBF293B217A5";
        public const string RoleUserId = "D0C7A5D9-9F7B-4B7B-9C6F-8D6F6F6F6F6F";
        public const string UserId = "F0C7A5D9-9F7B-4B7B-9C6F-8D6F6F6F6F6F";
        public const string AdminId = "4D921CC6-5349-46FB-B62C-EBF293B217A5";
        public const string AddressUserId = "0C2CCCBB-7326-45FE-B800-F348CF4C37CF";
        public const string AddressAdminId = "C0FE97C7-918F-4E48-97A6-2B081D56372C";
        public const string EmailAdmin = "admin@gmail.com";
        public const string RoleAdminName = "Admin";
        public const string RoleUserName = "User";
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else
            {
                string result = c switch
                {
                    'ĵ' => "j",
                    'ř' => "r",
                    'ł' => "l",
                    'đ' => "d",
                    'ß' => "ss",
                    'þ' => "th",
                    'ĥ' => "h",
                    _ => "ğČćĎĄęĖėĘėĢģīıİĮįŠšūŲūŪūĔĕ".Contains(c) ? c.ToString().ToLower() : "",
                };
                return result;
            }
        }
        public static readonly string[] VietNamChar = new string[] { "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };

        public const string RefreshTokenCookieName = "refreshToken";
        public const string MessageUserNotFound = "User not found";
        public const string MessagePasswordAndConfirmPasswordNotMatch = "Password and confirm new password do not match.";
        public const string MessageTokenNotFound = "Token not found";
        public const string MessageFailToResetPassword = "Fail to reset password";




    }
}
