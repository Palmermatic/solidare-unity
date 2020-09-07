using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotClick : MonoBehaviour, IPointerClickHandler
{
    public Sprite MoreDeals;
    public Sprite NoMoreDeals;
    public int NumberOfRedrawsLeft;

    void Awake()
    {
        NumberOfRedrawsLeft = GameManager.Instance.options.NumberOfRedraws;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Draw"))
        {
            if (NumberOfRedrawsLeft-- > 0)
            {
                foreach (var card in GameManager.Instance.FlopPile.Cards.ToList())
                {
                    card.MoveCardToDeck(GameManager.Instance.DrawPile);
                    card.Flip(false);
                    card.tag = "Draw";
                }
                foreach (var card in GameManager.Instance.DiscardPile.Cards.ToList())
                {
                    card.MoveCardToDeck(GameManager.Instance.DrawPile);
                    card.Flip(false);
                    card.tag = "Draw";
                }
            }
        }
    }

    /// <summary>
    /// Draws three cards and puts them on top of the discard pile, tagging them 'Discard'
    /// </summary>
    public void DrawCards()
    {
        var flop = GameManager.Instance.FlopPile;
        var draw = GameManager.Instance.DrawPile;
        var discard = GameManager.Instance.DiscardPile;

        foreach (var card in flop.Cards.ToList())
        {
            card.MoveCardToDeck(discard);
        }

        var num = GameManager.Instance.options.NumberToDraw;
        for (int i = 0; i < num; i++)
        {
            if (!draw.Cards.Any()) { continue; }

            var card = draw.Cards.Last();
            card.Flip();
            card.MoveCardToDeck(flop);
            card.tag = "Discard";
        }

        if (!draw.Cards.Any())
        {
            if (NumberOfRedrawsLeft > 0)
            {
                draw.GetComponent<Image>().sprite = MoreDeals;
            }
            else
            {
                draw.GetComponent<Image>().sprite = NoMoreDeals;
            }
        }
    }
}
