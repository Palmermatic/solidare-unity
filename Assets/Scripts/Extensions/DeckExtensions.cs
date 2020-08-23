using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Extensions
{
    public static class DeckExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Unwraps the new deck (adds all 52 cards in order)
        /// </summary>
        /// <param name="drawPile"></param>
        public static void Unwrap(this List<Card> drawPile)
        {
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Face f in Enum.GetValues(typeof(Face)))
                {
                    drawPile.Add(new Card(s, f));
                }
            }
        }

        /// <summary>
        /// Parses a string like "Ace of spades" into a <see cref="Card"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Card ToCard(this string name)
        {
            var names = name.Split(' ');
            var face = names[0];
            var suit = names[2];
            Suit s = (Suit)Enum.Parse(typeof(Suit), suit);
            Face f = (Face)Enum.Parse(typeof(Face), face);
            return new Card(s, f);
        }
    }
}
