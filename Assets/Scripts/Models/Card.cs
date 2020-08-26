using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public enum Suit { Clubs, Diamonds, Hearts, Spades }
    public enum Face { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    public class Card : MonoBehaviour
    {

        public Suit Suit;
        public Face Face;
        public bool IsFaceUp;
        public Sprite Image;

        public static Sprite CardFace;
        public static Sprite CardBack = Resources.Load<Sprite>("cardback");


        public bool HasBeenFlipped { get; set; }

        public Card(Suit suit, Face face, bool faceUp = false)
        {
            IsFaceUp = faceUp;
            Suit = suit;
            Face = face;

            CardFace = Resources.Load<Sprite>($"{face}_of_{suit}");
            Image = faceUp ? CardFace : CardBack;
        }

        private void Start()
        {

        }

        private void Awake()
        {

        }

        private void Update()
        {
            if (HasBeenFlipped)
            {
                Image = IsFaceUp ? CardFace : CardBack;
            }
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