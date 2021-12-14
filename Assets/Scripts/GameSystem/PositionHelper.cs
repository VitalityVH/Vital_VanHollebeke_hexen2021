using Hexen.BoardSystem;
using UnityEngine;

namespace Hexen.GameSystem
{
    [CreateAssetMenu(menuName = "Hexen/PositionHelper")]
    public class PositionHelper : ScriptableObject
    {
        #region Fields

        [SerializeField] private float _tileDimension;

        #endregion
        private void OnValidate()
        {
            if (_tileDimension <= 0)
                _tileDimension = 1;
        }

        #region Methods
        public Vector3 HexTileToWorldPos(HexTile hex)
        {
            float x = _tileDimension * Mathf.Sqrt(3) * hex.Q + Mathf.Sqrt(3) / 2 * hex.R;
            float y = _tileDimension * 3 / 2 * hex.R;

            return new Vector3(x,0,y);
        }

        // public (int x, int y) WorldToGridPosition(Board<IPlayableCard, IPosition> board ,Grid<HexTile> grid, Vector3 worldPos)
        // {
        //     var scaledWorldPos = worldPos / _tileDimension;
        //     return default;
        // }
        #endregion
    }
}