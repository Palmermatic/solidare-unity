using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject CardPrefab, EmptySlotPrefab, DrawSlotPrefab;
        public GameObject DrawContainer, DiscardContainer, FlopContainer, BureauContainer, TableauContainer;

        public Deck CardsInHand;
        public Deck DrawPile;
        public Deck DiscardPile;
        public Deck FlopPile;
        public List<Deck> Tableau = new List<Deck>();
        public List<Deck> Bureau = new List<Deck>();
        public GameOptions options = new GameOptions();

        public bool HasBeenDealt = false;
        public int NumberOfRedrawsLeft = 3;

        void Awake()
        {
            SetupGameBoard(options);
            UnwrapDeck();
            StartCoroutine(DrawPile.Shuffle());
        }

        public void Deal()
        {
            StartCoroutine(SolitaireDeal());
            HasBeenDealt = true;
        }

        /// <summary>
        /// Sets up all the empty slots according to game options.
        /// </summary>
        /// <param name="options"></param>
        public void SetupGameBoard(GameOptions options)
        {
            CardsInHand = Instantiate(EmptySlotPrefab, FindObjectOfType<Canvas>().transform).GetComponent<Deck>();
            CardsInHand.NewSlot("Cards in hand", "Hand", CardStackDirection.Vertical, -230);
            CardsInHand.GetComponent<Image>().enabled = false;
            CardsInHand.GetComponent<BoxCollider2D>().enabled = false;

            DrawPile = Instantiate(DrawSlotPrefab, DrawContainer.transform).GetComponent<Deck>();
            DrawPile.NewSlot("DrawPile", "Draw", CardStackDirection.None);

            DiscardPile = Instantiate(EmptySlotPrefab, DiscardContainer.transform).GetComponent<Deck>();
            DiscardPile.NewSlot("DiscardPile", "Discard", CardStackDirection.None, 0);


            FlopPile = Instantiate(EmptySlotPrefab, FlopContainer.transform).GetComponent<Deck>();
            FlopPile.NewSlot("FlopPile", "Discard", CardStackDirection.Horizontal, -140);

            for (int i = 0; i < options.NumberOfColumns; i++)
            {
                var d = Instantiate(EmptySlotPrefab, TableauContainer.transform).GetComponent<Deck>();
                d.NewSlot("Tableau" + i, "Tableau", CardStackDirection.Vertical, -230);
                Tableau.Add(d);
            }

            for (int i = 0; i < options.NumberOfSuits; i++)
            {
                var d = Instantiate(EmptySlotPrefab, BureauContainer.transform).GetComponent<Deck>();
                d.NewSlot("Bureau" + i, "Bureau", CardStackDirection.Vertical, -290);
                Bureau.Add(d);
            }
        }

        /// <summary>
        /// Generates all possible cards, face-down and shuffled, in the draw pile with tag 'Draw'
        /// </summary>
        public void UnwrapDeck()
        {
            var p = DrawPile.transform.position;
            float z = 0f;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Face face in Enum.GetValues(typeof(Face)))
                {
                    Card card = Instantiate(
                        CardPrefab,
                        new Vector3(p.x, p.y, p.z - z++),
                        Quaternion.identity,
                        DrawPile.transform
                    ).GetComponent<Card>();

                    card.NewCard(suit, face);
                    card.name = card.ToString();
                    card.tag = "Draw";

                    DrawPile.Cards.Add(card);
                }
            }
        }

        /// <summary>
        /// Deals out the Klondike Solitaire columns.
        /// </summary>
        public IEnumerator SolitaireDeal()
        {
            for (int i = 0; i < options.NumberOfColumns; i++)
            {
                for (int j = i; j < options.NumberOfColumns; j++)
                {
                    var card = DrawPile.Cards.Last();
                    card.MoveCardToDeck(Tableau[j]);
                    card.tag = "Tableau";

                    if (j == i)
                    {
                        card.Flip();
                    }

                    yield return new WaitForSeconds(options.DealSpeed);
                }
            }
        }
    }

}