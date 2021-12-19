using System.Collections.Generic;
using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.HexenSystem.PlayableCards
{
    public abstract class CardBase<TPosition> : ICard<TPosition>
    {
        public PlayableCardName PlayableCardName { get; set; }
        public string Description { get; set; }

        protected Board<Capsule<TPosition>, TPosition> Board { get; }
        protected Grid<TPosition> Grid { get; }
            // private Grid<>

        protected CardBase(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid)
        {
            Board = board; 
            Grid = grid;
        }

        public bool CanExecute(CardBase<TPosition> card)
        {
            return true;
        }

        public abstract void Execute(TPosition atPosition);
        public abstract List<TPosition> Positions(CardBase<TPosition> card);


    }
}