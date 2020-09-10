using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardClick : MonoBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) { return; }
        if (eventData.clickCount > 1)
        {
            DoubleClicked();
        }
        else
        {
            SingleClicked();
        }
    }

    /// <summary>
    /// Clicked on a card.
    /// </summary>
    public void SingleClicked()
    {
        Card card = GetComponent<Card>();
        if (card.IsFaceUp) { return; }

        Deck deck = card.gameObject.GetComponentInParent<Deck>();

        if (!deck.Cards.Any() || card != deck.Cards.Last()) { return; }

        if (CompareTag("Tableau"))
        {
            card.IsFaceUp = true;
            GetComponent<Image>().sprite = card.CardFace;
        }
        if (CompareTag("Draw"))
        {
            if (!GameManager.Instance.HasBeenDealt)
            {
                GameManager.Instance.Deal();
                return;
            }
            card.GetComponentInParent<SlotClick>().DrawCards();
        }
    }

    /// <summary>
    /// Double-clicked on a card.
    /// </summary>
    public void DoubleClicked()
    {
        Card card = GetComponent<Card>();
        Deck deck = card.GetComponentInParent<Deck>();

        if (card != deck.Cards.Last()) { return; }

        if (card.Face == Face.Ace)
        {
            // aces go to the first empty bureau
            var column = GameManager.Instance.Bureau.Find(b => b.Cards.Count == 0);
            card.MoveCardToDeck(column);
            card.tag = "Bureau";
        }
        else
        {
            // cards look for the bureau of their suit to have the preceding face value
            foreach (var col in GameManager.Instance.Bureau)
            {
                if (!col.Cards.Any()) { continue; }

                var lastCard = col.Cards.Last();
                if (lastCard.Suit == card.Suit && lastCard.Face == (card.Face - 1))
                {
                    ResourceManager.Instance.Cash += 10;
                    card.MoveCardToDeck(col);
                    card.tag = "Bureau";

                    if (card.Face == Face.King)
                    {
                        GameManager.Instance.CheckForWin();
                    }
                    return;
                }
            }
        }
    }
}
