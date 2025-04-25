namespace BankAdminSystem.Models
{
    public class BankAccount
    {
        public string Id { get; set; }
        public double Balance { get; set; }


        public BankAccount() => Balance = 0;
        public BankAccount(double initialBalance) => Balance = initialBalance;
    }
}
