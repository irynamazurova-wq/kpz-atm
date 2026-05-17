namespace ATMSimulator.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Card UserCard { get; set; }
        public Account UserAccount { get; set; }

        public User(string id, string firstName, string lastName, Card card, Account account)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserCard = card;
            UserAccount = account;
        }

        public string GetFullName() => $"{FirstName} {LastName}";
    }
}