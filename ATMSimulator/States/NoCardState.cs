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
                atm.TriggerNotification("Картку не знайдено");
                return;
            }
            if (user.UserCard.IsBlocked)
            {
                atm.TriggerNotification("Картку заблоковано");
                return;
            }
            atm.SetCurrentUser(user);
            atm.TriggerNotification("Картку зчитано. Введіть ПІН-код");
            atm.ChangeState(new WaitingForPinState());
        }

        public void EnterPin(AtmService atm, string pin) => atm.TriggerNotification("Спочатку вставте картку");
        public void Withdraw(AtmService atm, decimal amount) => atm.TriggerNotification("Спочатку вставте картку");
        public void Deposit(AtmService atm, decimal amount) => atm.TriggerNotification("Спочатку вставте картку");
        public void EjectCard(AtmService atm) => atm.TriggerNotification("У банкоматі немає картки");
    }
}