using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Models
{
    public enum Color { Black, Red }
    public enum Suit { Clubs, Diamonds, Hearts, Spades }
    public enum Face { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    [Serializable]
    public class Card : MonoBehaviour
    {
        public Color Color;
        public Suit Suit;
        public Face Face;

        public bool IsFaceUp;

        public Sprite CardFace;
        public Sprite CardBack;

        /// <summary>
        /// Initializes a new card.
        /// </summary>
        public void NewCard(Suit suit, Face face, bool faceUp = false)
        {
            IsFaceUp = faceUp;
            Suit = suit;
            Face = face;
            Color = suit == Suit.Clubs || suit == Suit.Spades ? Color.Black : Color.Red;

            CardFace = Resources.Load<Sprite>($"{face}_of_{suit}");
        }

        public override string ToString()
        {
            return Face + " of " + Suit;
        }

        public override bool Equals(object other)
        {
            Card card = other as Card;
            return card.Suit == Suit && card.Face == Face;
        }

        public override int GetHashCode()
        {
            return (int)Suit * (int)Face;
        }
    }
}