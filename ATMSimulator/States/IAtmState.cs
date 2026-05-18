using ATMSimulator.Services;

namespace ATMSimulator.States
{
    public interface IAtmState
    {
        void InsertCard(AtmService atm, string cardNumber);
        void EnterPin(AtmService atm, string pin);
        void Withdraw(AtmService atm, decimal amount);
        void Deposit(AtmService atm, decimal amount);
        void EjectCard(AtmService atm);
    }
}