using BankAdminSystem.Services;
using BankAdminSystem2.Services;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BankAdminSystem.Models
{
    public class BankSystem
    {
        private List<Account> accounts;



        public BankSystem()
        {
            accounts = DataStorageManager.GetAccountsList();
        }


        public void Start()
        {
            Runner.RunNext(
                "Bank Admin System",
                () => Console.WriteLine("Welcome to the Bank System."),
                new()
                {
                    {"Register account", RegisterAccount },
                    {"Remove account", RemoveAccount },
                    {"View accounts", ViewAccounts },
                    {"Transact money", TransactMoney },
                }
                );
        }


        private void RegisterAccount()
        {
            void Registration()
            {
                Console.WriteLine("Input account name:");
                string accountName = MyReader.ReadString_WithCondition(
                    input => Validator.IsUniqueNewUserName(input, accounts) || input == "exit",
                    "Account name already exists, please choose another one."
                );

                if (accountName == "exit") return;

                Console.WriteLine("Input password:");
                string accountPassword = MyReader.ReadString_WithCondition(
                    input => Validator.IsValidNewPassword(input) || input == "exit",
                    "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one digit."
                );

                if (accountPassword == "exit") return;

                Console.WriteLine("Input initial balance:");
                double initialBalance = MyReader.ReadDouble_WithCondition(
                    input => input >= 0,
                    "Initial balance must be a positive number."
                );

                try
                {
                    Account newAccount = new()
                    {
                        UserName = accountName,
                        PasswordEncrypt = MyCrypter.Encrypt(accountPassword)
                    };
                    newAccount.AddBankAccount(initialBalance);

                    accounts.Add(newAccount);
                    DataStorageManager.UpdateAccountsList(accounts);
                }
                catch
                {
                    Console.WriteLine("Error while creating account.");
                    return;
                }

                Console.WriteLine("Account created successfully.");
            }

            Runner.RunNext(
                "Register account",
                Registration
            );
        }

        private void ViewAccounts()
        {
            void ViewAllAccounts()
            {
                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }
                foreach (var account in accounts)
                {
                    Console.WriteLine(account.GetAccountFullInfo());
                }
            }
            Runner.RunNext(
                "View all accounts",
                ViewAllAccounts,
                new Dictionary<string, Action>()
                {
                    {"Remove an account", RemoveAccount},
                    {"Add bank account to account", AddBankAccount},
                    {"Remove bank account from account", RemoveBankAccountFromAccount},
                    {"Deposit money to account", DepositToBankAccount},
                    {"Withdraw money from account", WithDrawFromBankAccount}
                }
            );
        }

        private void TransactMoney()
        {
            void Transact()
            {
                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                if (accounts.Count == 1)
                {
                    Console.WriteLine("No more than 1 account, (so transaction impossible)");
                    return;
                }

                Console.WriteLine("Accounts list");
                Console.WriteLine("-----------------");
                foreach (var account in accounts)
                {
                    Console.WriteLine($" - {account.UserName}");
                }
                Console.WriteLine("-----------------\n");

                Account senderAccount = null, recieverAccount = null;
                double amount = 0;

                Console.WriteLine("Input name of sender account: \n('exit' to cancel operation)");
                string senderAccountName = MyReader.ReadString_WithCondition(
                    input => Validator.UserNameExists(input, accounts) || input == "exit",
                    "Such account does not exist"
                    );

                senderAccount = accounts.FirstOrDefault(a => a.UserName == senderAccountName);

                Console.WriteLine(senderAccount.GetAccountFullInfo());

                Console.WriteLine("Input balance id for sender account");
                string senderBankAccountId = MyReader.ReadString_WithCondition(
                    input => senderAccount.BankAccounts.Count(a => a.Id == input) > 0 || input == "exit",
                    "Such account balance does not exist"
                );
                BankAccount senderbankAccount = senderAccount.BankAccounts.FirstOrDefault(a => a.Id == senderBankAccountId);

                Console.WriteLine("Input sending amount");
                string amountString = MyReader.ReadString_WithCondition(
                    input => (double.TryParse(input, out amount) && amount > 0 && amount <= senderbankAccount.Balance) || input == "exit",
                    "Invalid amount, please try again."
                );

                if (amountString == "exit") return;

                Console.WriteLine("Input name of reciever account: \n('exit' to cancel operation)");
                string recieverAccountName = MyReader.ReadString_WithCondition(
                    input => Validator.UserNameExists(input, accounts) || input == "exit",
                    "Such account does not exist"
                );
                if (recieverAccountName == "exit") return;

                recieverAccount = accounts.FirstOrDefault(a => a.UserName == recieverAccountName);

                Console.WriteLine(recieverAccount.GetAccountFullInfo());


                Console.WriteLine("Input balance id for sender account");
                string receiverBankAccountId = MyReader.ReadString_WithCondition(
                    input => recieverAccount.BankAccounts.Count(a => a.Id == input) > 0 || input == "exit",
                    "Such account balance does not exist"
                );
                if (receiverBankAccountId == "exit") return;

                BankAccount recieverBankAccount = recieverAccount.BankAccounts.FirstOrDefault(a => a.Id == receiverBankAccountId);

                Console.WriteLine($"{senderAccount.UserName} wants to send ${amount} by balance with {senderbankAccount.Balance} to {recieverAccount.UserName} with balance {recieverBankAccount.Balance}");

                try
                {
                    recieverBankAccount.Balance += amount;
                    senderbankAccount.Balance -= amount;
                    DataStorageManager.UpdateAccountsList(accounts);
                    Console.WriteLine("All done");

                }
                catch
                {
                    Console.WriteLine("Error while creating account.");
                    return;
                }

            }
            Runner.RunNext(
                "Transact money",
                Transact
            );
        }

        private void RemoveAccount()
        {
            void Remove()
            {
                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                Console.WriteLine("Accounts list");
                foreach (var account in accounts)
                {
                    Console.WriteLine($" - {account.UserName}");
                }

                Console.WriteLine("---------------------------------");
                Console.WriteLine("Input account name to remove:");

                string accountName = MyReader.ReadString_WithCondition(
                    input => Validator.UserNameExists(input, accounts) || input == "exit",
                    "Such account was not found"
                );
                if (accountName == "exit") return;

                Console.WriteLine("Are you sure to remove the account? (yes/no)");

                string confirmation = MyReader.ReadString_WithCondition(
                    input => input == "yes" || input == "no",
                    "Please answer with 'yes' or 'no'."
                );

                if (confirmation == "no")
                {
                    Console.WriteLine("Removing account was canceled");
                    return;
                }

                Account accountToRemove = accounts.FirstOrDefault(a => a.UserName == accountName);

                if (accountToRemove != null)
                {
                    accounts.Remove(accountToRemove);
                    DataStorageManager.UpdateAccountsList(accounts);
                    Console.WriteLine("Account removed successfully.");
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            Runner.RunNext(
                "Remove account",
                Remove
            );
        }

        private void AddBankAccount()
        {
            void Add()
            {
                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }
                Console.WriteLine("Accounts list");
                foreach (var account in accounts)
                {
                    Console.WriteLine($" - {account.UserName}");
                }
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Input account name to add bank account:");
                string accountName = MyReader.ReadString_WithCondition(
                    input => Validator.UserNameExists(input, accounts) || input == "exit",
                    "Such account was not found"
                );
                if (accountName == "exit") return;

                Account accountToAdd = accounts.FirstOrDefault(a => a.UserName == accountName);
                if (accountToAdd != null)
                {
                    Console.WriteLine("Input initial balance:");
                    double initialBalance = MyReader.ReadDouble_WithCondition(
                        input => input >= 0,
                        "Initial balance must be a positive number."
                    );
                    try
                    {
                        accountToAdd.AddBankAccount(initialBalance);
                        DataStorageManager.UpdateAccountsList(accounts);
                        Console.WriteLine("Bank account added successfully.");
                    }
                    catch
                    {
                        Console.WriteLine("Error while creating bank account.");
                        return;
                    }
                }
            }
            Runner.RunNext(
                "Add bank account",
                Add
            );
        }

        private void RemoveBankAccountFromAccount()
        {
            void Remove()
            {
                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }
                Console.WriteLine("Accounts list");
                foreach (var account in accounts)
                {
                    Console.WriteLine($" - {account.UserName}");
                }
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Input account name to remove bank account:");
                string accountName = MyReader.ReadString_WithCondition(
                    input => Validator.UserNameExists(input, accounts) || input == "exit",
                    "Such account was not found"
                );
                if (accountName == "exit") return;
                Account accountToRemoveBalance = accounts.FirstOrDefault(a => a.UserName == accountName);

                Console.WriteLine(accountToRemoveBalance.GetAccountFullInfo());

                if (accountToRemoveBalance != null)
                {
                    if (accountToRemoveBalance.BankAccounts.Count <= 1)
                    {
                        Console.WriteLine("Emptying all bank accounts not allowed.");
                        return;
                    }

                    Console.WriteLine("Input balance id for sender account");
                    string senderBankAccountId = MyReader.ReadString_WithCondition(
                        input => accountToRemoveBalance.BankAccounts.Count(a => a.Id == input) > 0 || input == "exit",
                        "Such account balance does not exist"
                    );
                    if (senderBankAccountId == "exit") return;
                    BankAccount senderbankAccount = accountToRemoveBalance.BankAccounts.FirstOrDefault(a => a.Id == senderBankAccountId);
                    try
                    {
                        accountToRemoveBalance.BankAccounts.Remove(senderbankAccount);
                        DataStorageManager.UpdateAccountsList(accounts);
                        Console.WriteLine("Bank account removed successfully.");
                    }
                    catch
                    {
                        Console.WriteLine("Error while creating bank account.");
                        return;
                    }
                }
            }
            Runner.RunNext(
                "Remove bank account",
                Remove
            );
        }

        private void DepositToBankAccount()
        {
            void Deposit()
            {
                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }
                Console.WriteLine("Accounts list");
                foreach (var account in accounts)
                {
                    Console.WriteLine($" - {account.UserName}");
                }
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Input account name to deposit money:");
                string accountName = MyReader.ReadString_WithCondition(
                    input => Validator.UserNameExists(input, accounts) || input == "exit",
                    "Such account was not found"
                );
                if (accountName == "exit") return;
                Account accountToDeposit = accounts.FirstOrDefault(a => a.UserName == accountName);
                Console.WriteLine(accountToDeposit.GetAccountFullInfo());
                Console.WriteLine("Input balance id to deposit on that bank account");
                string bankAccountId = MyReader.ReadString_WithCondition(
                    input => accountToDeposit.BankAccounts.Count(a => a.Id == input) > 0 || input == "exit",
                    "Such account balance does not exist"
                );
                if (bankAccountId == "exit") return;
                BankAccount bankAccountToDeposit = accountToDeposit.BankAccounts.FirstOrDefault(a => a.Id == bankAccountId);
                Console.WriteLine("Input deposit amount:");
                double depositAmount = MyReader.ReadDouble_WithCondition(
                    input => input > 0,
                    "Deposit amount must be a positive number."
                );
                if (depositAmount == 0) return;
                try
                {
                    bankAccountToDeposit.Balance += depositAmount;
                    DataStorageManager.UpdateAccountsList(accounts);
                    Console.WriteLine("Deposit successful.");
                }
                catch
                {
                    Console.WriteLine("Error while depositing money.");
                    return;
                }
            }
            Runner.RunNext(
                "Deposit money",
                Deposit
            );
        }

        private void WithDrawFromBankAccount()
        {
            void Withdraw()
            {
                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }
                Console.WriteLine("Accounts list");
                foreach (var account in accounts)
                {
                    Console.WriteLine($" - {account.UserName}");
                }
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Input account name to withdraw money:");
                string accountName = MyReader.ReadString_WithCondition(
                    input => Validator.UserNameExists(input, accounts) || input == "exit",
                    "Such account was not found"
                );
                if (accountName == "exit") return;

                Account accountToWithdraw = accounts.FirstOrDefault(a => a.UserName == accountName);
                Console.WriteLine(accountToWithdraw.GetAccountFullInfo());
                Console.WriteLine("Input balance id to withdraw from that bank account");
                string bankAccountId = MyReader.ReadString_WithCondition(
                    input => accountToWithdraw.BankAccounts.Count(a => a.Id == input) > 0 || input == "exit",
                    "Such account balance does not exist"
                );
                if (bankAccountId == "exit") return;
                BankAccount bankAccountToWithdraw = accountToWithdraw.BankAccounts.FirstOrDefault(a => a.Id == bankAccountId);
                Console.WriteLine("Input withdraw amount:");
                double withdrawAmount = MyReader.ReadDouble_WithCondition(
                    input => input > 0 && input <= bankAccountToWithdraw.Balance,
                    "Withdraw amount must be a positive number and less than or equal to the balance."
                );
                if (withdrawAmount == 0) return;
                try
                {
                    bankAccountToWithdraw.Balance -= withdrawAmount;
                    DataStorageManager.UpdateAccountsList(accounts);
                    Console.WriteLine("Withdraw successful.");
                }
                catch
                {
                    Console.WriteLine("Error while withdrawing money.");
                    return;
                }
            }

            Runner.RunNext(
                "Withdraw money",
                Withdraw
            );
        }

    }
}

/*
+ register account
- remove account
- veiw all accounts
- transact money
*/