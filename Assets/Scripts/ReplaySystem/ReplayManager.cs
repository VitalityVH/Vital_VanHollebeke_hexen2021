using System.Collections.Generic;

namespace Hexen.ReplaySystem
{
    public class ReplayManager
    {
        private List<IReplayCommand> _replayCommands = new List<IReplayCommand>();
        private int _currentCommand = -1;

        public bool IsAtEnd => _currentCommand >= _replayCommands.Count - 1;

        public void Execute(IReplayCommand command)
        {
            _replayCommands.Add(command);
            Forward();
        }

        public void Backward()
        {
            if (_currentCommand < 0)
                return;

            _replayCommands[_currentCommand].Backward();
            _currentCommand--;
        }

        public void Forward()
        {
            if (IsAtEnd)
                return;
            _currentCommand++;
            _replayCommands[_currentCommand].Forward();
        }

    }
}