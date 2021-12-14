using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Hexen.GameSystem
{
    public class TileEventArgs : EventArgs
    {
        public HexTileView HexTileView { get; }

        public TileEventArgs(HexTileView hexTileView)
        {
            HexTileView = hexTileView;
        }
    }

    public class HexTileView : MonoBehaviour, IPointerClickHandler, IDropHandler
    {
        #region Properties

        public HexTile HexTile { get; set; }
        public static GameObject Model { get; set; }

        #endregion

        #region Fields

        public event EventHandler<TileEventArgs> Clicked;

        [SerializeField] private UnityEvent OnActivate;
        [SerializeField] private UnityEvent OnDeactivate;

        public bool Highlight
        {
            set
            {
                if (value)
                    OnActivate.Invoke();
                else
                    OnDeactivate.Invoke();
            }
        }

        #endregion


        #region Constructor

        public HexTileView(HexTile hexTile)
        {
            HexTile = hexTile;
        }

        #endregion

        #region Event Methods

        public void OnDrop(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick(new TileEventArgs(this));
        }

        #endregion
        

        protected virtual void OnClick(TileEventArgs tileEventArgs)
        {
            var handler = Clicked;
            handler?.Invoke(this, tileEventArgs);
        }
    }
}