using System;
using System.Collections.Generic;
using Hexen.BoardSystem;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hexen.GameSystem.Cards 
{

    public class SlashCard : MonoBehaviour, ICard<HexTile>, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Properties
        public Canvas Canvas { get; set; }
        public Board<Capsule<HexTile>, HexTile> Board { get; set; }
        public Grid<HexTile> Grid { get; set; }

        public PlayableCardName Type { get; set; }
        #endregion

        #region Fields

        [SerializeField] private Image _image;
        [SerializeField] private Text _title;
        [SerializeField] private Text _description;


        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector3 _origin;


        #endregion

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _origin = this.transform.position;
            _canvasGroup = GetComponent<CanvasGroup>();

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

        public bool Execute(HexTile atPosition)
        {
            if (CanExecute(atPosition))
            {
                foreach (var hexTile in Positions(atPosition))
                {
                    if (Board.TryGetCapsule(hexTile, out var capsule))
                    {
                        Board.Hit(capsule);
                        capsule.HitFrom(hexTile);
                    }
                }
                return true;
            }
            return false;
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

        #region Event Methods

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = .6f;
            _canvasGroup.blocksRaycasts = false;
            GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            this.transform.position = _origin;
            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
        }

        public void ActivateLayoutGroup()
        {
            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
        }
        #endregion
    }
}