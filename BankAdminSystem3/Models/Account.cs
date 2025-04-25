using BankAdminSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAdminSystem.Models
{
    public class Account
    {
        public string UserName { get; set; }
        public string PasswordEncrypt
        {
            get
            {
                return MyCrypter.Encrypt(_password);
            }
            set
            {
                _password = MyCrypter.Decrypt(value);
            }
        }
        
        private string _password = string.Empty;
        public List<BankAccount> BankAccounts { get; set; }  = new();        



        public void AddBankAccount(double initialBalance)
        {
            BankAccount newBankAccount = new()
            {
                Id = Generator.GenerateBankAccountId(this),
                Balance = initialBalance
            };
            BankAccounts.Add(newBankAccount);
        }

        public void AddBankAccount(BankAccount bankAccount)
        {
            BankAccounts.Add(bankAccount);
        }   


        public string GetAccountFullInfo()
        {
            StringBuilder accountInfo = new();
            accountInfo.AppendLine($"Account name: {UserName}");
            accountInfo.AppendLine($"Password: {MyCrypter.Decrypt(PasswordEncrypt)}");
            accountInfo.AppendLine($"Bank accounts count: {BankAccounts.Count}");
            foreach (var bankAccount in BankAccounts)
            {
                accountInfo.AppendLine($" - Bank Account ID: {bankAccount.Id}, Balance: ${bankAccount.Balance}");
            }
            return accountInfo.ToString();
        }

    }
}
