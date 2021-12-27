using System.Collections.Generic;
using Hexen.HexenSystem.PlayableCards;

namespace Hexen.HexenSystem
{
    public interface ICard<TPosition>
    {
        public PlayableCardName Type { get; set; }
        void SetActive(bool active);
        bool CanExecute(TPosition atPosition);
        void Execute(TPosition atPosition);
        void ActivateLayoutGroup();
        List<TPosition> Positions(TPosition atPosition);
    }
}