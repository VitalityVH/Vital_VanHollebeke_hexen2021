using System;
using System.Collections.Generic;
using System.Diagnostics;
using Hexen.Plugins;

namespace Hexen.BoardSystem
{
    public class Board<TCapsule, TPosition>
    {
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
            
        }

        public void Hit(TCapsule capsule)
        {
            if (!_capsules.Remove(capsule))
                return;
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
        }
    }
}