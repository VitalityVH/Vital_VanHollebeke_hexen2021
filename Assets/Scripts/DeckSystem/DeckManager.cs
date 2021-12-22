using System;
using System.Collections.Generic;
using Hexen.HexenSystem;

namespace Hexen.DeckSystem
{

    public class CardEventArgs<TCard> : EventArgs
    {
        public TCard Card { get; }

        public CardEventArgs(TCard card)
        {
            Card = card;
        }
    }

    public class DeckManager<TPosition>
    {
        private List<ICard<TPosition>> _cards;

        public EventHandler<CardEventArgs<ICard<TPosition>>> PlayCard;

        public void Register(ICard<TPosition> card)
        {
            _cards.Add(card);
        }

        public void FillHand()
        {

        }

        public bool Play(ICard<TPosition> card, TPosition position)
        {
            return true;
        }


        protected virtual void OnPlay(CardEventArgs<ICard<TPosition>> eventArgs)
        {
            var handler = PlayCard;
            handler?.Invoke(this, eventArgs);
        }
    }
}