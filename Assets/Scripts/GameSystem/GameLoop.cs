using System;
using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.SelectionSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Hexen.GameSystem
{
    public class GameLoop : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject _boardParent;
        [SerializeField] private GameObject _hexTileModel;
        [SerializeField] private GameObject _enemyModel;
        [SerializeField] private GameObject _heroModel;
        [SerializeField] private GameObject _deckHand;
        [SerializeField] private GameObject _cardDisplay;

        [SerializeField] private PositionHelper _positionHelper;


        [SerializeField] private int _deckSize = 13;


        private Grid<HexTile> _grid;
        private Board<Capsule<HexTile>, HexTile> _board;
        private SelectionManager<HexTile> _selectionManager;
        private MoveManager<HexTile> _moveManager;
        

        #endregion

        #region Methods

        void Start()
        {
            _grid = new Grid<HexTile>(3);
            _board = new Board<Capsule<HexTile>, HexTile>();
            _moveManager = new MoveManager<HexTile>(_board, _grid);
            _selectionManager = new SelectionManager<HexTile>();

            _selectionManager.Selected += (source, eventArgs) =>
            {
                _grid.TryGetCoordinateAt(_selectionManager.SelectedItem, out var coordinate);
                // _board.Teleport(_selectionManager.SelectedItem);

                _selectionManager.SelectedItem.Highlight = true;
            };

            _selectionManager.Deselected += (source, eventArgs) =>
            {
                eventArgs.SelectionItem.Highlight = false;
            };

            _board.CapsuleTeleported += (source, eventArgs) =>
            {
                
            };

            GenerateGrid();
            GenerateHero();
            GenerateEnemies();
            GenerateHand(PopulateDeck());
        }

        private void GenerateGrid()
        {
            List<HexTile> hexTiles = new List<HexTile>();

            for (int q = -_grid.Radius; q <= _grid.Radius; q++)
            {
                int r1 = Mathf.Max(-_grid.Radius, -q - _grid.Radius);
                int r2 = Mathf.Min(_grid.Radius, -q + _grid.Radius);

                for (int r = r1; r <= r2; r++)
                {
                    Vector3 WorldPos = _positionHelper.HexTileToWorldPos((q,r));
                    var newHexTileObject = Instantiate(_hexTileModel, WorldPos, Quaternion.identity, _boardParent.transform);
                    var newHexTile = newHexTileObject.GetComponent<HexTile>();

                    newHexTile.Q = q;
                    newHexTile.R = r;
                    newHexTile.S = -q - r;

                    newHexTile.CardEntered += (source, EventArgs) => Select(EventArgs.HexTile);
                    newHexTile.CardExited += (source, EventArgs) => Deselect(EventArgs.HexTile);

                    newHexTile.Dropped += (source, eventArgs)
                        => Select(eventArgs.HexTile);

                    _grid.Register(newHexTile, q,r);
                }

            }

            // foreach (var hexTile in hexTiles)
            // {
            //     Vector3 worldPos = _positionHelper.HexTileToWorldPos(hexTile);
            //     var _hexTileView = Instantiate(_hexTileModel, worldPos, Quaternion.identity,
            //         transform);
            //     _hexTileView.GetComponent<HexTileView>().HexTile = hexTile;
            //     _hexTileView.GetComponent<HexTileView>().Dropped += (source, eventArgs) 
            //         => Select(eventArgs.HexTileView);
            //
            //     _grid.Register(hexTile, hexTile.Q, hexTile.R);
            // }

        }

        private void GenerateHero()
        {
            if (_grid.TryGetPositionAt(0,0, out var middleHexTile))
            {
                Vector3 worldPos = _positionHelper.HexTileToWorldPos((middleHexTile.Q, middleHexTile.R));
                var newHeroCapsuleObject = Instantiate(_heroModel, worldPos, Quaternion.identity, transform);
                var newHeroCapsuleView = newHeroCapsuleObject.GetComponent<CapsuleView>();
                Capsule<HexTile> newHeroCapsule = new Capsule<HexTile>();

                newHeroCapsuleView.Model = newHeroCapsule;
                newHeroCapsule.HexTile = middleHexTile;
                newHeroCapsule.CapsuleType = CapsuleType.Hero;
                //
                // newHeroCapsule.Teleported += (source, eventArgs)
                //     => Teleport(eventArgs.Position);

                _board.HeroCapsule = newHeroCapsule;
                _board.Place(newHeroCapsule, newHeroCapsule.HexTile);
            }
        }

        private void GenerateEnemies()
        {
            HexTile[] hexTiles = FindObjectsOfType<HexTile>();
            foreach (var hexTile in hexTiles)
            {
                int random = 1;

                if (_grid.TryGetPositionAt(0,0 , out var middleHexTile))
                {
                    if (hexTile != middleHexTile && Random.Range(0,3) == random)
                    {
                        Vector3 worldPos = _positionHelper.HexTileToWorldPos((hexTile.Q, hexTile.R));
                        var newEnemyCapsuleObject = Instantiate(_enemyModel, worldPos, Quaternion.identity, transform);
                        var newEnemyCapsuleView = newEnemyCapsuleObject.GetComponent<CapsuleView>();
                        var newEnemyCapsule = new Capsule<HexTile>();

                        newEnemyCapsuleView.Model = newEnemyCapsule;
                        newEnemyCapsule.HexTile = hexTile;
                        newEnemyCapsule.CapsuleType = CapsuleType.Enemy;

                        _board.Place(newEnemyCapsule, hexTile);
                    }
                }
            }
        }

        private List<ICard<HexTile>> PopulateDeck()
        {
            List<ICard<HexTile>> deckCards = new List<ICard<HexTile>>();

            for (int i = 0; i < _deckSize; i++)
            {
                // deckCards.Add(GenerateCard());
            }

            return deckCards;
        }

        private void GenerateHand(List<ICard<HexTile>> deckCards)
        {
            for (int i = 0; i < 5; i++)
            {
                // var newCardDisplay = Instantiate(_cardDisplay, _deckHand.transform);
                // newCardDisplay.GetComponent<CardDisplay>().Canvas = FindObjectOfType<Canvas>();
                // newCardDisplay.GetComponent<CardDisplay>().Card = deckCards[_deckSize-1];
                //
                // newCardDisplay.GetComponent<CardDisplay>().Dragging += (source, args) =>
                // {
                //     // if (args.pointerCurrentRaycast.gameObject.GetComponentInParent<HexTile>() != null)
                //     // {
                //     //     Select(args.pointerCurrentRaycast.gameObject.GetComponentInParent<HexTile>());
                //     // }
                //     
                // };
                //
                // newCardDisplay.GetComponent<CardDisplay>().UsedCard += (source, args) =>
                // {
                //     Debug.Log($"Used {args.Card.PlayableCardName}");
                // };
                //
                // _deckSize--;
                //
                // Debug.Log(newCardDisplay.GetComponent<CardDisplay>().Card.PlayableCardName);
            }
        }

        // private ICard<HexTile> GenerateCard()
        // {
        //     switch (Random.Range(1, 5))
        //     {
        //         case 1:
        //             return new TeleportCard<HexTile>(_board, _grid);
        //         case 2:
        //             return new SlashCard<HexTile>(_board, _grid);
        //         case 3:
        //             return new SwipeCard<HexTile>(_board, _grid);
        //         case 4:
        //             return new PushCard<HexTile>(_board, _grid);
        //     }
        //     return null;
        // }

        private void Select(HexTile hexTile)
        {
            _selectionManager.Select(hexTile);
        }

        private void Deselect(HexTile hexTile)
        {
            _selectionManager.Deselect(hexTile);
        }

        // private void Teleport(HexTile hexTile)
        // {
        //     _selectionManager.Select(hexTile);
        // }
        #endregion

    }
}