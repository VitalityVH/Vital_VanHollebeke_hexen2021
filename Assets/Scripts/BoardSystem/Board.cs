using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class CapsuleTeleportedEventArgs<TCapsule, TPosition> : EventArgs
    {
        public TCapsule Capsule { get; }
        public TPosition FromPosition { get; }
        public TPosition ToPosition { get; }
        
    
        public CapsuleTeleportedEventArgs(TCapsule capsule, TPosition fromPosition, TPosition toPosition)
        {
            Capsule = capsule;
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }
    }

    public class CapsulePushedEventArgs<TCapsule, TPosition> : EventArgs
    {
        public TCapsule Capsule { get; }
        public TPosition FromPosition { get; }
        public TPosition ToPosition { get; }


        public CapsulePushedEventArgs(TCapsule capsule, TPosition fromPosition, TPosition toPosition)
        {
            Capsule = capsule;
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }
    }

    public class CapsuleHitEventArgs<TCapsule>
    {
        public TCapsule Capsule { get; }

        public CapsuleHitEventArgs(TCapsule capsule)
        {
            Capsule = capsule;
        }
    }

    public class Board<TCapsule, TPosition>
    {
        public event EventHandler<CapsulePlacedEventArgs<TCapsule, TPosition>> CapsulePlaced;
        public event EventHandler<CapsuleTeleportedEventArgs<TCapsule, TPosition>> CapsuleTeleported;
        public event EventHandler<CapsulePushedEventArgs<TCapsule, TPosition>> CapsulePushed;
        public event EventHandler<CapsuleHitEventArgs<TCapsule>> CapsuleHit;

        private BidirectionalDictionary<TCapsule, TPosition> _capsules
            = new BidirectionalDictionary<TCapsule, TPosition>();

        public TCapsule HeroCapsule;

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

        public void Hit(TCapsule capsule)
        {
            if (!_capsules.Remove(capsule))
                return;
                
            OnCapsuleHit(new CapsuleHitEventArgs<TCapsule>(capsule));
        }

        public void Push(TCapsule capsule, TPosition toPosition)
        {
            if (!TryGetPosition(capsule, out var fromPosition))
                return;

            if (TryGetCapsule(toPosition, out _))
                return;

            if (!_capsules.Remove(capsule))
                return;

            _capsules.Add(capsule, toPosition);
            OnCapsulePushed(new CapsulePushedEventArgs<TCapsule, TPosition>(capsule, fromPosition, toPosition));
        }

        public void Teleport(TPosition toPosition)
        {
            if (!TryGetPosition(HeroCapsule, out var fromPosition))
                return;
            if (TryGetCapsule(toPosition, out _))
                return;
            if (!_capsules.Remove(HeroCapsule))
                return;
            
            _capsules.Add(HeroCapsule, toPosition);
            OnCapsuleTeleported(new CapsuleTeleportedEventArgs<TCapsule, TPosition>(HeroCapsule, fromPosition, toPosition));
        }

        protected virtual void OnCapsuleHit(CapsuleHitEventArgs<TCapsule> eventArgs)
        {
            var handler = CapsuleHit;
            handler?.Invoke(this, eventArgs);
        }
        protected virtual void OnCapsulePlaced(CapsulePlacedEventArgs<TCapsule, TPosition> eventArgs)
        {
            var handler = CapsulePlaced;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnCapsuleTeleported(CapsuleTeleportedEventArgs<TCapsule, TPosition> eventArgs)
        {
            var handler = CapsuleTeleported;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnCapsulePushed(CapsulePushedEventArgs<TCapsule, TPosition> eventArgs)
        {
            var handler = CapsulePushed;
            handler?.Invoke(this, eventArgs);
        }
    }
}