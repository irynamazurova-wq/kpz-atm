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

        public void ProcessTransfer(User sourceUser, User targetUser, decimal amount)
        {
            sourceUser.UserAccount.Withdraw(amount);
            targetUser.UserAccount.Deposit(amount);
        }

        public void ProcessExchange(User user, string currency, decimal amount, decimal rate)
        {
            decimal cost = amount * rate;
            user.UserAccount.Withdraw(cost);

            if (currency == "USD")
            {
                user.UserAccount.UsdBalance += amount;
            }
            else if (currency == "EUR")
            {
                user.UserAccount.EurBalance += amount;
            }
        }
    }
}