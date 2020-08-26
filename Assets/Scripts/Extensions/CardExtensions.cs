using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System;
using System.Linq;

namespace Assets.Scripts.Extensions
{
    public static class CardExtensions
    {
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

        public static void PickUp(this Card card)
        {
            GameManager.Instance.Tableau.Select(t => t.Cards).Single(column => column.Contains(card)).Remove(card);
        }
    }
}
