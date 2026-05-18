using ATMSimulator.Services;

namespace ATMSimulator.States
{
    public class NoCardState : IAtmState
    {
        public void InsertCard(AtmService atm, string cardNumber)
        {
            var user = atm.FindUserByCard(cardNumber);
            if (user == null)
            {
                atm.TriggerNotification("Card not found");
                return;
            }
            if (user.UserCard.IsBlocked)
            {
                atm.TriggerNotification("Card is blocked");
                return;
            }
            atm.SetCurrentUser(user);
            atm.TriggerNotification("Card inserted. Enter PIN");
            atm.ChangeState(new WaitingForPinState());
        }

        public void EnterPin(AtmService atm, string pin) => atm.TriggerNotification("Insert card first");
        public void Withdraw(AtmService atm, decimal amount) => atm.TriggerNotification("Insert card first");
        public void Deposit(AtmService atm, decimal amount) => atm.TriggerNotification("Insert card first");
        public void EjectCard(AtmService atm) => atm.TriggerNotification("No card in ATM");
    }
}