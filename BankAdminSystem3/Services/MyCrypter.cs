using System.Text;

namespace BankAdminSystem.Services
{
    public static class MyCrypter
    {
        public static string Encrypt(string data)
        {
            StringBuilder encryptedDataStringBuilder = new StringBuilder();
            foreach (char c in data)
            {
                int encryptedChar = ((int)c + 3) * 11;
                encryptedDataStringBuilder.Append(encryptedChar.ToString());
                encryptedDataStringBuilder.Append("-");
            }
            encryptedDataStringBuilder.Length--; // Remove last hyphen
            return encryptedDataStringBuilder.ToString();
        }
        
        public static string Decrypt(string encryptedData)
        {
            StringBuilder decryptedDataStringBuilder = new StringBuilder();

            string[] encryptedElements = encryptedData.Split('-');

            foreach (string element in encryptedElements)
            {
                int decryptedChar = int.Parse(element) / 11 - 3;
                decryptedDataStringBuilder.Append((char)decryptedChar);
            }
            return decryptedDataStringBuilder.ToString();
        }
    }
}
