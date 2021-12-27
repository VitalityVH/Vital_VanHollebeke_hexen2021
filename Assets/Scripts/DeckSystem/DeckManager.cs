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

    public class DeckManager<TCard, TPosition>
        where TCard : ICard<TPosition>
    {
        private List<TCard> _cards = new List<TCard>();
        private int _handSize = 5;

        public EventHandler<CardEventArgs<TCard>> PlayCard;

        public void Register(TCard card)
        {
            card.SetActive(false);
            _cards.Add(card);
        }

        public void FillHand()
        {
            if (_cards.Count > _handSize)
            {
                for (int i = 0; i < _handSize; i++)
                {
                    if (_cards.Count > 0)
                    {
                        Activate(_cards[i]);
                    }
                }
            }
            
        }

        private void Activate(TCard card)
        {
            card.SetActive(true);
        }

        public void Play(TCard card, TPosition position)
        {
            if (!_cards.Remove(card))
                return;

            if (!card.CanExecute(position))
                return;

            card.Execute(position);
            OnPlay(new CardEventArgs<TCard>(card));
        }


        protected virtual void OnPlay(CardEventArgs<TCard> eventArgs)
        {
            var handler = PlayCard;
            handler?.Invoke(this, eventArgs);
        }
    }
}