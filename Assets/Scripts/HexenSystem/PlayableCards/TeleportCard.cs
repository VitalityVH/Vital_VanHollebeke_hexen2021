using System;
using System.Collections.Generic;
using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.HexenSystem.PlayableCards
{
    public class TeleportCard<TPosition> : CardBase<TPosition> 
    {
        public delegate List<TPosition>
            PositionsCollector(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, CardBase<TPosition> card);

        private PositionsCollector _collectPositions;

        public TeleportCard(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, PositionsCollector positionsCollector) : base(board, grid)
        {
            Description = "Teleports the hero capsule to a available hexTile";
            PlayableCardName = PlayableCardName.Teleport;
            _collectPositions = positionsCollector;
        }

        public override void Execute(TPosition atPosition)
        {
            Board.Teleport(atPosition);
        }

        public override List<TPosition> Positions(CardBase<TPosition> card)
            => _collectPositions(Board, Grid, card);

    }
}