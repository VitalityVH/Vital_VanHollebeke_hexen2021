using Hexen.ReplaySystem;
using Hexen.StateSystem;

namespace Hexen.GameSystem.GameStates
{
    public class ReplayGameState : GameStateBase
    {
        private readonly ReplayManager _replayManager;

        public ReplayGameState(StateMachine<GameStateBase> stateMachine, ReplayManager replayManager) : base(stateMachine)
        {
            _replayManager = replayManager;
        }

        public override void OnEnter()
        {
            Backward();
        }

        public override void Backward()
        {
            _replayManager.Backward();
        }

        public override void Forward()
        {
            _replayManager.Forward();
            if (_replayManager.IsAtEnd)
                StateMachine.MoveTo(PlayingState);
        }
    }
}