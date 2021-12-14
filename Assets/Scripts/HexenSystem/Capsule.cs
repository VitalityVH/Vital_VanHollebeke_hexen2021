using System;

namespace Hexen.HexenSystem
{
    public class CapsuleEventArgs<TPosition> : EventArgs
    {
        public TPosition Position { get; }

        public CapsuleEventArgs(TPosition position)
        {
            Position = position;
        }
    }

    public class Capsule<TPosition>
    {

        public event EventHandler<CapsuleEventArgs<TPosition>> Teleported;
        public event EventHandler<CapsuleEventArgs<TPosition>> Pushed;
        public event EventHandler<CapsuleEventArgs<TPosition>> Hit;

        public CapsuleType CapsuleType;
        public TPosition HexTile;

        public Capsule(CapsuleType capsuleType, TPosition hexTile)
        {
            CapsuleType = capsuleType;
            HexTile = hexTile;
        }

        public void TeleportTo(TPosition position)
        {
            OnTeleported(new CapsuleEventArgs<TPosition>(position));
        }

        protected virtual void OnTeleported(CapsuleEventArgs<TPosition> capsuleEventArgs)
        {
            var handler = Teleported;
            handler?.Invoke(this, capsuleEventArgs);
        }

        public void PushedFrom(TPosition position)
        {
            OnPushed(new CapsuleEventArgs<TPosition>(position));
        }

        protected virtual void OnPushed(CapsuleEventArgs<TPosition> capsuleEventArgs)
        {
            var handler = Pushed;
            handler?.Invoke(this, capsuleEventArgs);
        }

        public void HitFrom(TPosition position)
        {
            OnHit(new CapsuleEventArgs<TPosition>(position));
        }

        protected virtual void OnHit(CapsuleEventArgs<TPosition> capsuleEventArgs)
        {
            var handler = Hit;
            handler?.Invoke(this, capsuleEventArgs);
        }
    }
}