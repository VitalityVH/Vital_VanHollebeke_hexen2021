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
    class EndScreenGameState : GameStateBase
    {
        private SelectionManager<HexTile> _selectionManager;
        private GameObject _uiElement;

        public EndScreenGameState(StateMachine<GameStateBase> stateMachine, GameObject UIElement) : base(stateMachine)
        {
            _uiElement = UIElement;
            _selectionManager = new SelectionManager<HexTile>();
        }

        public override void OnEnter()
        {
            DeselectAll();
            _uiElement.SetActive(true);
        }

        public override void OnExit()
        {
            
        }

        private void DeselectAll() => _selectionManager.DeselectAll();

    }
}