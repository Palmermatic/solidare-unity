using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Extensions
{
    public static class DeckExtensions
    {
        private static Random rng = new Random();

        /// <summary>
        /// Randomizes the order of cards in the deck.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deck"></param>
        public static void Shuffle(this Deck deck)
        {
            int n = deck.Cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = deck.Cards[k];
                deck.Cards[k] = deck.Cards[n];
                deck.Cards[n] = value;
            }
        }

        /// <summary>
        /// Unwraps a new deck (adds all cards in order)
        /// </summary>
        /// <param name="drawPile"></param>
        public static void Unwrap(this Deck drawPile)
        {
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Face f in Enum.GetValues(typeof(Face)))
                {
                    drawPile.Add(new Card(s, f));
                }
            }
        }
    }
}
