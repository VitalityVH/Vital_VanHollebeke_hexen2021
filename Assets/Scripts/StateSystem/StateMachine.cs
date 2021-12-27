using System.Collections.Generic;

namespace Hexen.StateSystem
{
    public class StateMachine<TState>
        where TState : IState<TState>
    {
        private Dictionary<string, TState> _states = new Dictionary<string, TState>();
        private string _currentStateName;
        public TState CurrentState => _states[_currentStateName];

        public string InitialState
        {
            set
            {
                _currentStateName = value;
                CurrentState.OnEnter();
            }
        }

        public void Register(string stateName, TState state)
        {
            _states.Add(stateName,state);
        }

        public void MoveTo(string stateName)
        {
            CurrentState?.OnExit();
            _currentStateName = stateName;
            CurrentState?.OnEnter();
        }
    }
}
//var s = new StateMachine();
//((GamePlayState)s.CurrentState).Select();
//s.CurrentState.Backward();

//var sm = new StateMachine<GameState>();
//sm.Register("PlayingP1", new PlayingGameState(1));
//sm.Register("PlayingP2", new PlayingGameState(2));
//sm.Register("Replaying", new ReplayingGameState());