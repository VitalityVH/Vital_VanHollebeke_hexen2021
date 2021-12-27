using System;
using System.Collections;
using System.Collections.Generic;
using Hexen.HexenSystem;
using UnityEngine;

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
        #region EventHandlers

        public event EventHandler<CapsuleEventArgs<TPosition>> Teleported;
        public event EventHandler<CapsuleEventArgs<TPosition>> Pushed;
        public event EventHandler<CapsuleEventArgs<TPosition>> Hit;
        public event EventHandler<CapsuleEventArgs<TPosition>> UnHit;

        #endregion

        #region Properties

        public CapsuleType CapsuleType { get; set; }
        public TPosition HexTile;

        #endregion
        
        #region Teleport

        public void TeleportTo(TPosition position)
        {
            OnTeleported(new CapsuleEventArgs<TPosition>(position));
        }

        protected virtual void OnTeleported(CapsuleEventArgs<TPosition> capsuleEventArgs)
        {
            var handler = Teleported;
            handler?.Invoke(this, capsuleEventArgs);
        }

        #endregion

        #region Push

        public void PushedTo(TPosition position)
        {
            OnPushed(new CapsuleEventArgs<TPosition>(position));
        }

        protected virtual void OnPushed(CapsuleEventArgs<TPosition> capsuleEventArgs)
        {
            var handler = Pushed;
            handler?.Invoke(this, capsuleEventArgs);
        }

        #endregion

        #region Hit

        public void HitFrom(TPosition position)
        {
            OnHit(new CapsuleEventArgs<TPosition>(position));
        }

        protected virtual void OnHit(CapsuleEventArgs<TPosition> capsuleEventArgs)
        {
            var handler = Hit;
            handler?.Invoke(this, capsuleEventArgs);
        }

        #endregion

        public void Reappear(TPosition position)
        {
            OnReappear(new CapsuleEventArgs<TPosition>(position));
        }

        protected virtual void OnReappear(CapsuleEventArgs<TPosition> capsuleEventArgs)
        {
            var handler = UnHit;
            handler?.Invoke(this, capsuleEventArgs);
        }
    }
}

