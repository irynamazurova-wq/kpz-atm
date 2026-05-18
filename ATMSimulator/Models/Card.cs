using System.Text.Json.Serialization;

namespace ATMSimulator.Models
{
    public class Card
    {
        public string CardNumber { get; set; }
        public string PinHash { get; set; }
        public bool IsBlocked { get; set; }
        public string CardType { get; set; }

        [JsonConstructor]
        public Card() { }

        public Card(string cardNumber, string pinHash, string cardType = "Regular")
        {
            CardNumber = cardNumber;
            PinHash = pinHash;
            IsBlocked = false;
            CardType = cardType;
        }
    }
}