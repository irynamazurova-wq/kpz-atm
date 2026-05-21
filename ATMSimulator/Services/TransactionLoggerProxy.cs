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

            _realProcessor.ProcessWithdraw(user, amount);

            LogToFile($"SUCCESS WITHDRAW: {user.GetFullName()} - Balance: {user.UserAccount.Balance} UAH");
        }

        public void ProcessDeposit(User user, decimal amount)
        {
            LogToFile($"TRY DEPOSIT: {user.GetFullName()} - {amount} UAH");

            _realProcessor.ProcessDeposit(user, amount);

            LogToFile($"SUCCESS DEPOSIT: {user.GetFullName()} - Balance: {user.UserAccount.Balance} UAH");
        }

        public void ProcessTransfer(User sourceUser, User targetUser, decimal amount)
        {
            LogToFile($"TRY TRANSFER: From {sourceUser.GetFullName()} To {targetUser.GetFullName()} - {amount} UAH");

            _realProcessor.ProcessTransfer(sourceUser, targetUser, amount);

            LogToFile($"SUCCESS TRANSFER: From {sourceUser.GetFullName()} To {targetUser.GetFullName()}");
        }

        public void ProcessExchange(User user, string currency, decimal amount, decimal rate)
        {
            LogToFile($"TRY EXCHANGE: {user.GetFullName()} buy {amount} {currency} at rate {rate}");

            _realProcessor.ProcessExchange(user, currency, amount, rate);

            LogToFile($"SUCCESS EXCHANGE: {user.GetFullName()} bought {amount} {currency}");
        }

        private void LogToFile(string message)
        {
            File.AppendAllText(_logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
        }
    }
}