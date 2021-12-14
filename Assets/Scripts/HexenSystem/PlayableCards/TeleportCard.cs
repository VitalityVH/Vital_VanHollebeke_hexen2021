using System.Collections.Generic;
using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.HexenSystem.PlayableCards
{
    public class TeleportCard<TPosition> : CardBase<TPosition>
    {
        public TeleportCard(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid) : base(board, grid)
        {
        }
        // Description = "The player can teleport to a free hexTile";
        // PlayableCardName = PlayableCardName.Teleport;

        public override bool CanExecute(CardBase<TPosition> card)
        {
            return true;
        }

        public override void Execute(CardBase<TPosition> card, TPosition position)
        {
            throw new System.NotImplementedException();
        }

        public override List<TPosition> Positions(Capsule<TPosition> capsule, CardBase<TPosition> card)
        {
            throw new System.NotImplementedException();
        }

        
    }
}