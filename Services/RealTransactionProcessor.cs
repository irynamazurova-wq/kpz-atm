using System;
using ATMSimulator.Interfaces;
using ATMSimulator.Models;

namespace ATMSimulator.Services
{
    public class RealTransactionProcessor : ITransactionProcessor
    {
        public void ProcessWithdraw(User user, decimal amount)
        {
            user.UserAccount.Withdraw(amount);
        }

        public void ProcessDeposit(User user, decimal amount)
        {
            user.UserAccount.Deposit(amount);
        }
    }
}