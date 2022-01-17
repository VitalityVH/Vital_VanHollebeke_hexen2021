using System;
using System.Collections.Generic;
using Hexen.Plugins;
using UnityEngine;

namespace Hexen.BoardSystem
{
    public class Grid<THexTile>
    {
        #region Properties

        public readonly BidirectionalDictionary<THexTile, (float x, float y)> HexPositions
            = new BidirectionalDictionary<THexTile, (float xPos, float yPos)>();

        #endregion

        #region Fields

        public int Radius { get; }

        #endregion

        #region Constructor

        public Grid(int radius)
        {
            Radius = radius;
        }

        #endregion

        #region Methods

        public bool TryGetPositionAt(float x, float y, out THexTile hexTile)
            => HexPositions.TryGetKey((x, y), out hexTile);

        public bool TryGetCoordinateAt(THexTile hexTile, out (float x, float y) coordinate /*, out ... coordinate*/)
            => HexPositions.TryGetValue(hexTile, out coordinate);

        public void Register(THexTile hexTile, float x, float y)
        {
            HexPositions.Add(hexTile, (x,y));
        }

        public void Remove(THexTile hexTile)
        {
            if (!HexPositions.Remove(hexTile))
                return;
        }

        public BidirectionalDictionary<THexTile, (float xPos, float yPos)> AllHexPositions()
        {
            return HexPositions;
        }

        #endregion

    }
}