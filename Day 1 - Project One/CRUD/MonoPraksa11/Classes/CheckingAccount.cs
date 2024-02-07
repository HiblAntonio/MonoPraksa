using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoPraksa11.Classes
{
    internal class CheckingAccount : BankAccount
    {
        private float intrestRate = 1.04f;
        public CheckingAccount(string owner) : base(owner) { }

        public override void ApplyIntrestRate()
        {
            Balance *= intrestRate;
        }
    }
}
