using Hexen.HexenSystem;
using UnityEngine;

namespace Hexen.GameSystem
{
    public class CapsuleView : MonoBehaviour
    {
        #region Properties

        private Capsule<HexTile> model;

        #endregion

        #region Fields

        [SerializeField] private CapsuleType _capsuleType;

        #endregion
        public Capsule<HexTile> Model
            
        {
            get => model;

            set
            {
                if (model != null)
                {
                    model.Hit -= OnCapsuleHit;
                    model.Pushed -= OnCapsulePushed;
                    model.Teleported -= OnCapsuleTeleported;

                    //     model.ActivationStatusChanged -= OnPieceActivationChanged;
                }

                model = value;

                if (model != null)
                {
                    model.Hit += OnCapsuleHit;
                    model.Pushed += OnCapsulePushed;
                    model.Teleported += OnCapsuleTeleported;
                    //     model.ActivationStatusChanged += OnPieceActivationChanged;
                }
            }
        }

        public CapsuleType CapsuleType => _capsuleType;

        private void OnCapsuleHit(object sender, CapsuleEventArgs<HexTile> eventArgs)
        {

        }

        private void OnCapsulePushed(object sender, CapsuleEventArgs<HexTile> eventArgs)
        {

        }

        private void OnCapsuleTeleported(object sender, CapsuleEventArgs<HexTile> eventArgs)
        {

        }


    }
}