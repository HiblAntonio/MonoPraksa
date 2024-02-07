using MonoPraksa11.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoPraksa11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool incorrectSpelling = false;
            int userMenuInput = 0;
            int transactionNumber = 0;
            float moneyAmount = 0f;

            Console.WriteLine("What is your name?");
            string name = Console.ReadLine();

            // Creating an instance of a class that implement IBanking interface
            IBanking usersAccount;

            // Checking if the user has enter a correct input
            // checking or savings account
            do
            {
                Console.WriteLine("Which account would you like to create?");
                Console.WriteLine("(Type \"Checking\" for checking account or \"Savings\" for savings account.)");
                string userInputAccountType = Console.ReadLine().ToLower();

                if (userInputAccountType == "checking")
                {
                    usersAccount = new CheckingAccount(name);
                    incorrectSpelling = false;
                }
                else if (userInputAccountType == "savings")
                {
                    Console.WriteLine("Amount of money you will deposit into your account: ");
                    float amount = float.Parse(Console.ReadLine());

                    if (amount < 0)
                        amount = 0f;

                    usersAccount = new SavingsAccount(amount, name);
                    incorrectSpelling = false;
                }
                else
                {
                    usersAccount = null;
                    Console.WriteLine("Invalid input!");
                    incorrectSpelling = true;
                }
            } while (incorrectSpelling);

            do
            {
                Console.WriteLine();
                Console.WriteLine("------- Menu -------");
                Console.WriteLine("1. Add a deposit");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Check balance");
                Console.WriteLine("4. Check owner");
                Console.WriteLine("5. Apply intrest rate");
                Console.WriteLine("6. Show transaction history");
                Console.WriteLine("7. Find a transaction");
                userMenuInput = Convert.ToInt32(Console.ReadLine());

                switch (userMenuInput)
                {
                    case 1:
                        moneyAmount = ReturnMoneyAmount();
                        usersAccount.DepositMoney(moneyAmount);
                        break;
                    case 2:
                        moneyAmount = ReturnMoneyAmount();
                        usersAccount.WithdrawMoney(moneyAmount);
                        break;
                    case 3:
                        usersAccount.CheckBalance();
                        break;
                    case 4:
                        usersAccount.CheckOwner();
                        break;
                    case 5:
                        usersAccount.ApplyIntrestRate();
                        break;
                    case 6:
                        usersAccount.DisplayTransactionHistory();
                        break;
                    case 7:
                        Console.WriteLine("Enter a transaction number: ");
                        transactionNumber = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(usersAccount.FindTransaction(transactionNumber));
                        break;
                    default:
                        Console.WriteLine("Wrong input!");
                        break;
                }

                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                Console.Clear();
            } while(true);
        }

        private static float ReturnMoneyAmount()
        {
            // Should a function have a Console.WriteLine in it or everything should be in main?
            Console.WriteLine("Type an amount: ");
            return float.Parse(Console.ReadLine());
        }
    }
}
