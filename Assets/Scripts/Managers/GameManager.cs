using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject CardPrefab, EmptySlotPrefab, DrawSlotPrefab;
        public GameObject DrawContainer, DiscardContainer, FlopContainer, BureauContainer, TableauContainer, Floor;

        public Deck CardsInHand;
        public Deck DrawPile;
        public Deck DiscardPile;
        public Deck FlopPile;
        public List<Deck> Tableau = new List<Deck>();
        public List<Deck> Bureau = new List<Deck>();
        public GameOptions options = new GameOptions();

        public bool HasBeenDealt = false;
        public int NumberOfRedrawsLeft = 3;
        public Random rng;

        private GameObject Canvas;

        void Awake()
        {
            rng = new Random();
            Canvas = GameObject.Find("Canvas");
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
                    if (!DrawPile.Cards.Any()) { yield break; }
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

        public void Cheat()
        {
            var cards = Canvas.GetComponentsInChildren<Card>();

            foreach (var card in cards.OrderBy(c => c.Face).Take(51))
            {
                card.Flip();
                if (Bureau.Any(b => b.Cards.Any(c => c.Suit == card.Suit)))
                {
                    card.MoveCardToDeck(Bureau.First(b => b.Cards.Any(c => c.Suit == card.Suit)));
                }
                else
                {
                    card.MoveCardToDeck(Bureau.First(b => !b.Cards.Any()));
                }
            }
        }

        public void CheckForWin()
        {
            if (Tableau.All(t => t.Cards.Count == 0))
            {
                StartCoroutine(DropCard());
            }
        }

        public IEnumerator DropCard()
        {
            var cards = Canvas.GetComponentsInChildren<Card>().OrderByDescending(c => c.Face).ThenBy(c => c.Suit);
            foreach (var card in cards)
            {
                card.GetComponent<DragDrop>().enabled = false;

                var rb = card.gameObject.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = GetRandomVelocity();

                card.gameObject.layer = LayerMask.NameToLayer("WinningCards");

                card.PickUp();
                card.transform.SetParent(Canvas.transform, true);

                yield return new WaitWhile(() => IsInsideCanvas(card));
                Destroy(card.gameObject);
            }
        }

        private bool IsInsideCanvas(Card card)
        {
            var canvasRect = Canvas.GetComponent<RectTransform>().rect;
            return canvasRect.Contains(card.gameObject.transform.position);
        }

        private Vector2 GetRandomVelocity()
        {
            var x = rng.Next(15, 50);
            var y = rng.Next(-30, 30);

            var randomSign = rng.Next(0, 2);
            if (randomSign == 1) { x *= -1; }

            return new Vector2(x, y);
        }

        public void SpawnJoker()
        {
            var joker = Instantiate(CardPrefab, new Vector3(rng.Next(0,100), rng.Next(0,100), 0), Quaternion.identity, Canvas.transform);
            joker.GetComponent<Image>().sprite = Resources.Load<Sprite>("black_joker");
            joker.layer = LayerMask.NameToLayer("Bouncy");
            joker.name = "JOKER";
            var rb = joker.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity = GetRandomVelocity();
            rb.gravityScale = 0;
        }
    }

}