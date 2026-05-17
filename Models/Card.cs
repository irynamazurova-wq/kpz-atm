namespace ATMSimulator.Models
{
    public class Card
    {
        public string CardNumber { get; set; }
        public string PinHash { get; set; } // Зберігаємо хеш, а не чистий ПІН
        public bool IsBlocked { get; set; }

        public Card(string cardNumber, string pinHash)
        {
            CardNumber = cardNumber;
            PinHash = pinHash;
            IsBlocked = false;
        }
    }
}