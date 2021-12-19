using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.Plugins;

namespace Hexen.HexenSystem
{
    public class MoveManager<TPosition> where TPosition : IPosition
    {
        private Board<Capsule<TPosition>, TPosition> _board;
        private Grid<TPosition> _grid;

        public List<ICard<TPosition>> _cardMoves = new List<ICard<TPosition>>();

        public MoveManager(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid)
        {
            _board = board;
            _board.CapsuleTeleported += (s, e) => e.Capsule.TeleportTo(e.ToPosition);


            _grid = grid;

            _cardMoves.Add(
                new TeleportCard<TPosition>(board, grid, (b,g,c) 
                    => new MovementHelper<TPosition>(b,g,c)
                        .ReturnAllHexTiles(MovementHelper<TPosition>.Empty)
                        .CollectValidPositions()));

            //Swipe en pushback zijn drie tile rond de speler
            _cardMoves.Add(
                new SwipeCard<TPosition>(board, grid, (b, g, c)
                    => new MovementHelper<TPosition>(b, g, c)
                        .Left(1)
                        .TopLeft(1)
                        .BottomLeft(1)
                        .Right(1)
                        .TopRight(1)
                        .BottomRight(1)
                        .CollectValidPositions()));


            _cardMoves.Add(
                new PushCard<TPosition>(board, grid, (b, g, c)
                    => new MovementHelper<TPosition>(b, g, c)
                        .Left(1)
                        .TopLeft(1)
                        .BottomLeft(1)
                        .Right(1)
                        .TopRight(1)
                        .BottomRight(1)
                        .CollectValidPositions()));

            //Slash is alle richtingen tot einde board
            _cardMoves.Add(
                new SlashCard<TPosition>(board,grid,(b,g,c)
                    => new MovementHelper<TPosition>(b,g,c)
                        .Left()
                        .TopLeft()
                        .BottomLeft()
                        .Right()
                        .TopRight()
                        .BottomRight()
                        .CollectValidPositions()));
        }


        public List<TPosition> ValidPositionFor(CardBase<TPosition> card)
        {
            return _cardMoves
                .Where((m) => m.CanExecute(card))
                .SelectMany((m) => m.Positions(card))
                .ToList();

            // return _cardMoves[capsule.CapsuleType]
            //     .Where((m) => m.CanExecute(capsule))
            //     .SelectMany((m) => m.Positions(capsule))
            //     .ToList();
        }

        public void Teleport(CardBase<TPosition> card, TPosition position)
        {
            var move = _cardMoves
                .Where(m => m.CanExecute(card))
                .Where(m => m.Positions(card).Contains(position))
                .First();

            move.Execute(position);
            //get first moves
            //waarvan positie deel uit maakt van zijn valid moves
            //en [voer uit]
        }

    }
}