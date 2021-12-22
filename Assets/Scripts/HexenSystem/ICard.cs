using System.Collections.Generic;
using Hexen.HexenSystem.PlayableCards;

namespace Hexen.HexenSystem
{
    public interface ICard<TPosition>
    {
        bool CanExecute();
        void Execute(TPosition atPosition);
        List<TPosition> Positions();
    }
}