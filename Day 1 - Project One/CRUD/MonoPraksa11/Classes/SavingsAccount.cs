using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoPraksa11.Classes
{
    internal class SavingsAccount : BankAccount
    {
        private float intrestRate = 1.07f;
        public SavingsAccount(float balance, string owner) : base(balance, owner) { }

        public override void WithdrawMoney(float amount)
        {
            if ((Balance - amount) < 0)
                Console.WriteLine("Not enough funds!");
            else
            {
                Balance -= amount;
                RecordTransaction(amount, "Withdraw");
            }
        }

        public override void ApplyIntrestRate()
        {
            Balance *= intrestRate;
        }
    }
}
