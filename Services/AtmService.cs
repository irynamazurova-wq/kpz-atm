using System;
using System.Collections.Generic;
using System.Linq;
using ATMSimulator.Interfaces;
using ATMSimulator.Models;

namespace ATMSimulator.Services
{
    public class AtmService
    {
        private readonly IDataStorage _storage;
        private readonly List<User> _users;

        public IAtmState CurrentState { get; private set; }
        public User CurrentUser { get; private set; }
        public Card CurrentCard => CurrentUser?.UserCard;

        public event Action<string> OnMessageDisplayed;
        public event Action<string> OnScreenUpdated;

        public AtmService(IDataStorage storage)
        {
            _storage = storage;
            _users = _storage.LoadUsers();
            
        }

        public void ChangeState(IAtmState newState)
        {
            CurrentState = newState;
            UpdateScreen();
        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public User FindUserByCard(string cardNumber)
        {
            return _users.FirstOrDefault(u => u.UserCard?.CardNumber == cardNumber);
        }

        public void CommitTransaction()
        {
            _storage.SaveUsers(_users);
        }

        public void TriggerNotification(string message)
        {
            OnMessageDisplayed?.Invoke(message);
        }

        public void UpdateScreen()
        {
            string stateName = CurrentState?.GetType().Name ?? "Невідомий стан";
            OnScreenUpdated?.Invoke(stateName);
        }

        public void InsertCard(string cardNumber) => CurrentState?.InsertCard(this, cardNumber);
        public void EnterPin(string pin) => CurrentState?.EnterPin(this, pin);
        public void Withdraw(decimal amount) => CurrentState?.Withdraw(this, amount);
        public void Deposit(decimal amount) => CurrentState?.Deposit(this, amount);
        public void EjectCard() => CurrentState?.EjectCard(this);
    }
}