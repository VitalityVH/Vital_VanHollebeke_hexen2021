using System;
using System.Collections.Generic;
using Hexen.HexenSystem.PlayableCards;

namespace Hexen.HexenSystem
{
    public interface ICard<TPosition>
    {
        public PlayableCardName Type { get; set; }
        void SetActive(bool active);
        bool CanExecute(TPosition atPosition);
        void Execute(TPosition atPosition, out Action forward, out Action backward);
        void ResetCard();
        void Fade();
        List<TPosition> Positions(TPosition atPosition);
    }
}