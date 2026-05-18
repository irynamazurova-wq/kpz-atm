using ATMSimulator.Models;

namespace ATMSimulator.Interfaces
{
    public interface ITransactionProcessor
    {
        void ProcessWithdraw(User user, decimal amount);
        void ProcessDeposit(User user, decimal amount);
        void ProcessTransfer(User sourceUser, User targetUser, decimal amount);
        void ProcessExchange(User user, string currency, decimal amount, decimal rate);
    }
}