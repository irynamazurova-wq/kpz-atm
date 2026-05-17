using ATMSimulator.Interfaces;
using ATMSimulator.Services;

namespace ATMSimulator.States
{
    public class WaitingForPinState : IAtmState
    {
        private int _pinAttempts = 0;
        private const int MaxAttempts = 3;

        public void InsertCard(AtmService atm, string cardNumber) => 
            atm.TriggerNotification("Картка вже знаходиться всередині банкомата.");

        public void EnterPin(AtmService atm, string pin)
        {
            if (atm.CurrentUser == null) return;

            if (atm.CurrentCard.PinHash == pin)
            {
                atm.TriggerNotification($"ПІН-код правильний. Вітаємо, {atm.CurrentUser.GetFullName()}!");
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
                    atm.TriggerNotification("Картку заблоковано через перевищення кількості спроб введення ПІН-коду!");
                    atm.SetCurrentUser(null);
                    atm.ChangeState(new NoCardState());
                }
                else
                {
                    atm.TriggerNotification($"Неправильний ПІН-код! Залишилось спроб: {remaining}");
                }
            }
        }

        public void Withdraw(AtmService atm, decimal amount) => 
            atm.TriggerNotification("Спочатку введіть правильний ПІН-код для авторизації.");

        public void Deposit(AtmService atm, decimal amount) => 
            atm.TriggerNotification("Спочатку введіть правильний ПІН-код для авторизації.");

        public void EjectCard(AtmService atm)
        {
            atm.TriggerNotification("Картку успішно повернуто користувачу.");
            atm.SetCurrentUser(null);
            atm.ChangeState(new NoCardState());
        }
    }
}