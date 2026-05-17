using System;

namespace ATMSimulator.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }

        public Account(string accountNumber, decimal initialBalance, string currency = "UAH")
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
            Currency = currency;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума поповнення має бути більшою за нуль.");
            
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума зняття має бути більшою за нуль.");
            
            if (Balance < amount)
                throw new InvalidOperationException("Недостатньо коштів на рахунку.");

            Balance -= amount;
        }
    }
}