using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum Suit { Clubs, Diamonds, Hearts, Spades }
public enum Face { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

public class Card
{
    public Suit Suit;
    public Face Face;
    public bool IsFaceUp;
    public Sprite Image;

    public Card(Suit suit, Face face, bool faceUp = false)
    {
        IsFaceUp = faceUp;
        Suit = suit;
        Face = face;

        Image = Resources.Load<Sprite>($"{face}_of_{suit}");
    }

    public override string ToString()
    {
        return Face + " of " + Suit;
    }
}
