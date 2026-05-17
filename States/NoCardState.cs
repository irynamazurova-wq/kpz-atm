using System;
using ATMSimulator.Interfaces;
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
                atm.TriggerNotification("Помилка: Картку не знайдено в системі або вона недійсна.");
                return;
            }

            if (user.UserCard.IsBlocked)
            {
                atm.TriggerNotification("Помилка: Цю картку заблоковано банком!");
                return;
            }

            atm.SetCurrentUser(user);
            atm.TriggerNotification("Картку успішно зчитано. Будь ласка, введіть ПІН-код.");
            atm.ChangeState(new WaitingForPinState());
        }

        public void EnterPin(AtmService atm, string pin) => 
            atm.TriggerNotification("Спочатку вставте банківську картку!");

        public void Withdraw(AtmService atm, decimal amount) => 
            atm.TriggerNotification("Операція неможлива. Картку не вставлено.");

        public void Deposit(AtmService atm, decimal amount) => 
            atm.TriggerNotification("Операція неможлива. Картку не вставлено.");

        public void EjectCard(AtmService atm) => 
            atm.TriggerNotification("У банкоматі немає картки.");
    }
}