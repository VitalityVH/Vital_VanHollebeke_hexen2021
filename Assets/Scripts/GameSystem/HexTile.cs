using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Hexen.GameSystem
{
    public class DropEventArgs : EventArgs
    {
        public HexTile HexTile { get; }

        public DropEventArgs(HexTile hexTile)
        {
            HexTile = hexTile;
        }
    }

    public class HexTile : MonoBehaviour, IDropHandler, HexenSystem.IPosition
    {
        #region Fields

        public int Q, R, S;
        
        public event EventHandler<DropEventArgs> Dropped;

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

        public HexTile(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
            if (q+r+s != 0) 
                Debug.Log($"Tile is not legit");
        }

        #endregion

        #region Coordinate Arithmetic Methods
        
        public HexTile Add(HexTile hex, HexTile hexAdd)
        {
            HexTile newHex = hex;
            newHex.Q += hexAdd.Q;
            newHex.R += hexAdd.R;
            newHex.S += hexAdd.S;

            return newHex;
        }
        
        public HexTile Subtract(HexTile hex, HexTile hexSub)
        {
            HexTile newHex = hex;
            newHex.Q -= hexSub.Q;
            newHex.R -= hexSub.R;
            newHex.S -= hexSub.S;

            return newHex;
        }
        
        public HexTile Scale(HexTile hex, int factor)
        {
            HexTile newHex = hex;
            newHex.Q *= factor;
            newHex.R *= factor;
            newHex.S *= factor;

            return newHex;
        }
        
        #endregion

        #region Distance Methods
        
        public int Length()
        {
            return (int)((Mathf.Abs(Q) + Mathf.Abs(R) + Mathf.Abs(S)) / 2);
        }
        
        public int Distance(HexTile hexA, HexTile hexB)
        {
            return Subtract(hexB , hexA).Length();
        }

        #endregion

        #region Neighbor Methods

        // public static List<(int q, int r, int s)> Directions = new List<(int q, int r, int s)>()
        // {
        //     (1,0,-1),
        //     (1, -1, 0),
        //     (0, -1, 1),
        //     (-1, 0, 1),
        //     (-1, 1, 0),
        //     (0, 1, -1)
        // };
        //
        // public static HexTile Direction(int direction)
        // {
        //     return Directions[direction];
        // }
        //
        //
        // public HexTile Neighbor(HexTile hex, int direction)
        // {
        //     return Add(hex,HexTile.Direction(direction));
        // }
        //
        // public static List<HexTile> Diagonals = new List<HexTile>
        // {
        //     new HexTile(2, -1, -1),
        //     new HexTile(1, -2, 1),
        //     new HexTile(-1, -1, 2),
        //     new HexTile(-2, 1, 1),
        //     new HexTile(-1, 2, -1),
        //     new HexTile(1, 1, -2)
        // };
        //
        // public HexTile DiagonalNeighbor(HexTile hex,int direction)
        // {
        //     return Add(hex, HexTile.Diagonals[direction]);
        // }
        
        #endregion

        #region Event Methods

        public void OnDrop(PointerEventData eventData)
        {
            OnCardDrop(new DropEventArgs(this));
        }

        #endregion

        protected virtual void OnCardDrop(DropEventArgs tileEventArgs)
        {
            var handler = Dropped;
            handler?.Invoke(this, tileEventArgs);
        }
    }
}