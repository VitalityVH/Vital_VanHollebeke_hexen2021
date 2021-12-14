using Hexen.HexenSystem.PlayableCards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hexen.GameSystem
{
    public class CardDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Properties

        public CardBase<HexTile> Card { get; set; }

        #endregion

        #region Construct
        // public CardDisplay(CardBase<HexTile> card)
        // {
        //     this.Card = card;
        // }

        #endregion

        #region Fields

        [SerializeField] private Canvas _canvas;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector3 _origin;


        #endregion

        
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _origin = this.transform.position;
            _canvasGroup = GetComponent<CanvasGroup>();

        }

        #region Event Methods

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = .6f;
            _canvasGroup.blocksRaycasts = false;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            this.transform.position = _origin;

            Debug.Log($"OnEndDrag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor ;
        }

        #endregion

        
    }
}