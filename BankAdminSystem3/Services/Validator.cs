using BankAdminSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAdminSystem.Services
{
    public static class Validator
    {
        public static bool IsUniqueNewUserName(string userName, List<Account> accounts)
        {
            if (accounts == null || accounts.Count == 0) return true;
            foreach (var account in accounts)
            {
                if (account.UserName == userName) return false;
            }
            return true;
        }

        public static bool IsValidNewPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (password.Length < 8) return false;
            if (!password.Any(char.IsDigit)) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            return true;
        }


        public static bool IsCorrectPassword(string inputPassword, Account forAccount)
        {
            return MyCrypter.Decrypt(forAccount.PasswordEncrypt) == inputPassword;
        }

        public static bool UserNameExists(string userName, List<Account> accounts)
        {
            return accounts.Count(account => account.UserName == userName) > 0;
        }
    }
}
