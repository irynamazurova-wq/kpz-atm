using ATMSimulator.Services;

namespace ATMSimulator.States
{
    public class WaitingForPinState : IAtmState
    {
        private int _pinAttempts = 0;
        private const int MaxAttempts = 3;

        public void InsertCard(AtmService atm, string cardNumber) => atm.TriggerNotification("Card already inside");

        public void EnterPin(AtmService atm, string pin)
        {
            if (atm.CurrentUser == null) return;

            if (atm.CurrentCard.PinHash == pin)
            {
                atm.TriggerNotification("PIN correct");
                atm.ChangeState(new AuthorizedState());
            }
            else
            {
                _pinAttempts++;
                int remaining = MaxAttempts - _pinAttempts;
                if (remaining <= 0)
                {
                    atm.CurrentCard.IsBlocked = true;
                    atm.CommitTransaction();
                    atm.TriggerNotification("Card blocked");
                    atm.SetCurrentUser(null);
                    atm.ChangeState(new NoCardState());
                }
                else
                {
                    atm.TriggerNotification($"Wrong PIN. Remaining: {remaining}");
                }
            }
        }

        public void Withdraw(AtmService atm, decimal amount) => atm.TriggerNotification("Enter PIN first");
        public void Deposit(AtmService atm, decimal amount) => atm.TriggerNotification("Enter PIN first");

        public void EjectCard(AtmService atm)
        {
            atm.TriggerNotification("Card ejected");
            atm.SetCurrentUser(null);
            atm.ChangeState(new NoCardState());
        }
    }
}