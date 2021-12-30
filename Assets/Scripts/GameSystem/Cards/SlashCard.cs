using System;
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

    public class SlashCard : MonoBehaviour, ICard<HexTile>
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
            _title.text = PlayableCardName.Slash.ToString();
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

        public void Execute(HexTile atPosition)
        {
            var hitCapsules = new Dictionary<Capsule<HexTile>, HexTile>();

            Action forward = () =>
            {
                hitCapsules.Clear();

                foreach (var hexTile in Positions(atPosition))
                {
                    if (Board.TryGetCapsule(hexTile, out var capsule))
                    {
                        hitCapsules.Add(capsule, hexTile);
                        Board.Hit(capsule);
                        capsule.HitFrom(hexTile);
                    }
                }
            };

            Action backward = () =>
            {
                foreach (var capsule in hitCapsules)
                {
                    capsule.Key.Reappear(atPosition);
                    Board.Place(capsule.Key, capsule.Value);
                }
            };

            ReplayManager.Execute(new DelegateReplayCommand(forward, backward));
        }

        public List<HexTile> Positions(HexTile hoveredTile)
        {
            List<HexTile> completeList = new List<HexTile>();

            foreach(var offset in MovementHelper<HexTile>.Offsets)
            {
                var list = new MovementHelper<HexTile>(Board, Grid).Collect(offset.x, offset.y).CollectValidPositions();
                if (list.Contains(hoveredTile))
                    return list;
                else
                    completeList.AddRange(list);
            }

            return completeList;
        }

        public void ActivateLayoutGroup()
        {
            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
        }
    }
}