using ATMSimulator.Models;

namespace ATMSimulator.Interfaces
{
    public interface ITransactionProcessor
    {
        void ProcessWithdraw(User user, decimal amount);
        void ProcessDeposit(User user, decimal amount);
    }
}