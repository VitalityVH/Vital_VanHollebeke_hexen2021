using System;
using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.DeckSystem;
using Hexen.GameSystem.Cards;
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
        [SerializeField] private GameObject _teleportCard;
        [SerializeField] private GameObject _swipeCard;
        [SerializeField] private GameObject _slashCard;
        [SerializeField] private GameObject _pushCard;

        [SerializeField] private PositionHelper _positionHelper;

        [SerializeField] private int _fieldRadius = 5;
        [SerializeField] private int _deckSize = 13;


        private Grid<HexTile> _grid;
        private Board<Capsule<HexTile>, HexTile> _board;
        private SelectionManager<HexTile> _selectionManager;
        private DeckManager<ICard<HexTile>, HexTile> _deckManager;
        

        #endregion

        #region Methods

        void Start()
        {
            _grid = new Grid<HexTile>(_fieldRadius);
            _board = new Board<Capsule<HexTile>, HexTile>();
            _selectionManager = new SelectionManager<HexTile>();
            _deckManager = new DeckManager<ICard<HexTile>,HexTile>();
            

            _selectionManager.Selected += (source, eventArgs) =>
            {
                eventArgs.SelectionItem.Highlight = true;
            };

            _selectionManager.Deselected += (source, eventArgs) =>
            {
                eventArgs.SelectionItem.Highlight = false;
            };

            _deckManager.PlayCard += (source, eventArgs) =>
            {
                DeselectAll();
                eventArgs.Card.ActivateLayoutGroup();
                eventArgs.Card.SetActive(false);
                _deckManager.FillHand();
            };

            GenerateGrid();
            GenerateHero();
            GenerateEnemies();
            PopulateDeck();
            GetHand();
        }

        private void DeselectAll()
        {
            _selectionManager.DeselectAll();
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

                    newHexTile.CardEntered += (source, EventArgs) => Select(EventArgs.Card,EventArgs.HexTile);
                    newHexTile.CardExited += (source, EventArgs) => Deselect(EventArgs.Card, EventArgs.HexTile);

                    newHexTile.Dropped += (source, eventArgs) => Play(eventArgs.Card, eventArgs.HexTile);

                    _grid.Register(newHexTile, q,r);
                }
            }
        }

        private void Play(ICard<HexTile> eventArgsCard, HexTile eventArgsHexTile)
        {
            _deckManager.Play(eventArgsCard, eventArgsHexTile);
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

        private void PopulateDeck()
        {
            for (int i = 0; i < _deckSize; i++)
            {
                GenerateCard();
            }
        }
        
        private void GenerateCard()
        {
            switch (Random.Range(1, 5))
            {
                case 1:
                    var newTeleportCard = Instantiate(_teleportCard, _deckHand.transform);
                    TeleportCard teleportCard = newTeleportCard.GetComponent<TeleportCard>();
                    teleportCard.Type = PlayableCardName.Teleport;
                    teleportCard.Board = _board;
                    teleportCard.Grid = _grid;
                    _deckManager.Register(teleportCard);
                    break;
                case 2:
                    var newSlashCard = Instantiate(_slashCard, _deckHand.transform);
                    SlashCard slashCard = newSlashCard.GetComponent<SlashCard>();
                    slashCard.Type = PlayableCardName.Slash;
                    slashCard.Board = _board;
                    slashCard.Grid = _grid;
                    _deckManager.Register(slashCard);
                    break;
                case 3:
                    var newSwipeCard = Instantiate(_swipeCard, _deckHand.transform);
                    SwipeCard swipeCard = newSwipeCard.GetComponent<SwipeCard>();
                    swipeCard.Type = PlayableCardName.Swipe;
                    swipeCard.Board = _board;
                    swipeCard.Grid = _grid;
                    _deckManager.Register(swipeCard);
                    break;
                case 4:
                    var newPushCard = Instantiate(_pushCard, _deckHand.transform);
                    PushCard pushCard = newPushCard.GetComponent<PushCard>();
                    pushCard.Type = PlayableCardName.Pushback;
                    pushCard.Board = _board;
                    pushCard.Grid = _grid;
                    _deckManager.Register(pushCard);
                    break;
            }
        }


        private void GetHand()
        {
            _deckManager.FillHand();
        }

        private void Select(ICard<HexTile> card,HexTile hexTile)
        {
            if (card.Type == PlayableCardName.Teleport && card.Positions(hexTile).Contains(hexTile))
            {
                _selectionManager.Select(hexTile);
            }
            else
            {
                foreach (var validHexTile in card.Positions(hexTile))
                {
                    _selectionManager.Select(validHexTile);
                    // validHexTile.Highlight = true;
                }
            }
            
        }

        private void Deselect(ICard<HexTile> card, HexTile hexTile)
        {
            foreach (var validHexTile in card.Positions(hexTile))
            {
                _selectionManager.Deselect(validHexTile);
            }
            //_selectionManager.Deselect(hexTile);
        }
        
        #endregion

    }
}