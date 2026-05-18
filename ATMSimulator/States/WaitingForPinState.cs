using ATMSimulator.Services;

namespace ATMSimulator.States
{
    public class WaitingForPinState : IAtmState
    {
        private int _pinAttempts = 0;
        private const int MaxAttempts = 3;

        public void InsertCard(AtmService atm, string cardNumber) => atm.TriggerNotification("Картка вже всередині");

        public void EnterPin(AtmService atm, string pin)
        {
            if (atm.CurrentUser == null) return;

            if (atm.CurrentCard.PinHash == pin)
            {
                atm.TriggerNotification("ПІН-код правильний");
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
                    atm.TriggerNotification("Картку заблоковано примусово");
                    atm.SetCurrentUser(null);
                    atm.ChangeState(new NoCardState());
                }
                else
                {
                    atm.TriggerNotification($"Невірний ПІН. Залишилось спроб: {remaining}");
                }
            }
        }

        public void Withdraw(AtmService atm, decimal amount) => atm.TriggerNotification("Спочатку введіть ПІН");
        public void Deposit(AtmService atm, decimal amount) => atm.TriggerNotification("Спочатку введіть ПІН");
        public void Transfer(AtmService atm, string targetCardNumber, decimal amount) => atm.TriggerNotification("Спочатку авторизуйтесь");
        public void BuyCurrency(AtmService atm, string currency, decimal amount, decimal rate) => atm.TriggerNotification("Спочатку авторизуйтесь");

        public void EjectCard(AtmService atm)
        {
            atm.TriggerNotification("Картку повернуто");
            atm.SetCurrentUser(null);
            atm.ChangeState(new NoCardState());
        }
    }
}