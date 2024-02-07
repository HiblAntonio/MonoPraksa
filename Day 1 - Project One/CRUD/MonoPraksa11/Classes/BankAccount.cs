using MonoPraksa11.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoPraksa11
{
    abstract class BankAccount : IBanking
    {
        private string Owner { get; set; }
        public float Balance { get; set; }
        public List<Transaction> Transactions;

        public BankAccount(float balance, string owner)
        {
            this.Balance = balance;
            this.Owner = owner;
            Transactions = new List<Transaction>();
        }

        public BankAccount(string owner)
        {
            this.Balance = 0;
            this.Owner = owner;
            Transactions = new List<Transaction>();
        }

        public void CheckBalance() { Console.WriteLine("Your current balance is: " + Balance); }

        public void CheckOwner() { Console.WriteLine(Owner); }

        public void DepositMoney(float amount)
        {
            RecordTransaction(amount, "Deposit");
            Balance += amount;
        }

        public virtual void WithdrawMoney(float amount)
        {
            RecordTransaction(amount, "Withdraw");
            Balance -= amount;
        }

        public abstract void ApplyIntrestRate();

        public void RecordTransaction(float amount, string description)
        {
            Transactions.Add(new Transaction(amount, description));
        }

        public void DisplayTransactionHistory()
        {
            int counter = 0;
            Transactions.ForEach(transaction => { Console.WriteLine(counter + ". " + transaction.Amount + " - " + transaction.Description);
                                                  counter++;  });
        }

        public string FindTransaction(int transactionNumber)
        {
            return Transactions[transactionNumber].Amount + " - " + Transactions[transactionNumber].Description;
        }
    }
}
