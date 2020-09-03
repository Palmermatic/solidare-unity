using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
            var card = GameManager.Instance.GetComponents<Card>().Single(c => c.Suit == s && c.Face == f);
            return card;
        }

        public static void PickUp(this Card card, Deck source = null)
        {
            if (source == null)
            {
                source = card.gameObject.GetComponentInParent<Deck>();
            }
            source.Cards.Remove(card);
        }

        public static void Flip(this Card card, bool faceUp = true)
        {
            card.IsFaceUp = faceUp;
            card.GetComponent<Image>().sprite = faceUp ? card.CardFace : card.CardBack;
        }

        public static void MoveCardToDeck(this Card card, Deck deck, int spacing = 0, Deck source = null)
        {
            card.PickUp(source);
            var verticalSpace = deck.CompareTag("Tableau") ? 16 * deck.Cards.Count : spacing;
            var dest = new Vector3(
                deck.transform.position.x,
                deck.transform.position.y - verticalSpace,
                deck.transform.position.z - deck.Cards.Count);

            deck.Add(card);
            deck.GetComponent<BoxCollider2D>().size = new Vector2(200, 290 + verticalSpace);
            if (source != null)
            {
                source.GetComponent<BoxCollider2D>().size = new Vector2(200, 290);
            }

            card.transform
                .DOMove(dest, GameManager.Instance.options.DealSpeed)
                .SetEase(Ease.OutQuart)
                .OnComplete(() =>
                {
                    card.transform.SetParent(deck.transform, false);
                });
        }
    }
}
