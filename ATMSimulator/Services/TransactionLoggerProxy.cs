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
            LogToFile($"TRY WITHDRAW: {user.GetFullName()} - {amount} UAH");
            try
            {
                _realProcessor.ProcessWithdraw(user, amount);
                LogToFile($"SUCCESS WITHDRAW: {user.GetFullName()} - Balance: {user.UserAccount.Balance} UAH");
            }
            catch (Exception ex)
            {
                LogToFile($"FAIL WITHDRAW: {user.GetFullName()} - Error: {ex.Message}");
                throw;
            }
        }

        public void ProcessDeposit(User user, decimal amount)
        {
            LogToFile($"TRY DEPOSIT: {user.GetFullName()} - {amount} UAH");
            try
            {
                _realProcessor.ProcessDeposit(user, amount);
                LogToFile($"SUCCESS DEPOSIT: {user.GetFullName()} - Balance: {user.UserAccount.Balance} UAH");
            }
            catch (Exception ex)
            {
                LogToFile($"FAIL DEPOSIT: {user.GetFullName()} - Error: {ex.Message}");
                throw;
            }
        }

        public void ProcessTransfer(User sourceUser, User targetUser, decimal amount)
        {
            LogToFile($"TRY TRANSFER: From {sourceUser.GetFullName()} To {targetUser.GetFullName()} - {amount} UAH");
            try
            {
                _realProcessor.ProcessTransfer(sourceUser, targetUser, amount);
                LogToFile($"SUCCESS TRANSFER: From {sourceUser.GetFullName()} To {targetUser.GetFullName()}");
            }
            catch (Exception ex)
            {
                LogToFile($"FAIL TRANSFER: From {sourceUser.GetFullName()} - Error: {ex.Message}");
                throw;
            }
        }

        public void ProcessExchange(User user, string currency, decimal amount, decimal rate)
        {
            LogToFile($"TRY EXCHANGE: {user.GetFullName()} buy {amount} {currency} at rate {rate}");
            try
            {
                _realProcessor.ProcessExchange(user, currency, amount, rate);
                LogToFile($"SUCCESS EXCHANGE: {user.GetFullName()} bought {amount} {currency}");
            }
            catch (Exception ex)
            {
                LogToFile($"FAIL EXCHANGE: {user.GetFullName()} - Error: {ex.Message}");
                throw;
            }
        }

        private void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
            }
            catch
            {
            }
        }
    }
}