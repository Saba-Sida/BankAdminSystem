using BankAdminSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAdminSystem.Services
{
    public static class Generator
    {
        public static Random Random = new();

        public static string GenerateBankAccountId(Account forAccount)
        {
            if(forAccount.BankAccounts == null || forAccount.BankAccounts.Count == 0) return GenerateBankAccountIdAssist();

            string newId = GenerateBankAccountIdAssist();
            while (forAccount.BankAccounts.Any(x => x.Id == newId))
            {
                newId = GenerateBankAccountIdAssist();
            }

            return newId;
        }

        private static string GenerateBankAccountIdAssist()
        {
            StringBuilder idStringBuilder = new();

            for (int i = 0; i < 5; i++)
            {
                idStringBuilder.Append(Random.Next(0, 10));
            }

            return idStringBuilder.ToString();
        }
    }
}
