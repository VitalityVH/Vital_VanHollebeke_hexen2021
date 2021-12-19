using System.Collections.Generic;
using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.HexenSystem.PlayableCards
{
    public class PushCard<TPosition> : CardBase<TPosition> where TPosition : IPosition
    {
        public delegate List<TPosition>
            PositionsCollector(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, CardBase<TPosition> card);

        private PositionsCollector _collectPositions;

        public PushCard(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, PositionsCollector positionsCollector) : base(board, grid)
        {
            Description = "Pushes enemies back in a chosen neighboring hexTile";
            PlayableCardName = PlayableCardName.Pushback;
            _collectPositions = positionsCollector;
        }

        public override void Execute(TPosition atPosition)
        {
            throw new System.NotImplementedException();
        }

        public override List<TPosition> Positions(CardBase<TPosition> card)
            => _collectPositions(Board, Grid, card);

    }
}