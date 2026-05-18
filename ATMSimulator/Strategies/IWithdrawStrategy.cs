namespace ATMSimulator.Strategies
{
    public interface IWithdrawStrategy
    {
        decimal CalculateFee(decimal amount);
    }
}