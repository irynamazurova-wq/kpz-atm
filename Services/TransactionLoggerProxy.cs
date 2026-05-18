using System;
using System.IO;
using ATMSimulator.Interfaces;
using ATMSimulator.Models;

namespace ATMSimulator.Services
{
    public class TransactionLoggerProxy : ITransactionProcessor
    {
        private readonly ITransactionProcessor _realProcessor;
        private readonly string _logFilePath = "transactions_log.txt";

        public TransactionLoggerProxy(ITransactionProcessor realProcessor)
        {
            _realProcessor = realProcessor;
        }

        public void ProcessWithdraw(User user, decimal amount)
        {
            LogToFile($"Користувач {user.GetFullName()} (Рахунок: {user.UserAccount.AccountNumber}) запит на {amount} UAH.");

            try
            {
                _realProcessor.ProcessWithdraw(user, amount);

                LogToFile($"Знято {amount} UAH. Новий баланс: {user.UserAccount.Balance} UAH.");
            }
            catch (Exception ex)
            {

                LogToFile($"Помилка зняття суми {amount} UAH. Причина: {ex.Message}");
                throw; 
            }
        }

        public void ProcessDeposit(User user, decimal amount)
        {
            LogToFile($"Користувач {user.GetFullName()} на суму {amount} UAH.");

            try
            {
                _realProcessor.ProcessDeposit(user, amount);
                LogToFile($"Рахунок поповнено на {amount} UAH. Новий баланс: {user.UserAccount.Balance} UAH.");
            }
            catch (Exception ex)
            {
                LogToFile($"Помилка поповнення на суму {amount} UAH. Причина: {ex.Message}");
                throw;
            }
        }

        private void LogToFile(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(_logFilePath, logEntry);
            }
            catch (Exception)
            {

            }
        }
    }
}