namespace ATMSimulator.Strategies
{
    public class RegularWithdrawStrategy : IWithdrawStrategy
    {
        public decimal CalculateFee(decimal amount) => amount * 0.01m;
    }
}