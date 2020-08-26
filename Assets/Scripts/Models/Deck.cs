using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Deck
    {
        public List<Card> Cards { get; set; } = new List<Card>();

        public Deck(IList<Card> cards)
        {
            Cards = cards.ToList();
        }

        public Deck() { }

        /// <summary>
        /// It's an empty deck.
        /// </summary>
        public static Deck Empty = new Deck();

        /// <summary>
        /// Returns the given card and all cards on top of it.
        /// </summary>
        /// <param name="anchor"></param>
        /// <returns></returns>
        public Deck PickUpFrom(Card anchor)
        {
            var index = Cards.FindIndex(card => card.Equals(anchor));
            var cards = Cards.GetRange(0, index);
            Cards.RemoveRange(0, index);
            return new Deck(cards);
        }

        /// <summary>
        /// Draws a card from the top of the deck.
        /// </summary>
        /// <returns></returns>
        public Card Draw()
        {
            var card = Cards.First();
            Cards.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Adds a card to the top of the deck.
        /// </summary>
        /// <param name="card"></param>
        public void Add(Card card)
        {
            Cards.Add(card);
        }
    }
}
