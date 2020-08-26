using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        void ClickDrawPile()
        {
            var num = GameManager.Instance.options.NumberToDraw;
            for (int i = 0; i < num; i++)
            {
                var card = GameManager.Instance.DrawPile.Draw();
                card.IsFaceUp = true;
                GameManager.Instance.DiscardPile.Add(card);
            }
        }

        void DoubleClickCard(Card card)
        {
            var suit = card.Suit;
            var column = GameManager.Instance.Bureau.Where(deck => deck.Cards.All(cards => cards.Suit == suit));
            if (GameManager.Instance.Bureau.Any(deck => deck.Cards.Any(c => c.Suit == suit)))
            {
                card.PickUp();

            }
        }
    }
}
