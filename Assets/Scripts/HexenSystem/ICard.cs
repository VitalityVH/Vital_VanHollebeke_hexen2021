using System.Collections.Generic;
using Hexen.HexenSystem.PlayableCards;

namespace Hexen.HexenSystem
{
    public interface ICard<TPosition>
    {
        bool CanExecute(CardBase<TPosition> card);
        void Execute(TPosition atPosition);
        List<TPosition> Positions(CardBase<TPosition> card);
    }
}