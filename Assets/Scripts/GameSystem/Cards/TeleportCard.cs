using System;
using System.Collections.Generic;
using Hexen.BoardSystem;
using Hexen.DeckSystem;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using Hexen.ReplaySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hexen.GameSystem.Cards
{
    public class TeleportCard : MonoBehaviour, ICard<HexTile>
    {
        #region Properties
        public Board<Capsule<HexTile>, HexTile> Board { get; set; }
        public Grid<HexTile> Grid { get; set; }
        public ReplayManager ReplayManager { get; set; }

        public PlayableCardName Type { get; set; }

        #endregion

        #region Fields

        [SerializeField] private Image _image;
        [SerializeField] private Text _title;
        [SerializeField] private Text _description;


        #endregion

        void Start()
        {
            _title.text = PlayableCardName.Teleport.ToString();
            _description.text = "Teleports the hero capsule to a available hexTile";
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public bool CanExecute(HexTile atPosition)
        {
            return Positions(atPosition).Contains(atPosition);
        }

        public void Execute(HexTile atPosition)
        {
            if (!Board.TryGetPosition(Board.HeroCapsule, out var oldPos))
                return;

            Action forward = () =>
            {
                Board.Teleport(atPosition);
                Board.HeroCapsule.TeleportTo(atPosition);
            };

            Action backward = () =>
            {
                Board.Teleport(oldPos);
                Board.HeroCapsule.TeleportTo(oldPos);
            };

            ReplayManager.Execute(new DelegateReplayCommand(forward, backward));
        }

        public List<HexTile> Positions(HexTile pos)
        {
            return new MovementHelper<HexTile>(Board,Grid)
                .ReturnAllHexTiles(MovementHelper<HexTile>.Empty)
                .CollectValidPositions();
        }

        public void ActivateLayoutGroup()
        {
            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
        }
    }

}