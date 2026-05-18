using System;
using ATMSimulator.Interfaces;
using ATMSimulator.Services;

namespace ATMSimulator.States
{
    public class AuthorizedState : IAtmState
    {
        private readonly ITransactionProcessor _processor = new TransactionLoggerProxy(new RealTransactionProcessor());

        public void InsertCard(AtmService atm, string cardNumber) => atm.TriggerNotification("Card already inside");
        public void EnterPin(AtmService atm, string pin) => atm.TriggerNotification("Already authorized");

        public void Withdraw(AtmService atm, decimal amount)
        {
            try
            {
                _processor.ProcessWithdraw(atm.CurrentUser, amount);
                atm.CommitTransaction();
                atm.TriggerNotification($"Withdrawn {amount} UAH");
            }
            catch (Exception ex)
            {
                atm.TriggerNotification(ex.Message);
            }
        }

        public void Deposit(AtmService atm, decimal amount)
        {
            try
            {
                _processor.ProcessDeposit(atm.CurrentUser, amount);
                atm.CommitTransaction();
                atm.TriggerNotification($"Deposited {amount} UAH");
            }
            catch (Exception ex)
            {
                atm.TriggerNotification(ex.Message);
            }
        }

        public void EjectCard(AtmService atm)
        {
            atm.TriggerNotification("Session ended. Take card");
            atm.SetCurrentUser(null);
            atm.ChangeState(new NoCardState());
        }
    }
}