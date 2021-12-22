using System.Collections.Generic;
using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.HexenSystem.PlayableCards
{
    class ConfigurableCard<TPosition> : ICard<TPosition> where TPosition : IPosition
    {
        public delegate List<TPosition>
            PositionsCollector(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, ICard<TPosition> card);

        private PositionsCollector _collectPositions;
        private Board<Capsule<TPosition>, TPosition> _board;
        private Grid<TPosition> _grid;
        public ConfigurableCard(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, PositionsCollector positionsCollector)
        {
            _collectPositions = positionsCollector;
            _board = board;
            _grid = grid;
        }

        public List<TPosition> Positions(ICard<TPosition> card)
            => _collectPositions(_board, _grid, card);

        public bool CanExecute()
        {
            throw new System.NotImplementedException();
        }

        public void Execute(TPosition atPosition)
        {
            throw new System.NotImplementedException();
        }

        public List<TPosition> Positions()
        {
            throw new System.NotImplementedException();
        }
    }
}