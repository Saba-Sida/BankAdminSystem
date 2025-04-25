using BankAdminSystem.Models;

namespace MainSpace
{
    class Program
    {
        static void Main(string[] args)
        {
            BankSystem bankAdminSystem = new();
            bankAdminSystem.Start();
        }
    }
}