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
        public const string WishlistUserId = "62AC35DB-100C-46A9-9413-A79107439A76";
        public const string WishlistAdminId = "BD4D2BE5-FEE8-4213-9648-76FF9BDBC835";
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


        public const string USERNAME_NOT_FOUND = "USERNAME_NOT_FOUND";
        public const string PIN_TIME_OUT = "PIN_TIME_OUT";
        public const string PIN_INVALID = "PIN_INVALID";
        public const string UNAUTHORIZED = "UNAUTHORIZED";
        public const string USER_NOT_FOUND = "USER_NOT_FOUND";
        public const string USER_DATA_FAILED = "USER_DATA_FAILED";
        public const string NOT_FOUND = "NOT_FOUND";
        public const string PROFILE_NOT_FOUND = "PROFILE_NOT_FOUND";
        public const string USER_EXISTED = "USER_EXISTED";
        public const string USER_ALREADY_USED = "USER_ALREADY_USED";
        public const string USER_HAS_BEEN_LOCKED = "USER_HAS_BEEN_LOCKED";
        public const string USER_HAS_BEEN_DELETED = "USER_HAS_BEEN_DELETED";
        public const string GROUP_ALREADY_USED = "GROUP_ALREADY_USED";
        public const string EMAIL_NOT_FOUND = "EMAIL_NOT_FOUND";
        public const string PASSWORD_INVALID = "PASSWORD_INVALID";
        public const string LOGIN_ERROR = "LOGIN_ERROR";
        public const string CURRENT_PASSWORD_MISMATCH = "CURRENT_PASSWORD_MISMATCH";
        public const string RESET_PASSWORD_FAIL = "RESET_PASSWORD_FAIL";
        public const string RESET_PASSWORD_SUCCESS = "RESET_PASSWORD_SUCCESS";
        public const string TOKEN_INVALID = "TOKEN_INVALID";
        public const string CODE_EXISTS = "CODE_EXISTS";
        public const string SURVEY_COPY_CODE_EXISTS = "SURVEY_COPY_CODE_EXISTS";
        public const string PHONE_EMAIL_IDNUMBER_EXISTS = "PHONE_EMAIL_IDNUMBER_EXISTS";
        public const string NAME_EXISTS = "NAME_EXISTS";
        public const string SETTING_AUTO_ASSIGN_EXISTS = "SETTING_AUTO_ASSIGN_EXISTS";
        public const string EMAIL_EXISTED = "EMAIL_EXISTED";
        public const string CAN_NOT_EDIT_SUPERADMIN = "CAN_NOT_EDIT_SUPERADMIN";
        public const string CAN_NOT_DELETE_SUPERADMIN = "CAN_NOT_DELETE_SUPERADMIN";
        public const string CAN_NOT_DELETE_USER = "CAN_NOT_DELETE_USER";
        public const string CAN_NOT_DELETE_SUPPERADMIN = "CAN_NOT_DELETE_SUPPERADMIN";
        public const string CAN_NOT_DELETE_USER_IS_USER = "CAN_NOT_DELETE_USER_IS_USER";
        public const string CAN_NOT_GET_ACCOUNT_LIST = "CAN_NOT_GET_ACCOUNT_LIST";
        public const string USER_DEACTIVED = "USER_DEACTIVED";
        public const string NOT_ALLOW = "NOT_ALLOW";
        public const string UPDATE_FAIL = "UPDATE_FAIL";
        public const string UPDATE_SUCCESSFULL = "UPDATE_SUCCESSFULL";
        public const string SUCCESSFULL = "SUCCESSFULL";
        public const string TEAM_FULLS = "TEAM_FULLS";
        public const string INSERT_FAIL = "INSERT_FAIL";
        public const string FAIL = "FAIL";
        public const string INSERT_SUCCESSFULL = "INSERT_SUSSESFULL";
        public const string SYSTEM_ERROR = "SYSTEM_ERROR";
        public const string EXISTS = "EXISTS";
        public const string LEAD_ASSIGN_EXISTS = "LEAD_ASSIGN_EXISTS";
        public const string DELETE_FAIL = "DELETE_FAIL";
        public const string DELETE_SUCCESSFULL = "DELETE_SUCCESSFULL";
        public const string ROLE_NOT_FOUND = "ROLE_NOT_FOUND";
        public const string ACCOUNT_ALREADY_EXIST = "ACCOUNT_ALREADY_EXIST";
        public const string VALUE_YOU_FIND_NOT_EXITS = "VALUE_YOU_FIND_NOT_EXITS";
        public const string CREATE_TABLE_JOBTITLE_FAIL = "CREATE_TABLE_JOBTITLE_FAIL";

        //addmore 
        public const string DATE_TIME_INPUT_ERROR = "DATE_TIME_INPUT_ERROR";
        public const string USER_NOT_ALLOWED = "USER_NOT_ALLOWED";
        public const string CAN_NOT_RATING = "CAN_NOT_RATING";
        public const string MESSAGE_FOR_CREATE_CONSULTING = "IF YOU ISN'T CONSULTANT PLEASE INPUT CONSUTANT_ID AND BRAND_ID";


        //NEW 10/4
        public const string CUSTOMER_NOT_FOUND = "CUSTOMER_NOT_FOUND";
        public const string WARD_NOT_FOUND = "WARD_NOT_FOUND";
        public const string DISTRICT_NOT_FOUND = "DISTRICT_NOT_FOUND";
        public const string UPLOAD_FAIL = "UPLOAD_FAIL";
        public const string UPLOAD_SUCCESSFULL = "UPLOAD_SUCCESSFULL";
        public const string PROVINCE_NOT_FOUND = "PROVINCE_NOT_FOUND";
        public const string CONSULTING_NOT_FOUND = "CONSULTING_NOT_FOUND";
        public const string USER_DONT_HAVE_THIS_CONSULTING_ = "USER_DONT_HAVE_THIS_CONSULTING";
        public const string SUPERVISOR_NOT_FOUND = "CONSULTING_NOT_FOUND";
        public const string SUPERVISOR_NOT_SAME_BRANCH = "SUPERVISOR_NOT_SAME_BRAND";


        //new 12/4 
        public const string CREATE_HISTORY_FAIL = "CREATE_HISTORY_FAIL";
        public const string RATING_SUCCESSFUL = "RATING_SUCCESSFUL";
        public const string CANCEL_SUCCESSFUL = "CANCEL_SUCCESSFUL";
        public const string CANCEL_FAIL = "CANCEL_FAIL";

        //new 15/4
        public const string OTP_OUT_TIME = "OTP_OUT_TIME";
        public const string OTP_SEND_SUCCESSFUL = "OTP_SEND_SUCCESSFUL";
        public const string OTP_SEND_FAIL = "OTP_SEND_FAIL";
        public const string OTP_WRONG = "OTP_WRONG";
        public const string OTP_VERIFY_SUCCESSFUL = "OTP_VERIFY_SUCCESSFUL";
        public const string OTP_VERIFY_FAIL = "OTP_VERIFY_FAIL";

        //20/4
        public const string USER_NOT_CONSULTANT = "USER_NOT_CONSULTANT";

        //22/4
        public const string DATE_OUT_RANGE = "DATE_OUT_RANGE";
        public const string MONTH_NOT_SAME = "MONTH_NOT_SAME";

        public const string NOT_STORE_NEAR = "NOT_STORE_NEAR";


    }
}
