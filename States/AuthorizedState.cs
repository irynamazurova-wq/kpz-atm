using System;
using ATMSimulator.Interfaces;
using ATMSimulator.Services;

namespace ATMSimulator.States
{
    public class AuthorizedState : IAtmState
    {
        public void InsertCard(AtmService atm, string cardNumber) => 
            atm.TriggerNotification("У системі вже є активна картка.");

        public void EnterPin(AtmService atm, string pin) => 
            atm.TriggerNotification("Ви вже успішно авторизовані в системі.");

        public void Withdraw(AtmService atm, decimal amount)
        {
            try
            {
                atm.CurrentUser.UserAccount.Withdraw(amount);
                atm.CommitTransaction(); 
                atm.TriggerNotification($"Успішно знято {amount} UAH. Новий баланс: {atm.CurrentUser.UserAccount.Balance} UAH");
            }
            catch (Exception ex)
            {
                atm.TriggerNotification($"Помилка транзакції: {ex.Message}");
            }
        }

        public void Deposit(AtmService atm, decimal amount)
        {
            try
            {
                atm.CurrentUser.UserAccount.Deposit(amount);
                atm.CommitTransaction(); 
                atm.TriggerNotification($"Рахунок поповнено на {amount} UAH. Новий баланс: {atm.CurrentUser.UserAccount.Balance} UAH");
            }
            catch (Exception ex)
            {
                atm.TriggerNotification($"Помилка транзакції: {ex.Message}");
            }
        }

        public void EjectCard(AtmService atm)
        {
            atm.TriggerNotification($"Дякуємо за роботу, {atm.CurrentUser.FirstName}. Сесію завершено. Заберіть картку.");
            atm.SetCurrentUser(null);
            atm.ChangeState(new NoCardState());
        }
    }
}