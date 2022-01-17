using System;
using Hexen.BoardSystem;
using Hexen.DeckSystem;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.ReplaySystem;
using Hexen.SelectionSystem;
using Hexen.StateSystem;
using UnityEditorInternal;
using UnityEngine;

namespace Hexen.GameSystem.GameStates
{
    class StartScreenGameState : GameStateBase
    {
        private SelectionManager<HexTile> _selectionManager;
        private GameObject _uiElement;

        public StartScreenGameState(StateMachine<GameStateBase> stateMachine, GameObject UIElement) : base(stateMachine)
        {
            _uiElement = UIElement;
            _selectionManager = new SelectionManager<HexTile>();
        }

        public override void OnEnter()
        {
            DeselectAll();
        }

        public override void OnExit()
        {
            _uiElement.SetActive(false);
        }

        public override void Forward()
        {
            StateMachine.MoveTo(PlayingState);
        }

        private void DeselectAll() => _selectionManager.DeselectAll();

    }
}