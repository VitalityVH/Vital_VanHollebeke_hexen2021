﻿using System;
using System.Collections.Generic;
using Hexen.BoardSystem;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.ReplaySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hexen.GameSystem.Cards
{
    public class PushCard : MonoBehaviour, ICard<HexTile>
    {
        #region Properties
        public Board<Capsule<HexTile>, HexTile> Board { get; set; }
        public Grid<HexTile> Grid { get; set; }
        public PlayableCardName Type { get; set; }
        #endregion

        #region Fields

        [SerializeField] private Image _image;
        [SerializeField] private Text _title;
        [SerializeField] private Text _description;

        #endregion

        void Start()
        {
            _title.text = PlayableCardName.Pushback.ToString();
            _description.text = "Slashes all enemies in a chosen direction";
        }
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        public bool CanExecute(HexTile atPosition)
        {
            return Positions(atPosition).Contains(atPosition);
        }

        public void Execute(HexTile atPosition, out Action forward, out Action backward)
        {
            forward = null;
            backward = null;

            var hitCapsules = new Dictionary<Capsule<HexTile>, HexTile>();
            var pushedCapsuleOldPositions = new Dictionary<Capsule<HexTile>, HexTile>();
            var pushedCapsuleNewPositions = new Dictionary<Capsule<HexTile>, HexTile>();

            forward = () =>
            {
                hitCapsules.Clear();
                pushedCapsuleOldPositions.Clear();
                pushedCapsuleNewPositions.Clear();

                foreach (var hexTile in Positions(atPosition))
                {
                    if (!Board.TryGetCapsule(hexTile, out var capsule)) continue;

                    Grid.TryGetCoordinateAt(hexTile, out var hexCoordinate);
                    Board.TryGetPosition(Board.HeroCapsule, out var heroHex);

                    Grid.TryGetCoordinateAt(heroHex, out var heroHexCoordinate);

                    (float x, float y) offSet = (hexCoordinate.x - heroHexCoordinate.x,
                        hexCoordinate.y - heroHexCoordinate.y);

                    Grid.TryGetPositionAt(hexCoordinate.x + offSet.x, hexCoordinate.y + offSet.y,
                        out var targetHexTile);

                    if (Grid.HexPositions.ContainsKey(targetHexTile)) //if there is a tile behind the pushed enemy
                    {
                        if (!Board.TryGetCapsule(targetHexTile, out _)) //and if there is no enemy behind
                        {
                            pushedCapsuleOldPositions.Add(capsule, hexTile);
                            pushedCapsuleNewPositions.Add(capsule, targetHexTile);

                            Board.Push(capsule, targetHexTile);
                            capsule.PushedTo(targetHexTile);
                        }
                    }
                    else //hit
                    {
                        hitCapsules.Add(capsule, hexTile);
                        Board.Hit(capsule);
                        capsule.HitFrom(hexTile);
                    }
                }
            };

            backward = () =>
            {
                foreach (var capsule in hitCapsules)
                {
                    capsule.Key.Reappear(atPosition);
                    Board.Place(capsule.Key, capsule.Value);
                }

                foreach (var capsule in pushedCapsuleOldPositions)
                {
                    Board.Push(capsule.Key, capsule.Value);
                    capsule.Key.PushedTo(capsule.Value);
                }
            };
        }
        private static int Mod(int x, int m) => (x % m + m) % m;
        public List<HexTile> Positions(HexTile hoveredTile)
        {
            List<HexTile> completeList = new List<HexTile>();

            for (int i = 0; i < MovementHelper<HexTile>.Offsets.Count; i++)
            {
                var list = new MovementHelper<HexTile>(Board, Grid).Collect(MovementHelper<HexTile>.Offsets[i].x,
                    MovementHelper<HexTile>.Offsets[i].y, 1).CollectValidPositions();
                if (list.Contains(hoveredTile))
                {
                    completeList.Clear();

                    completeList.AddRange(new MovementHelper<HexTile>(Board, Grid).Collect(MovementHelper<HexTile>.Offsets[Mod((i - 1), 6)].x,
                        MovementHelper<HexTile>.Offsets[Mod((i - 1), 6)].y, 1).CollectValidPositions());
                    completeList.AddRange(list);
                    completeList.AddRange(new MovementHelper<HexTile>(Board, Grid).Collect(MovementHelper<HexTile>.Offsets[Mod((i + 1), 6)].x,
                        MovementHelper<HexTile>.Offsets[Mod((i + 1), 6)].y, 1).CollectValidPositions());

                    return completeList;
                }
                else
                {
                    completeList.AddRange(list);
                }
            }
            return completeList;
        }
        public void ResetCard() => gameObject.GetComponent<CardBase>().ResetCard();
        public void Fade() => gameObject.GetComponent<CardBase>().Fade();
        
    }
}