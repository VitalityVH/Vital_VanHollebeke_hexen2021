using System;
using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.HexenSystem;
using Hexen.SelectionSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hexen.GameSystem
{
    public class GameLoop : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _hexTileModel;
        [SerializeField] private GameObject _enemyModel;
        [SerializeField] private GameObject _heroModel;
        [SerializeField] private PositionHelper _positionHelper;

        private Grid<HexTile> _grid;
        private Board<Capsule<HexTile>, HexTile> _board;
        private SelectionManager<HexTileView> _selectionManager;

        #endregion

        #region Methods

        void Start()
        {
            _grid = new Grid<HexTile>(3);
            _board = new Board<Capsule<HexTile>, HexTile>();

            _selectionManager = new SelectionManager<HexTileView>();

            _selectionManager.Selected += (source, eventArgs) =>
            {
                _grid.TryGetCoordinateAt(_selectionManager.SelectedItem.HexTile, out var coordinate);
                Debug.Log(coordinate);
                // _selectionManager.SelectedItem.Highlight = true;
            };

            // _selectionManager.Deselected += (source, eventArgs) =>
            // {
            //     _selectionManager.SelectedItem.Highlight = false;
            // };

            GenerateGrid();
            GenerateHero();
            GenerateEnemies();
        }

        public void DeselectAll()
            => _selectionManager.DeselectAll();

        private void GenerateGrid()
        {
            List<HexTile> hexTiles = new List<HexTile>();

            for (int q = -_grid.Radius; q <= _grid.Radius; q++)
            {
                int r1 = Mathf.Max(-_grid.Radius, -q - _grid.Radius);
                int r2 = Mathf.Min(_grid.Radius, -q + _grid.Radius);

                for (int r = r1; r <= r2; r++)
                {
                    var newHexTile = new HexTile(q, r, -q - r);
                    hexTiles.Add(newHexTile);
                }
            }

            foreach (var hexTile in hexTiles)
            {
                Vector3 worldPos = _positionHelper.HexTileToWorldPos(hexTile);
                var _hexTileView = Instantiate(_hexTileModel, worldPos, Quaternion.identity,
                    transform);
                _hexTileView.GetComponent<HexTileView>().HexTile = hexTile;
                _hexTileView.GetComponent<HexTileView>().Clicked += (source, eventArgs) => Select(eventArgs.HexTileView);

                _grid.Register(hexTile, worldPos.x, worldPos.z);
            }

        }

        private void GenerateHero()
        {
            if (_grid.TryGetPositionAt(0,0, out var middleHexTile))
            {
                Capsule<HexTile> capsule = new Capsule<HexTile>(CapsuleType.Hero, middleHexTile);

                Vector3 worldPos = _positionHelper.HexTileToWorldPos(capsule.HexTile);
                var heroView = Instantiate(_heroModel, worldPos, Quaternion.identity, transform);
                heroView.GetComponent<CapsuleView>().Model = capsule;

                _board.Place(capsule, capsule.HexTile);
            }
        }

        private void GenerateEnemies()
        {
            List<Capsule<HexTile>> enemies = new List<Capsule<HexTile>>();
            HexTileView[] hexTileViews = FindObjectsOfType<HexTileView>();

            foreach (var hexTileView in hexTileViews)
            {
                int random = 1;

                if (_grid.TryGetPositionAt(0,0 , out var middleHexTile))
                {
                    if (hexTileView.HexTile != middleHexTile && Random.Range(0,3) == random)
                    {
                        Capsule<HexTile> capsule = new Capsule<HexTile>(CapsuleType.Enemy, hexTileView.HexTile);
                        enemies.Add(capsule);
                    }
                }
            }

            foreach (var enemy in enemies)
            {
                Vector3 worldPos = _positionHelper.HexTileToWorldPos(enemy.HexTile);
                var _enemyView = Instantiate(_enemyModel, worldPos, Quaternion.identity, transform);
                _enemyView.GetComponent<CapsuleView>().Model = enemy;

                _board.Place(enemy, enemy.HexTile);
            }
        }

        private void Select(HexTileView hexTileView)
        {
            DeselectAll();
            _selectionManager.Select(hexTileView);
        }

        #endregion

    }
}