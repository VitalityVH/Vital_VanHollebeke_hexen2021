using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.HexenSystem.PlayableCards;
using UnityEngine.Scripting;

namespace Hexen.HexenSystem
{
    public class MovementHelper<TPosition>
    {

        public static List<(int x, int y)> Offsets = new List<(int x, int y)>()
        {
            (1, 0), (0,1), (-1, 1), (-1, 0), (0, -1), (1, -1)
        };

        private Board<Capsule<TPosition>, TPosition> _board;
        private Grid<TPosition> _grid;
        private List<TPosition> _validPositions = new List<TPosition>();

        public MovementHelper(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid)
        {
            _board = board;
            _grid = grid;
        }
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

        public MovementHelper<TPosition> CollectAtPosition(TPosition hexTile, int xOffset, int yOffset, int maxSteps = int.MaxValue, params Validator[] validators)
        {
            if (!_grid.TryGetCoordinateAt(hexTile, out var currentCoordinates))
                return this;

            var nextCoordinateX = currentCoordinates.x + xOffset;
            var nextCoordinateY = currentCoordinates.y + yOffset;

            _grid.TryGetPositionAt(nextCoordinateX, nextCoordinateY, out var nextHexTile);
            var steps = 0;
            
            while (steps < maxSteps && nextHexTile != null && validators.All((v) => v(_board, _grid, _board.HeroCapsule, nextHexTile)))
            {
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
        
    }
}