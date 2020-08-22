using Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Solitaire : MonoBehaviour
{
    // editor-hardcoded
    public GameObject CardPrefab;
    public GameObject deckButton;
    public GameObject[] bottomPos;
    public GameObject[] topPos;

    private List<Card> Deck;
    private List<Card> DiscardPile = new List<Card>();

    public List<Card>[] bottoms;
    public List<Card>[] tops;
    public List<Card> tripsOnDisplay = new List<Card>();
    public List<List<Card>> deckTrips = new List<List<Card>>();

    private int trips;
    private int tripsRemainder;
    private int deckLocation;

    // fucking stupid
    private List<Card> bottom0 = new List<Card>();
    private List<Card> bottom1 = new List<Card>();
    private List<Card> bottom2 = new List<Card>();
    private List<Card> bottom3 = new List<Card>();
    private List<Card> bottom4 = new List<Card>();
    private List<Card> bottom5 = new List<Card>();
    private List<Card> bottom6 = new List<Card>();



    // Start is called before the first frame update
    void Start()
    {
        bottoms = new List<Card>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        DealCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DealCards()
    {
        Deck = new List<Card>();

        Deck.Unwrap();

        Deck.Shuffle();

        SolitaireSort();

        StartCoroutine(Deal());

        SortDeckIntoTrips();
    }


    IEnumerator Deal()
    {
        for (int i = 0; i < 7; i++)
        {
            float yOffset = 0;
            float zOffset = 0.03f;

            foreach (var card in bottoms[i])
            {
                yield return new WaitForSeconds(0.13f);
                GameObject newCard = Instantiate(
                    CardPrefab,
                    new Vector3(
                        bottomPos[i].transform.position.x,
                        bottomPos[i].transform.position.y - yOffset,
                        bottomPos[i].transform.position.z - zOffset
                    ),
                    Quaternion.identity
                );
                newCard.name = card.ToString();
                newCard.tag = "Card";

                newCard.GetComponent<Selectable>().IsFaceUp = card == bottoms[i][bottoms[i].Count - 1];
                newCard.GetComponent<UpdateSprite>().cardFace = card.Sprite;

                yOffset += 0.3f;
                zOffset += 0.03f;
                DiscardPile.Add(card);
            }
        }

        foreach (var card in DiscardPile)
        {
            if (Deck.Any(unDrawn => unDrawn.Suit == card.Suit && unDrawn.Face == card.Face))
            {
                Deck.Remove(card);
            }
        }
        DiscardPile.Clear();
    }

    void SolitaireSort()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = i; j < 7; j++)
            {
                bottoms[j].Add(Deck.Last());
                Deck.RemoveAt(Deck.Count - 1);
            }
        }
    }

    public void SortDeckIntoTrips()
    {
        trips = Deck.Count / 3;
        tripsRemainder = Deck.Count % 3;
        deckTrips.Clear();

        int modifier = 0;
        for (int i = 0; i < trips; i++)
        {
            var myTrips = new List<Card>();
            for (int j = 0; j < 3; j++)
            {
                myTrips.Add(Deck[j + modifier]);
            }
            deckTrips.Add(myTrips);
            modifier += 3;
        }
        if (tripsRemainder != 0)
        {
            var myRemainders = new List<Card>();
            modifier = 0;
            for (int k = 0; k < tripsRemainder; k++)
            {
                myRemainders.Add(Deck[Deck.Count - tripsRemainder + modifier]);
                modifier++;
            }
            deckTrips.Add(myRemainders);
            trips++;
        }
        deckLocation = 0;
    }

    public void DrawThree()
    {
        // add remaining cards to discard pile
        foreach (Transform child in deckButton.transform)
        {
            if (child.CompareTag("Card"))
            {
                Deck.Remove(child.name.ToCard());
                DiscardPile.Add(child.name.ToCard());
                Destroy(child.gameObject);
            }
        }

        if (deckLocation < trips)
        {
            // draw 3
            tripsOnDisplay.Clear();
            float xOffset = 2.5f;
            float zOffset = -0.2f;

            foreach (var card in deckTrips[deckLocation])
            {
                GameObject newTopCard = Instantiate(
                    CardPrefab, new Vector3(
                        deckButton.transform.position.x + xOffset,
                        deckButton.transform.position.y,
                        deckButton.transform.position.z + zOffset),
                    Quaternion.identity
                );
                xOffset += 0.5f;
                zOffset -= 0.2f;
                newTopCard.name = card.ToString();
                newTopCard.GetComponent<UpdateSprite>().cardFace = card.Sprite;
                tripsOnDisplay.Add(card);
                newTopCard.GetComponent<Selectable>().IsFaceUp = true;
            }
            deckLocation++;
        }
        else
        {
            RestackTopDeck();
        }
    }

    private void RestackTopDeck()
    {
        foreach (var card in DiscardPile)
        {
            Deck.Add(card);
        }
        DiscardPile.Clear();
        SortDeckIntoTrips();
    }
}
