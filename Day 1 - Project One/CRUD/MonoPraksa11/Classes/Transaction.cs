using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoPraksa11.Classes
{
    internal class Transaction
    {
        public DateTime Date { get;  }
        public string Description { get; }
        public float Amount { get; }

        public Transaction(float amount, string description)
        {
            this.Date = DateTime.Now;
            this.Amount = amount;
            this.Description = description;
        }
    }
}
