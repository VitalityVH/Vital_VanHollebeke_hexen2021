using System.Collections.Generic;
using Hexen.BoardSystem;
using Hexen.GameSystem;
using Hexen.HexenSystem.PlayableCards;
using UnityEngine;

namespace Hexen.HexenSystem
{
    public class CardDeck<TPosition> : MonoBehaviour
    {
        [SerializeField] private GameObject _cardDisplay;
        [SerializeField] private int _deckSize = 13;
        
        private List<PlayableCardName> _cardNames;
        private Board<Capsule<TPosition>, TPosition> _board;
        private Grid<TPosition> _grid;

        void Start()
        {
            PopulateDeck();
            GenerateHand();
        }

        private void PopulateDeck()
        {
            for (int i = 0; i < _deckSize; i++)
            {
                GenerateCard();
            }
        }

        private void GenerateHand()
        {
            
        }

        private void GenerateCard()
        {
            int random = Random.Range(1, 4);

            switch (random)
            {
                case 1:
                    // var newCard = new TeleportCard<TPosition>();

                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }

            
            _cardDisplay.GetComponent<CardDisplay>();
        }
    }
}