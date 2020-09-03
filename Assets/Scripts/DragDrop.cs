using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject Canvas;

    private bool isDragging = false;
    private bool isOverDropzone = false;
    private GameObject dropZone;
    private Vector2 startPosition;
    private GameObject startParent;

    void Awake()
    {
        Canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
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
        if (card.IsFaceUp && gameObject.GetComponentInParent<Deck>().Cards.Last() == card)
        {
            startParent = transform.parent.gameObject;
            startPosition = transform.position;
            isDragging = true;
            transform.SetParent(Canvas.transform, true);
        }
    }

    public void EndDrag()
    {
        if (!isDragging) { return; }

        isDragging = false;

        if (!isOverDropzone)
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
            return;
        }

        var cardInHand = gameObject.GetComponent<Card>();
        var dropCard = dropZone.GetComponentsInChildren<Card>().LastOrDefault();

        if (dropCard == null)
        {
            // dropped onto empty slot
            if ((dropZone.CompareTag("Bureau") && cardInHand.Face == Face.Ace)
                || (dropZone.CompareTag("Tableau") && cardInHand.Face == Face.King))
            {
                cardInHand.MoveCardToDeck(dropZone.GetComponent<Deck>(), 0, startParent.GetComponent<Deck>());
            }
            else
            {
                SnapBack();
            }
            return;
        }
        else
        {
            // dropped onto another card
            if    ((dropCard.CompareTag("Bureau")
                    && cardInHand.Suit == dropCard.Suit
                    && cardInHand.Face == dropCard.Face + 1)
                || (dropCard.CompareTag("Tableau")
                    && cardInHand.Color != dropCard.Color
                    && cardInHand.Face == dropCard.Face - 1)
                || (dropCard.CompareTag("Discard")))
            {
                cardInHand.MoveCardToDeck(dropZone.GetComponent<Deck>(), 0, startParent.GetComponent<Deck>());
                return;
            }
        }

        // dropped elsewhere
        SnapBack();
    }

    void SnapBack()
    {
        transform.position = startPosition;
        transform.SetParent(startParent.transform, false);
        isOverDropzone = false;
        dropZone = null;
        startParent = null;
    }
}
