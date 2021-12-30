using Hexen.BoardSystem;
using Hexen.HexenSystem;
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
        public Vector3 HexTileToWorldPos((int q, int r) hex)
        {
            float x = _tileDimension * Mathf.Sqrt(3) * hex.q + Mathf.Sqrt(3) / 2 * hex.r;
            float y = _tileDimension * 3 / 2 * hex.r;

            return new Vector3(x,0,y);
        }
        #endregion
    }
}