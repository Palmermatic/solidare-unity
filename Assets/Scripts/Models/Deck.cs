﻿using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models
{
    public enum CardStackDirection { None, Horizontal, Vertical }

    public class Deck : MonoBehaviour
    {
        public List<Card> Cards = new List<Card>();

        public void NewDeck(IList<Card> cards = null)
        {
            if (cards != null)
            {
                Cards = cards.ToList();
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Deck"/> slot
        /// </summary>
        public void NewSlot(string name, string tag, CardStackDirection direction, int spacing = 0)
        {
            gameObject.name = name;
            gameObject.tag = tag;
            if (direction == CardStackDirection.Horizontal)
            {
                var hlg = gameObject.AddComponent<HorizontalLayoutGroup>();
                hlg.spacing = spacing;
                hlg.childControlWidth = false;
            }
            if (direction == CardStackDirection.Vertical)
            {
                var vlg = gameObject.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = spacing;
                vlg.childControlHeight = false;
            }
            NewDeck();
        }


        /// <summary>
        /// Draws a card from the top of the deck.
        /// </summary>
        /// <returns></returns>
        public Card Draw()
        {
            var card = Cards.Last();
            Cards.Remove(card);
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
