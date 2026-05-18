using ATMSimulator.Models;

namespace ATMSimulator.Factories
{
    public static class UserFactory
    {
        public static User CreateUser(string id, string firstName, string lastName, string cardNumber, string pin, decimal balance, string cardType)
        {
            return new User(id, firstName, lastName, new Card(cardNumber, pin, cardType), new Account("UA" + id, balance));
        }
    }
}