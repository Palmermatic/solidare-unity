using Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Solitaire : MonoBehaviour
{
    // editor-hardcoded
    public GameObject CardPrefab; 
    public Button DrawCardButton;
    public GameObject[] BottomPos, TopPos;

    public List<Card> DrawPile, CurrentlyDrawnCards, DiscardPile;
    private List<List<Card>> Tableau, Bureau;

    private int trips, tripsRemainder, deckLocation, bottomPositionCount, deckDrawMultiple, tableauCount;


    private void Awake()
    {
        BoardSetup();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void BoardSetup()
    {
        AssignBoardPresets();
        DealCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DealCards()
    {

        DrawPile.Unwrap();

        DrawPile.Shuffle();

        SolitaireSort();

        StartCoroutine(Deal());

        //SortDeckIntoTrips();
    }

    public void AssignBoardPresets()
    {
        Tableau = new List<List<Card>>();
        Tableau.Add(new List<Card>()) ;
        tableauCount = 1;
        //Bureau = new List<Card>();
        //Tableau = new List<Card>[] { };
        //Bureau = new List<Card>[] { };
        CurrentlyDrawnCards = new List<Card>();
        DrawPile = new List<Card>();
        DiscardPile = new List<Card>();
        //deckDrawMultiple = new List<List<Card>>();

        //for (int i = 0; i < bottomPositionCount; i++)
        //{
        //    bottoms.Add = new List<Card>[] { };
        //}
        //bottoms = new List<Card>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        //foreach (var bottom in bottoms)
        //{

        //}
        //{

        //}
    }

    public void ClickDrawPile()
    {

    }




    IEnumerator Deal()
    {
        for (int i = 0; i < tableauCount; i++)
        {
            float yOffset = 0;
            float zOffset = 0.03f;

            foreach (var card in Tableau[i])
            {
                yield return new WaitForSeconds(0.13f);
                GameObject newCard = Instantiate(
                    CardPrefab,
                    new Vector3(
                        BottomPos[i].transform.position.x,
                        BottomPos[i].transform.position.y - yOffset,
                        BottomPos[i].transform.position.z - zOffset
                    ),
                    Quaternion.identity,
                    BottomPos[0].transform

                );
                newCard.name = card.ToString();
                newCard.tag = "Card";

                newCard.GetComponent<Selectable>().IsFaceUp = card == Tableau[i][Tableau[i].Count - 1];
                newCard.GetComponent<UpdateSprite>().CardFace = card.Image;

                yOffset += 0.3f;
                zOffset += 0.03f;
                DiscardPile.Add(card);
            }
        }

        foreach (var card in DiscardPile)
        {
            if (DrawPile.Any(unDrawn => unDrawn.Suit == card.Suit && unDrawn.Face == card.Face))
            {
                DrawPile.Remove(card);
            }
        }
        DiscardPile.Clear();
    }

    void SolitaireSort()
    {
        for (int i = 0; i < tableauCount; i++)
        {
            for (int j = i; j < tableauCount; j++)
            {
                Tableau[j].Add(DrawPile.Last());
                DrawPile.RemoveAt(DrawPile.Count - 1);
            }
        }
    }

    //public void SortDeckIntoTrips()
    //{
    //    trips = DrawPile.Count / 3;
    //    tripsRemainder = DrawPile.Count % 3;
    //    deckDrawMultiple.Clear();

    //    int modifier = 0;
    //    for (int i = 0; i < trips; i++)
    //    {
    //        var myTrips = new List<Card>();
    //        for (int j = 0; j < 3; j++)
    //        {
    //            myTrips.Add(DrawPile[j + modifier]);
    //        }
    //        deckDrawMultiple.Add(myTrips);
    //        modifier += 3;
    //    }
    //    if (tripsRemainder != 0)
    //    {
    //        var myRemainders = new List<Card>();
    //        modifier = 0;
    //        for (int k = 0; k < tripsRemainder; k++)
    //        {
    //            myRemainders.Add(DrawPile[DrawPile.Count - tripsRemainder + modifier]);
    //            modifier++;
    //        }
    //        deckDrawMultiple.Add(myRemainders);
    //        trips++;
    //    }
    //    deckLocation = 0;
    //}

    //public void DrawThree()
    //{
    //    // add remaining cards to discard pile
    //    foreach (Transform child in DeckButton.transform)
    //    {
    //        if (child.CompareTag("Card"))
    //        {
    //            DrawPile.Remove(child.name.ToCard());
    //            DiscardPile.Add(child.name.ToCard());
    //            Destroy(child.gameObject);
    //        }
    //    }

    //    if (deckLocation < trips)
    //    {
    //        // draw 3
    //        CurrentlyDrawnCards.Clear();
    //        float xOffset = 2.5f;
    //        float zOffset = -0.2f;

    //        //foreach (var card in deckDrawMultiple[deckLocation])
    //        //{
    //        //    GameObject newTopCard = Instantiate(
    //        //        CardPrefab, new Vector3(
    //        //            DeckButton.transform.position.x + xOffset,
    //        //            DeckButton.transform.position.y,
    //        //            DeckButton.transform.position.z + zOffset),
    //        //        Quaternion.identity
    //        //    );
    //        //    xOffset += 0.5f;
    //        //    zOffset -= 0.2f;
    //        //    newTopCard.name = card.ToString();
    //        //    newTopCard.GetComponent<UpdateSprite>().cardFace = card.Sprite;
    //        //    CurrentlyDrawnCards.Add(card);
    //        //    newTopCard.GetComponent<Selectable>().IsFaceUp = true;
    //        //}
    //        deckLocation++;
    //    }
    //    else
    //    {
    //        RestackTopDeck();
    //    }
    //}

    //private void RestackTopDeck()
    //{
    //    foreach (var card in DiscardPile)
    //    {
    //        DrawPile.Add(card);
    //    }
    //    DiscardPile.Clear();
    //    //SortDeckIntoTrips();
    //}
}
