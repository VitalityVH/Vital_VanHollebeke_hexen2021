using System;
using Hexen.Plugins;

namespace Hexen.BoardSystem
{
    public class CapsulePlacedEventArgs<TCapsule, TPosition> : EventArgs
    {
        public TPosition AtPosition { get; }
        public TCapsule Capsule { get; }

        public CapsulePlacedEventArgs(TPosition atPosition, TCapsule capsule)
        {
            AtPosition = atPosition;
            Capsule = capsule;
        }
    }
    public class Board<TCapsule, TPosition>
    {
        public event EventHandler<CapsulePlacedEventArgs<TCapsule, TPosition>> CapsulePlaced;

        private BidirectionalDictionary<TCapsule, TPosition> _capsules
            = new BidirectionalDictionary<TCapsule, TPosition>();

        public bool TryGetCapsule(TPosition position, out TCapsule capsule)
            => _capsules.TryGetKey(position, out capsule);

        public bool TryGetPosition(TCapsule capsule, out TPosition position)
            => _capsules.TryGetValue(capsule, out position);

        public void Place(TCapsule capsule, TPosition position)
        {
            if (_capsules.ContainsKey(capsule))
                return;

            if (_capsules.ContainsValue(position))
                return;

            _capsules.Add(capsule, position);

            OnCapsulePlaced(new CapsulePlacedEventArgs<TCapsule, TPosition>(position, capsule));
        }

        protected virtual void OnCapsulePlaced(CapsulePlacedEventArgs<TCapsule, TPosition> eventArgs)
        {
            var handler = CapsulePlaced;
            handler?.Invoke(this, eventArgs);
        }

        // public void PlayCard(TPlayableCard card, TPosition position)
        // {
        //     
        // }
    }
}