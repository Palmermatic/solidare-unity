using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DragDrop : MonoBehaviour
{
    public GameObject Canvas;

    private bool isDragging = false;
    private bool isOverDropzone = false;

    private GameObject dropZone;
    private GameObject startParent;

    void Awake()
    {
        Canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        if (isDragging)
        {
            GameManager.Instance.CardsInHand.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropzone = true;
        dropZone = collision.gameObject;
        //Debug.Log(gameObject.name + " is hovering over " + dropZone.name);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropzone = false;
        //Debug.Log("End hover");
        dropZone = null;
    }

    public void StartDrag()
    {
        var card = gameObject.GetComponent<Card>();
        var deck = gameObject.GetComponentInParent<Deck>();

        if (!card.IsFaceUp) { return; }

        isDragging = true;
        startParent = transform.parent.gameObject;
        var deckInHand = GameManager.Instance.CardsInHand;

        var i = deck.Cards.IndexOf(card);
        var cardStack = deck.Cards.GetRange(i, deck.Cards.Count() - i);
        card.PickUp();
        foreach (var c in cardStack)
        {
            c.PickUp();
            deckInHand.Cards.Add(c);
            c.transform.SetParent(deckInHand.transform, false);
        }
    }

    public void EndDrag()
    {
        if (!isDragging) { return; }

        isDragging = false;

        if (!isOverDropzone)
        {
            SnapBack();
            return;
        }

        var cardInHand = GetComponent<Card>();
        var cardsInHand = GetComponentInParent<Deck>();
        var dropCard = dropZone.GetComponentsInChildren<Card>().LastOrDefault();

        // dropped onto empty slot
        if (dropCard == null)
        {
            if (dropZone.CompareTag("Bureau") && cardInHand.Face == Face.Ace)
            {
                cardInHand.MoveCardToDeck(dropZone.GetComponent<Deck>());
            }

            if (dropZone.CompareTag("Tableau") && cardInHand.Face == Face.King)
            {
                if (cardsInHand != null)
                {
                    foreach (var card in cardsInHand.Cards.ToList())
                    {
                        card.MoveCardToDeck(dropZone.GetComponent<Deck>());
                    }
                    return;
                }
                cardInHand.MoveCardToDeck(dropZone.GetComponent<Deck>());
            }
            else
            {
                SnapBack();
            }
            return;
        }

        // dropped onto another card
        if    ((dropCard.CompareTag("Bureau")
                && cardInHand.Suit == dropCard.Suit
                && cardInHand.Face == dropCard.Face + 1)
            || (dropCard.CompareTag("Tableau")
                && cardInHand.Color != dropCard.Color
                && cardInHand.Face == dropCard.Face - 1)
            || (dropCard.CompareTag("Discard")))
        {
            foreach (var card in cardsInHand.Cards.ToList())
            {
                card.MoveCardToDeck(dropZone.GetComponent<Deck>());
            }
            return;
        }


        // dropped elsewhere
        SnapBack();
    }

    void SnapBack()
    {
        foreach (var card in GameManager.Instance.CardsInHand.Cards.ToList())
        {
            card.MoveCardToDeck(startParent.GetComponent<Deck>());
        }

        isOverDropzone = false;
        dropZone = null;
        startParent = null;
    }
}
