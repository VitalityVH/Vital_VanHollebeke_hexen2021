using System;
using Hexen.HexenSystem;
using Hexen.HexenSystem.PlayableCards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Hexen.GameSystem
{
    public class CardEventArgs : EventArgs
    {
        public CardBase<HexTile> Card { get; }

        public CardEventArgs(CardBase<HexTile> card)
        {
            Card = card;
        }
    }
    public class DragEventArgs : EventArgs
    {
        public HexTile HexTile { get; }

        public DragEventArgs(HexTile hexTile)
        {
            HexTile = hexTile;
        }
    }

    public class CardDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Properties

        public CardBase<HexTile> Card { get; set; }
        public Canvas Canvas { get; set; }

        #endregion

        public event EventHandler<PointerEventData> Dragging;
        public event EventHandler<CardEventArgs> UsedCard;

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

            _title.text = Card.PlayableCardName.ToString();
            _description.text = Card.Description;

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
            OnCardUse(new CardEventArgs(this.Card));
        }

        protected virtual void OnDragging(PointerEventData e)
        {
            var handler = Dragging;
            handler?.Invoke(this, e);
        }

        protected virtual void OnCardUse(CardEventArgs e)
        {
            var handler = UsedCard;
            handler?.Invoke(this, e);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragging(eventData);
            _rectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor ;
        }

        #endregion

        
    }

}