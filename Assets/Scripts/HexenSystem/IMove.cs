using System.Collections.Generic;
using Hexen.HexenSystem.PlayableCards;

namespace Hexen.HexenSystem
{
    public interface IMove<TPosition>
    {
        bool CanExecute(CardBase<TPosition> card);
        void Execute(CardBase<TPosition> card, TPosition position);
        List<TPosition> Positions(Capsule<TPosition> capsule, CardBase<TPosition> card);
    }
}