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
        public GameObject CardPrefab;
        public Button DrawCardButton;
        public GameObject[] BottomPos, TopPos;

        public Deck DrawPile, DiscardPile;
        public List<Deck> Tableau, Bureau;
        public GameOptions options;

        void Start()
        {
            AssignBoardPresets();
            DealCards();
        }

        void Update()
        {

        }

        public void DealCards()
        {

            DrawPile.Unwrap();

            DrawPile.Shuffle();

            SolitaireDeal();

            StartCoroutine(Deal());
        }

        public void AssignBoardPresets()
        {
            Bureau = new List<Deck>();
            Tableau = new List<Deck>();

            DrawPile = Deck.Empty;
            DiscardPile = Deck.Empty;

        }

        IEnumerator Deal()
        {
            for (int i = 0; i < options.NumberOfColumns; i++)
            {
                float yOffset = 0;
                float zOffset = 0.03f;

                foreach (var card in Tableau[i].Cards)
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


                    newCard.GetComponent<Card>().IsFaceUp = card == Tableau[i].Cards.Last();

                    yOffset += 0.3f;
                    zOffset += 0.03f;
                    DiscardPile.Add(card);
                }
            }

            foreach (var card in DiscardPile.Cards)
            {
                if (DrawPile.Cards.Any(unDrawn => unDrawn.Suit == card.Suit && unDrawn.Face == card.Face))
                {
                    DrawPile.Cards.Remove(card);
                }
            }
            DiscardPile.Cards.Clear();
        }

        /// <summary>
        /// Deals out the Klondike Solitaire columns.
        /// </summary>
        void SolitaireDeal()
        {
            for (int i = 0; i < options.NumberOfColumns; i++)
            {
                for (int j = i; j < options.NumberOfColumns; j++)
                {
                    Tableau[j].Add(DrawPile.Draw());
                }
            }
        }
    }

}