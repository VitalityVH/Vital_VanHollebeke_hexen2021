using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hexen.GameSystem.Cards
{
    public class CardBase : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Fields

        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector3 _origin;

        #endregion

        #region MonoBehaviour Methods

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _origin = this.transform.position;
            _canvas = FindObjectOfType<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion
        
        #region Event Methods

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = .6f;
            _canvasGroup.blocksRaycasts = false;
            GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        }

        public void OnEndDrag(PointerEventData eventData) => ResetCard();
        

        public void ResetCard()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            this.transform.position = _origin;
            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
        }
        
        public void Fade()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = .6f;
        }

        public void OnDrag(PointerEventData eventData) => _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        #endregion
    }
}