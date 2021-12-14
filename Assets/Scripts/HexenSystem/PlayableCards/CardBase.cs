using System.Collections.Generic;
using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.HexenSystem.PlayableCards
{
    public abstract class CardBase<TPosition> : IMove<TPosition>
    {
        public PlayableCardName PlayableCardName { get; set; }

        public string Description;
        public Sprite Sprite;
        protected Board<Capsule<TPosition>, TPosition> Board { get; }
        protected Grid<TPosition> Grid { get; }
            // private Grid<>

        protected CardBase(Board<Capsule<TPosition>, TPosition> board, Grid<TPosition> grid)
        {
            Board = board; 
            Grid = grid;
        }

        public abstract bool CanExecute(CardBase<TPosition> card);

        public abstract void Execute(CardBase<TPosition> card, TPosition position);

        public abstract List<TPosition> Positions(Capsule<TPosition> capsule, CardBase<TPosition> card);


    }
}