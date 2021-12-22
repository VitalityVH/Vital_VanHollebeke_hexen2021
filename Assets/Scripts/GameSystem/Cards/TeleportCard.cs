using System;
using System.Collections.Generic;
using Hexen.BoardSystem;
using Hexen.DeckSystem;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hexen.GameSystem.Cards
{
    public class TeleportCard : MonoBehaviour, ICard<HexTile>, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Properties
        public Canvas Canvas { get; set; }
        public Board<Capsule<HexTile>, HexTile> Board { get; set; }
        public Grid<HexTile> Grid { get; set; }

        #endregion

        public EventHandler<CardEventArgs<ICard<HexTile>>> Played;

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
            this.gameObject.SetActive(false);

            _rectTransform = GetComponent<RectTransform>();
            _origin = this.transform.position;
            _canvasGroup = GetComponent<CanvasGroup>();

            _title.text = PlayableCardName.Teleport.ToString();
            _description.text = "Teleports the hero capsule to a available hexTile";

        }

        public bool CanExecute()
        {
            throw new NotImplementedException();
        }

        public void Execute(HexTile atPosition)
        {
            throw new NotImplementedException();
        }

        public List<HexTile> Positions()
        {
            throw new NotImplementedException();
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

        #endregion
    }

}