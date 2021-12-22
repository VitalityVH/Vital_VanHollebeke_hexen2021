using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.HexenSystem.PlayableCards;

namespace Hexen.HexenSystem
{
    internal class MovementHelper<TPosition> 
    {
        private Board<Capsule<TPosition>, TPosition> _board;
        private Grid<TPosition> _grid;
        private ICard<TPosition> _card;
        private List<TPosition> _validPositions = new List<TPosition>();

        public MovementHelper(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, ICard<TPosition> card)
        {
            _board = board;
            _grid = grid;
            _card = card;
        }

        public MovementHelper<TPosition> Left(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(-1, 0, maxSteps, validators);

        public MovementHelper<TPosition> TopLeft(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(0, -1, maxSteps, validators);

        public MovementHelper<TPosition> BottomLeft(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(-1, +1, maxSteps, validators);

        public MovementHelper<TPosition> Right(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(1, 0, maxSteps, validators);

        public MovementHelper<TPosition> TopRight(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(1, -1, maxSteps, validators);

        public MovementHelper<TPosition> BottomRight(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(0, 1, maxSteps, validators);

        public MovementHelper<TPosition> AllHexTiles(int maxSteps = int.MaxValue, params Validator[] validators)
            => ReturnAllHexTiles(validators);



        public MovementHelper<TPosition> Collect(int xOffset, int yOffset, int maxSteps = int.MaxValue, params Validator[] validators)
        {
            if (!_board.TryGetPosition(_board.HeroCapsule, out var currentPosition))
                return this;

            if (!_grid.TryGetCoordinateAt(currentPosition, out var currentCoordinates))
                return this;

            var nextCoordinateX = currentCoordinates.x + xOffset;
            var nextCoordinateY = currentCoordinates.y + yOffset;

            _grid.TryGetPositionAt(nextCoordinateX, nextCoordinateY, out var nextHexTile);
            var steps = 0;

            while (steps < maxSteps && nextHexTile != null && validators.All((v) => v(_board, _grid, _board.HeroCapsule, nextHexTile)))
            {
                //var nextPiece = _board.PieceAt(nextPosition);

                _validPositions.Add(nextHexTile);

                nextCoordinateX += xOffset;
                nextCoordinateY += yOffset;

                _grid.TryGetPositionAt(nextCoordinateX, nextCoordinateY, out nextHexTile);

                steps++;
            }

            return this;
        }

        public MovementHelper<TPosition> ReturnAllHexTiles(params Validator[] validators)
        {
            if (!_board.TryGetPosition(_board.HeroCapsule, out var currentPosition))
                return this;
            if (!_grid.TryGetCoordinateAt(currentPosition, out var currentCoordinates))
                return this;

            var allHexPositions = _grid.AllHexPositions();

            foreach (var hexPosition in allHexPositions)
            {
                if (validators.All((v) => v(_board, _grid, _board.HeroCapsule, hexPosition.Key)))
                {
                    _validPositions.Add(hexPosition.Key);
                }
            }

            return this;
        }

        public List<TPosition> CollectValidPositions()
        {
            return _validPositions;
        }

        public delegate bool Validator(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, Capsule<TPosition> capsule, TPosition toPosition);

        public static bool Empty(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, Capsule<TPosition> capsule, TPosition toPosition)
            => !board.TryGetCapsule(toPosition, out var _);

        public static bool ContainsEnemy(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid, Capsule<TPosition> capsule,
            TPosition toPosition)
            => board.TryGetCapsule(toPosition, out var toCapsule) && toCapsule.CapsuleType == CapsuleType.Enemy;

    }
}