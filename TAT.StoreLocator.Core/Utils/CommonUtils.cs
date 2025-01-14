﻿using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using TAT.StoreLocator.Core.Helpers;

namespace TAT.StoreLocator.Core.Utils
{
    public static class CommonUtils
    {
        public static string UrlFriendly(string text, int maxLength = 0)
        { // Return empty value if text is null
            if (text == null)
            {
                return "";
            }

            string normalizedString = text
                // Make lowercase
                .ToLowerInvariant()
                // Normalize the text
                .Normalize(NormalizationForm.FormD);

            StringBuilder stringBuilder = new();
            bool prevdash = false;
            int trueLength = 0;

            char c;

            for (int i = 0; i < normalizedString.Length; i++)
            {
                c = normalizedString[i];

                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    // Check if the character is a letter or a digit if the character is a
                    // international character remap it to an ascii valid character
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        _ = c < 128 ? stringBuilder.Append(c) : stringBuilder.Append(GlobalConstants.RemapInternationalCharToAscii(c));

                        prevdash = false;
                        trueLength = stringBuilder.Length;
                        break;

                    // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.MathSymbol:
                        if (!prevdash)
                        {
                            _ = stringBuilder.Append('-');
                            prevdash = true;
                            trueLength = stringBuilder.Length;
                        }
                        break;
                }

                // If we are at max length, stop parsing
                if (maxLength > 0 && trueLength >= maxLength)
                {
                    break;
                }
            }

            // Trim excess hyphens
            string result = stringBuilder.ToString().Trim('-');

            // Remove any excess character to meet maxlength criteria
            return maxLength <= 0 || result.Length <= maxLength ? result : result[..maxLength];
        }

        public static byte[] ConvertFileToByteArray(IFormFile file)
        {
            byte[] fileBytes = new byte[0];
            if (file is not null)
            {
                using MemoryStream memoryStream = new();
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            return fileBytes;
        }

        public static string GetClaimUserName(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        public static string GetClaimUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(UserClaims.Id)?.Value ?? "";
        }

        public static string vietnameseReplace(string str)
        {
            for (int i = 1; i < GlobalConstants.VietNamChar.Length; i++)
            {
                for (int j = 0; j < GlobalConstants.VietNamChar[i].Length; j++)
                {
                    str = str.Replace(GlobalConstants.VietNamChar[i][j], GlobalConstants.VietNamChar[0][i - 1]);
                }
            }
            return str.ToUpper();
        }


        public static string vietnameseNotUpperReplace(string str)
        {
            for (int i = 1; i < GlobalConstants.VietNamChar.Length; i++)
            {
                for (int j = 0; j < GlobalConstants.VietNamChar[i].Length; j++)
                {
                    str = str.Replace(GlobalConstants.VietNamChar[i][j], GlobalConstants.VietNamChar[0][i - 1]);
                }
            }
            return str;
        }

        public static string ReplaceProvincePatterns(string province)
        {
            if (string.IsNullOrEmpty(province))
            {
                return province;
            }

            string pattern = @"\b(?:city|thanh\s*pho|tinh)\b";
            string cleanedProvince = Regex.Replace(province, pattern, "", RegexOptions.IgnoreCase).Trim();

            return vietnameseNotUpperReplace(cleanedProvince);
        }


        public static string ReplaceDistrictPatterns(string district)
        {
            if (string.IsNullOrEmpty(district))
            {
                return district;
            }


            string pattern = @"\b(?:quan|q\.|district|h\.|huyen|thanh pho)\b";
            string cleanedDistrict = Regex.Replace(district, pattern, "", RegexOptions.IgnoreCase).Trim();

            return vietnameseNotUpperReplace(cleanedDistrict);
        }

        public static string ReplaceWardPatterns(string ward)
        {
            if (string.IsNullOrEmpty(ward))
            {
                return ward;
            }
            string pattern = @"\b(?:phuong|p\.|ward)\b";

            string cleanedWard = Regex.Replace(ward, pattern, "", RegexOptions.IgnoreCase).Trim();

            return vietnameseNotUpperReplace(cleanedWard);
        }



        public static string? NormalizeEmail(string email)
        {
            try
            {
                // Attempt to create a MailAddress instance with the provided email
                MailAddress mailAddress = new(email);

                // If the MailAddress instance was created successfully, return the normalized email address
                return mailAddress.Address.ToLowerInvariant();
            }
            catch (FormatException)
            {
                // If the provided email address is not in a valid format, return null or throw an exception
                return null;
            }
        }

        public static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    _ = stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string NormalSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty; // Return an empty string if the input is null or empty
            }

            // Loại bỏ dấu tiếng Việt và chuyển đổi về chữ thường
            string normalizedText = vietnameseReplace(text).ToLowerInvariant();

            string result = new(normalizedText.Where(char.IsLetterOrDigit).ToArray());

            return result;



        }

        public static string GenerateRandomPhoneNumber()
        {
            Random random = new();

            // Generate a random 9-digit number (excluding the country code)
            string phoneNumber = random.Next(100000000, 1000000000).ToString();

            // Prepend the country code for Vietnam (+84)
            phoneNumber = "+84" + phoneNumber;

            return phoneNumber;
        }

        public static DateTimeOffset GenerateRandomDateOfBirth(int minAge, int maxAge)
        {
            Random random = new();

            // Calculate minimum and maximum birth years based on the provided age range
            int currentYear = DateTime.Now.Year;
            int minBirthYear = currentYear - maxAge;
            int maxBirthYear = currentYear - minAge;

            // Generate a random birth year within the specified range
            int birthYear = random.Next(minBirthYear, maxBirthYear + 1);

            // Generate a random birth month and day
            int birthMonth = random.Next(1, 13);
            int maxDayInMonth = DateTime.DaysInMonth(birthYear, birthMonth);
            int birthDay = random.Next(1, maxDayInMonth + 1);

            // Generate a random birth hour, minute, second, and millisecond
            int birthHour = random.Next(0, 24);
            int birthMinute = random.Next(0, 60);
            int birthSecond = random.Next(0, 60);
            int birthMillisecond = random.Next(0, 1000);

            // Create a DateTimeOffset object with the generated date and time
            DateTimeOffset dateOfBirth = new(birthYear, birthMonth, birthDay,
                birthHour, birthMinute, birthSecond, birthMillisecond, TimeSpan.Zero);

            return dateOfBirth;
        }

        public static DateTimeOffset ToDatetimeOffsetFromUtc(this DateTime date)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(date, DateTimeKind.Utc));
        }
    }
}