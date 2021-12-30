using System;
using System.Collections.Generic;
using System.Linq;
using Hexen.BoardSystem;
using Hexen.DeckSystem;
using Hexen.GameSystem.Cards;
using Hexen.GameSystem.GameStates;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.ReplaySystem;
using Hexen.SelectionSystem;
using Hexen.StateSystem;
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

        [SerializeField] private GameObject _deckHandParent;

        [SerializeField] private GameObject _teleportCard;
        [SerializeField] private GameObject _swipeCard;
        [SerializeField] private GameObject _slashCard;
        [SerializeField] private GameObject _pushCard;

        [SerializeField] private PositionHelper _positionHelper;

        [SerializeField] private int _fieldRadius = 5;
        [SerializeField] private int _deckSize = 13;


        private StateMachine<GameStateBase> _gameStateMachine;
        #endregion

        #region Methods

        void Start()
        {
            var grid = new Grid<HexTile>(_fieldRadius);
            var board = new Board<Capsule<HexTile>, HexTile>();
            var deckManager = new DeckManager<ICard<HexTile>, HexTile>();

            var replayManager = new ReplayManager();

            _gameStateMachine = new StateMachine<GameStateBase>();

            _gameStateMachine.Register(GameStateBase.PlayingState, new PlayingGameState(_gameStateMachine, board, grid, deckManager, replayManager));
            _gameStateMachine.Register(GameStateBase.ReplayingState, new ReplayGameState(_gameStateMachine, replayManager));
            _gameStateMachine.InitialState = GameStateBase.PlayingState;

            GenerateGrid(grid);
            GenerateHero(board, grid);
            GenerateEnemies(board, grid);
            PopulateDeck(board, grid, deckManager, replayManager);
            PopulateHand(deckManager);

        }
        private void GenerateGrid(Grid<HexTile> grid)
        {
            for (int q = -grid.Radius; q <= grid.Radius; q++)
            {
                int r1 = Mathf.Max(-grid.Radius, -q - grid.Radius);
                int r2 = Mathf.Min(grid.Radius, -q + grid.Radius);

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

                    grid.Register(newHexTile, q,r);
                }
            }
        }
        private void GenerateHero(Board<Capsule<HexTile>, HexTile> board, Grid<HexTile> grid)
        {
            if (grid.TryGetPositionAt(0,0, out var middleHexTile))
            {
                Vector3 worldPos = _positionHelper.HexTileToWorldPos((middleHexTile.Q, middleHexTile.R));
                var newHeroCapsuleObject = Instantiate(_heroModel, worldPos, Quaternion.identity, transform);
                var newHeroCapsuleView = newHeroCapsuleObject.GetComponent<CapsuleView>();
                Capsule<HexTile> newHeroCapsule = new Capsule<HexTile>();

                newHeroCapsuleView.Model = newHeroCapsule;

                newHeroCapsule.HexTile = middleHexTile;
                newHeroCapsule.CapsuleType = CapsuleType.Hero;

                board.HeroCapsule = newHeroCapsule;

                board.Place(newHeroCapsule, newHeroCapsule.HexTile);
            }
        }
        private void GenerateEnemies(Board<Capsule<HexTile>, HexTile> board, Grid<HexTile> grid)
        {
            HexTile[] hexTiles = FindObjectsOfType<HexTile>();
            foreach (var hexTile in hexTiles)
            {
                int random = 1;

                if (grid.TryGetPositionAt(0,0 , out var middleHexTile))
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

                        board.Place(newEnemyCapsule, hexTile);
                    }
                }
            }
        }
        private void PopulateDeck(Board<Capsule<HexTile>, HexTile> board, Grid<HexTile> grid, DeckManager<ICard<HexTile>, HexTile> deckManager, ReplayManager replayManager)
        {
            for (int i = 0; i < _deckSize; i++)
            {
                GenerateCard(board, grid, deckManager, replayManager);
            }
        }
        private void GenerateCard(Board<Capsule<HexTile>, HexTile> board, Grid<HexTile> grid, DeckManager<ICard<HexTile>, HexTile> deckManager, ReplayManager replayManager)
        {
            switch (Random.Range(1, 5))
            {
                case 1:
                    var newTeleportCard = Instantiate(_teleportCard, _deckHandParent.transform);
                    TeleportCard teleportCard = newTeleportCard.GetComponent<TeleportCard>();

                    teleportCard.Type = PlayableCardName.Teleport;
                    teleportCard.Board = board;
                    teleportCard.Grid = grid;
                    teleportCard.ReplayManager = replayManager;
                    deckManager.Register(teleportCard);
                    break;
                case 2:
                    var newSlashCard = Instantiate(_slashCard, _deckHandParent.transform);
                    SlashCard slashCard = newSlashCard.GetComponent<SlashCard>();
                    slashCard.Type = PlayableCardName.Slash;
                    slashCard.Board = board;
                    slashCard.Grid = grid;
                    slashCard.ReplayManager = replayManager;
                    deckManager.Register(slashCard);
                    break;
                case 3:
                    var newSwipeCard = Instantiate(_swipeCard, _deckHandParent.transform);
                    SwipeCard swipeCard = newSwipeCard.GetComponent<SwipeCard>();
                    swipeCard.Type = PlayableCardName.Swipe;
                    swipeCard.Board = board;
                    swipeCard.Grid = grid;
                    swipeCard.ReplayManager = replayManager;
                    deckManager.Register(swipeCard);
                    break;
                case 4:
                    var newPushCard = Instantiate(_pushCard, _deckHandParent.transform);
                    PushCard pushCard = newPushCard.GetComponent<PushCard>();
                    pushCard.Type = PlayableCardName.Pushback;
                    pushCard.Board = board;
                    pushCard.Grid = grid;
                    pushCard.ReplayManager = replayManager;
                    deckManager.Register(pushCard);
                    break;
            }
        }
        private void PopulateHand(DeckManager<ICard<HexTile>, HexTile> deckManager)
        {
            deckManager.FillHand();
        }

        private void Select(ICard<HexTile> card, HexTile hexTile)
            => _gameStateMachine.CurrentState.Select(card, hexTile);

        private void Deselect(ICard<HexTile> card, HexTile hexTile)
            => _gameStateMachine.CurrentState.Deselect(card, hexTile);

        private void Play(ICard<HexTile> card, HexTile hexTile)
            => _gameStateMachine.CurrentState.Play(card, hexTile);


        public void Backward()
            => _gameStateMachine.CurrentState.Backward();

        public void Forward()
            => _gameStateMachine.CurrentState.Forward();

        #endregion

    }
}