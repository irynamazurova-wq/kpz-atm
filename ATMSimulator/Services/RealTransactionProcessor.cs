using ATMSimulator.Interfaces;
using ATMSimulator.Models;
using ATMSimulator.Strategies;

namespace ATMSimulator.Services
{
    public class RealTransactionProcessor : ITransactionProcessor
    {
        public void ProcessWithdraw(User user, decimal amount)
        {
            IWithdrawStrategy strategy = user.UserCard.CardType == "VIP" 
                ? new VipWithdrawStrategy() 
                : new RegularWithdrawStrategy();

            decimal fee = strategy.CalculateFee(amount);
            user.UserAccount.Withdraw(amount + fee);
        }

        public void ProcessDeposit(User user, decimal amount)
        {
            user.UserAccount.Deposit(amount);
        }
    }
}