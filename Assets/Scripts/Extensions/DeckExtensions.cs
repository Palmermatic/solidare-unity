using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Extensions
{
    public static class DeckExtensions
    {
        private static readonly System.Random rng = new System.Random();

        /// <summary>
        /// Randomizes the order of cards in the deck.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deck"></param>
        public static IEnumerator Shuffle(this Deck deck)
        {
            int n = deck.Cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                deck.Cards.SwapCards(k, n);
                yield return new WaitForSeconds(0.05f);
            }

            deck.Cards = deck.Cards.OrderBy(c => c.transform.position.z).ToList();
            foreach (var card in deck.Cards)
            {
                card.gameObject.transform.SetAsLastSibling();
            }
            //GameManager.Instance.Deal();
        }

        /// <summary>
        /// Swaps the list position and transform positions of two cards.
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        private static void SwapCards(this IList<Card> deck, int c1, int c2)
        {
            var temp = deck[c1];
            deck[c1] = deck[c2];
            deck[c2] = temp;

            var p1 = deck[c1].transform.position;
            var p2 = deck[c2].transform.position;

            //deck[c1].transform.DOJump(new Vector3(p1.x, p1.y, p2.z), 10f, 1, 0.2f);
            //deck[c2].transform.DOJump(new Vector3(p2.x, p2.y, p1.z), -10f, 1, 0.2f);
            ShuffleAnimate(deck[c1].transform, p2.z);
            ShuffleAnimate(deck[c2].transform, p1.z);
        }

        private static void ShuffleAnimate(Transform xform, float z)
        {
            xform.DOLocalMoveY(30, 0.2f).OnComplete(() =>
            {
                xform.DOLocalMoveX(100, 0.2f).OnComplete(() =>
                {
                    xform.DOLocalMoveZ(z, 0).OnComplete(() =>
                    {
                        xform.DOLocalMoveY(0, 0.2f).OnComplete(() =>
                        {
                            xform.DOLocalMoveX(0, 0.2f);
                        });
                    });
                });
            });
        }
    }
}
