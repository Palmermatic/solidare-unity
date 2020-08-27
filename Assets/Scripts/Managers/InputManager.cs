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
            if (card.Face == Face.Ace)
            {
                // ace goes to the first empty Bureau

                card.PickUp();
                GameManager.Instance.Bureau.First(b => b.Cards.Count == 0).Cards.Add(card);
            }
            else
            {
                // card can go up if suit matches and previous face value is on top
                var column = GameManager.Instance.Bureau.Where(deck =>
                    deck.Cards.Last().Suit == card.Suit && deck.Cards.Last().Face == (card.Face - 1)).ToList();

                if (column.Count == 1)
                {
                    card.PickUp();
                    column.Single().Cards.Add(card);
                }
            }
        }
    }
}
