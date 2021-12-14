using System.Collections.Generic;
using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.HexenSystem.PlayableCards
{
    class ConfigurableCard<TPosition> : CardBase<TPosition>
    {
        public delegate List<TPosition>
            PositionsCollector(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, Capsule<TPosition> capsule);

        private PositionsCollector _collectPositions;

        public ConfigurableCard(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, PositionsCollector positionsCollector) : base(board, grid)
        {
            _collectPositions = positionsCollector;
        }

        public override bool CanExecute(CardBase<TPosition> card)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(CardBase<TPosition> card, TPosition position)
        {
            throw new System.NotImplementedException();
        }

        public override List<TPosition> Positions(Capsule<TPosition> capsule, CardBase<TPosition> card)
            => _collectPositions(Board, Grid, capsule);


        
    }
}