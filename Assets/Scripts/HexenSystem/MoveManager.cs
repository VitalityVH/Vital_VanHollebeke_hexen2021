using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.Plugins;

namespace Hexen.HexenSystem
{
    public class MoveManager<TPosition>
        where TPosition : IPosition
    {
        private Board<Capsule<TPosition>, TPosition> _board;
        private Grid<TPosition> _grid;

        public MultiValueDictionary<PlayableCardName, IMove<TPosition>> _cardMoves;

        public MoveManager(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid)
        {
            _board = board;
            _grid = grid;

            _cardMoves.Add(PlayableCardName.Teleport,
                new ConfigurableCard<TPosition>(board, grid, (b,g,c) 
                    => new MovementHelper<TPosition>(b,g,c)
                        .ReturnAllHexTiles(MovementHelper<TPosition>.Empty)
                        .CollectValidPositions()));

            //Swipe en pushback zijn drie tile rond de speler
            _cardMoves.Add(PlayableCardName.Pushback,
                new ConfigurableCard<TPosition>(board, grid, (b, g, c)
                    => new MovementHelper<TPosition>(b, g, c)
                        .Left(1)
                        .TopLeft(1)
                        .BottomLeft(1)
                        .Right(1)
                        .TopRight(1)
                        .BottomRight(1)
                        .CollectValidPositions()));


            _cardMoves.Add(PlayableCardName.Swipe,
                new ConfigurableCard<TPosition>(board, grid, (b, g, c)
                    => new MovementHelper<TPosition>(b, g, c)
                        .Left(1)
                        .TopLeft(1)
                        .BottomLeft(1)
                        .Right(1)
                        .TopRight(1)
                        .BottomRight(1)
                        .CollectValidPositions()));

            //Slash is alle richtingen tot einde board
            _cardMoves.Add(PlayableCardName.Slash,
                new ConfigurableCard<TPosition>(board,grid,(b,g,c)
                    => new MovementHelper<TPosition>(b,g,c)
                        .Left(_grid.Radius*2)
                        .TopLeft(_grid.Radius * 2)
                        .BottomLeft(_grid.Radius * 2)
                        .Right(_grid.Radius * 2)
                        .TopRight(_grid.Radius * 2)
                        .BottomRight(_grid.Radius * 2)
                        .CollectValidPositions()));
        }


        public List<TPosition> ValidPositionFor(Capsule<TPosition> capsule, CardBase<TPosition> card)
        {
            return _cardMoves[card.PlayableCardName]
                .Where((m) => m.CanExecute(card))
                .SelectMany((m) => m.Positions(capsule,card))
                .ToList();
            //get all [executable moves]
            //foreach move
            //   [get/collect positions]
            //return positions
        }

        public void Move(Capsule<TPosition> capsule, TPosition position, CardBase<TPosition> card)
        {
            var move = _cardMoves[card.PlayableCardName]
                .Where(m => m.CanExecute(card))
                .Where(m => m.Positions(capsule, card).Contains(position))
                .First();

            move.Execute(card, position);
            //get first moves
            //waarvan positie deel uit maakt van zijn valid moves
            //en [voer uit]
        }

    }
}