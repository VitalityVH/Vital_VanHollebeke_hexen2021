using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Hexen.GameSystem
{

    public class HexTile
    {
        #region Fields

        public readonly int Q, R, S;

        #endregion

        #region Constructor

        public HexTile(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
            if (q + r + s != 0) throw new ArgumentException("Hex is not legit");
        }

        #endregion

        #region Coordinate Arithmetic Methods

        public HexTile Add(HexTile hex)
        {
            return new HexTile(Q + hex.Q, R + hex.R, S + hex.S);
        }

        public HexTile Subtract(HexTile hex)
        {
            return new HexTile(Q - hex.Q, R - hex.R, S - hex.S);
        }

        public HexTile Scale(HexTile hex, int factor)
        {
            return new HexTile(hex.Q * factor, hex.R * factor, hex.S * factor);
        }

        #endregion

        #region Distance Methods

        public int Length()
        {
            return (int)((Mathf.Abs(Q) + Mathf.Abs(R) + Mathf.Abs(S)) / 2);
        }

        public int Distance(HexTile hexB)
        {
            return Subtract(hexB).Length();
        }

        #endregion

        #region Neighbor Methods

        public static List<HexTile> Directions = new List<HexTile>
        {
            new HexTile(1, 0, -1), 
            new HexTile(1, -1, 0), 
            new HexTile(0, -1, 1), 
            new HexTile(-1, 0, 1), 
            new HexTile(-1, 1, 0), 
            new HexTile(0, 1, -1)
        };

        public static HexTile Direction(int direction)
        {
            return HexTile.Directions[direction];
        }


        public HexTile Neighbor(int direction)
        {
            return Add(HexTile.Direction(direction));
        }

        public static List<HexTile> Diagonals = new List<HexTile>
        {
            new HexTile(2, -1, -1), 
            new HexTile(1, -2, 1), 
            new HexTile(-1, -1, 2), 
            new HexTile(-2, 1, 1), 
            new HexTile(-1, 2, -1), 
            new HexTile(1, 1, -2)
        };

        public HexTile DiagonalNeighbor(int direction)
        {
            return Add(HexTile.Diagonals[direction]);
        }

        #endregion
        
    }
}