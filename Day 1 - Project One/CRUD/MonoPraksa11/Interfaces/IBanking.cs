using MonoPraksa11.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoPraksa11
{
    internal interface IBanking
    {
        string FindTransaction(int transactionNumber);
        void DepositMoney(float value);
        void WithdrawMoney(float value);
        void ApplyIntrestRate();
        void CheckBalance();
        void CheckOwner();
        void DisplayTransactionHistory();
    }
}
