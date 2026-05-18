using System;
using System.Text.Json.Serialization;

namespace ATMSimulator.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }

        [JsonConstructor]
        public Account() { }

        public Account(string accountNumber, decimal initialBalance, string currency = "UAH")
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
            Currency = currency;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException();
            
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException();
            
            if (Balance < amount)
                throw new InvalidOperationException();

            Balance -= amount;
        }
    }
}