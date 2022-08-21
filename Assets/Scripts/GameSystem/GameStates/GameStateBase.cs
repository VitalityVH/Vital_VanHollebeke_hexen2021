using Hexen.HexenSystem;
using Hexen.StateSystem;

namespace Hexen.GameSystem.GameStates
{
    public abstract class GameStateBase: IState<GameStateBase>
    {
        public const string PlayingState = "playing";
        public const string ReplayingState = "replaying";
        public const string StartScreenState = "starting";
        public const string EndScreenState = "ending";

        public StateMachine<GameStateBase> StateMachine => _stateMachine;

        private StateMachine<GameStateBase> _stateMachine;
        protected GameStateBase(StateMachine<GameStateBase> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void OnEnter(){}
        public virtual void OnExit(){}
        public virtual void Forward(){}
        public virtual void Backward() {} 
        public virtual void Select(ICard<HexTile> card, HexTile hexTile) { }
        public virtual void Deselect(ICard<HexTile> card, HexTile hexTile) { }
        public virtual void Play(ICard<HexTile> card, HexTile hexTile) { }
    }
}