using Hexen.BoardSystem;
using Hexen.DeckSystem;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.ReplaySystem;
using Hexen.SelectionSystem;
using Hexen.StateSystem;
using UnityEditorInternal;

namespace Hexen.GameSystem.GameStates
{
    class PlayingGameState : GameStateBase
    {
        private Board<Capsule<HexTile>, HexTile> _board;
        private Grid<HexTile> _grid;

        private SelectionManager<HexTile> _selectionManager;
        private DeckManager<ICard<HexTile>, HexTile> _deckManager;

        public PlayingGameState(StateMachine<GameStateBase> stateMachine, Board<Capsule<HexTile>, HexTile> board,
            Grid<HexTile> grid, DeckManager<ICard<HexTile>, HexTile> deckManager, ReplayManager replayManager) : base(stateMachine)
        {
            _board = board;
            _grid = grid;
            _selectionManager = new SelectionManager<HexTile>();
            _deckManager = deckManager;
        }

        public override void OnEnter()
        {
            _deckManager.PlayCard += OnCardPlayed;
            _selectionManager.Selected += OnHexTileSelected;
            _selectionManager.Deselected += OnHexTileDeselected;
        }

        public override void OnExit()
        {
            _deckManager.PlayCard -= OnCardPlayed;
            _selectionManager.Selected -= OnHexTileSelected;
            _selectionManager.Deselected -= OnHexTileDeselected;
        }

        private void DeselectAll()
        {
            _selectionManager.DeselectAll();
        }

        public override void Deselect(ICard<HexTile> card, HexTile hexTile)
        {
            foreach (var validHexTile in card.Positions(hexTile))
            {
                _selectionManager.Deselect(validHexTile);
            }
        }

        public override void Select(ICard<HexTile> card, HexTile hexTile)
        {
            if (card.Type == PlayableCardName.Teleport && card.Positions(hexTile).Contains(hexTile))
            {
                _selectionManager.Select(hexTile);
            }
            else
            {
                foreach (var validHexTile in card.Positions(hexTile))
                {
                    _selectionManager.Select(validHexTile);
                }
            }
        }

        public override void Play(ICard<HexTile> eventArgsCard, HexTile eventArgsHexTile)
        {
            if (eventArgsCard.Positions(eventArgsHexTile).Contains(eventArgsHexTile))
            {
                _deckManager.Play(eventArgsCard, eventArgsHexTile);
            }
            else
            {
                DeselectAll();
            }
                
        }

        public override void Backward()
        {
            StateMachine.MoveTo(ReplayingState);
        }

        private void OnCardPlayed(object source, CardEventArgs<ICard<HexTile>> card)
        {
            DeselectAll();
            card.Card.ActivateLayoutGroup();
            card.Card.SetActive(false);
            _deckManager.FillHand();
        }

        private void OnHexTileDeselected(object source, SelectionEventArgs<HexTile> eventArgs)
        {
            eventArgs.SelectionItem.Highlight = false;
        }

        private void OnHexTileSelected(object source, SelectionEventArgs<HexTile> eventArgs)
        {
            eventArgs.SelectionItem.Highlight = true;
        }
    }
}