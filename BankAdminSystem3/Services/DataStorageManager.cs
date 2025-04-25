using BankAdminSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankAdminSystem.Services
{
    static class DataStorageManager
    {
        private static string accountsListFilePath = "./accountsList.json";
        private static FileStream fileStream = null;

        public static void UpdateAccountsList(List<Account> accounts)
        {
            using (fileStream = new FileStream(accountsListFilePath, FileMode.Create, FileAccess.Write))
            {
                string jsonData = JsonSerializer.Serialize(accounts ?? new List<Account>{ });
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                fileStream.Write(data, 0, data.Length);
            }
        }

        public static List<Account> GetAccountsList()
        {
            string jsonData = string.Empty;

            using (fileStream = new FileStream(accountsListFilePath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
                
                jsonData = Encoding.UTF8.GetString(data);
                
                if (jsonData == null || jsonData == string.Empty) return new List<Account>();
            }

            List<Account> accountsList = JsonSerializer.Deserialize<List<Account>>(jsonData);

            return accountsList;
        }
    }
}
