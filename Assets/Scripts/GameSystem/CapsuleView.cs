using System;
using Hexen.BoardSystem;
using Hexen.HexenSystem;
using UnityEngine;

namespace Hexen.GameSystem
{
    public class CapsuleView : MonoBehaviour
    {
        private Capsule<HexTile> model;
        public Capsule<HexTile> Model
        {
            get => model;

            set
            {
                if (model != null)
                {
                    model.Teleported -= OnCapsuleTeleported;
                    model.Hit -= OnCapsuleHit;
                    model.Pushed -= OnCapsulePushed;

                    //     model.ActivationStatusChanged -= OnPieceActivationChanged;
                }

                model = value;

                if (model != null)
                {
                    model.Teleported += OnCapsuleTeleported;
                    model.Hit += OnCapsuleHit;
                    model.Pushed += OnCapsulePushed;
                    //     model.ActivationStatusChanged += OnPieceActivationChanged;
                }
            }
        }

        private void OnCapsuleHit(object sender, CapsuleEventArgs<HexTile> eventArgs)
        {
            if (this.gameObject != null)
                Destroy(this.gameObject);
        }

        private void OnCapsulePushed(object sender, CapsuleEventArgs<HexTile> eventArgs)
        {
            gameObject.transform.position = eventArgs.Position.transform.position;
        }

        private void OnCapsuleTeleported(object sender, CapsuleEventArgs<HexTile> eventArgs)
        {
            gameObject.transform.position = eventArgs.Position.transform.position;
        }
    }
}