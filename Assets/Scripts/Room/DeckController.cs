using System.Collections;
using System.Collections.Generic;
using JsonClasses;
using UnityEngine;
using UserData;
using Random = UnityEngine.Random;

namespace Room {
    public class DeckController : MonoBehaviour {
        private Vector3 _deckScale;
        private Vector3 _positionHand;
        private Vector3 _targetPosition;

        private int _totalInitialCards;
        private List<int> _deckListCards;

        [SerializeField] private Vector3 rangeCardPosition;
        [SerializeField] private Vector3 positionShowPlayer;
        
        [SerializeField] private GameObject camObj;
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject cardPrefab;
        
        // animation variables
        [SerializeField] private float timeToShowPlayer;
        [SerializeField] private float dumbGetCard;

        private float _currentTimeToShowPlayer;
        private Camera _camera;

        private GameObject _tempCard;


        private Vector3 _minPosition;
        private Vector3 _maxPosition;
        private Vector3 _positionNextCard;

        private List<CardProperties> cards = new List<CardProperties>();

        private void Start() {
            _camera = camObj.GetComponent<Camera>();
            _deckListCards = UserDeck.getDeckCards();
            _deckScale = transform.localScale;
            _totalInitialCards = _deckListCards.Count;

            Vector3 handPosition = hand.transform.position;
            _minPosition = handPosition - rangeCardPosition;
            _maxPosition = handPosition + rangeCardPosition;
        }

        private void Update() {
            if (!ReferenceEquals(_tempCard, null)) {
                _currentTimeToShowPlayer += Time.deltaTime;
                if (_currentTimeToShowPlayer > timeToShowPlayer) {
                    _positionHand = _positionNextCard;
                    _targetPosition = _positionHand;
                }
                
                _tempCard.transform.position = Vector3.Lerp(_tempCard.transform.position, _targetPosition + new Vector3(0, 0.2f, 0), dumbGetCard * Time.deltaTime);
            }

            if (Input.touchCount == 1) {
                checkHit(_camera.ScreenPointToRay(Input.touches[0].position), false);
            }
            else if (Input.GetMouseButtonDown(0)) {
                checkHit(_camera.ScreenPointToRay(Input.mousePosition), true);
            }
        }

        private void checkHit(Ray ray, bool pc) {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.name == "Deck") {
                    if (pc) {
                        getCard();
                        return;
                    }
                    Touch screenTouch = Input.GetTouch(0);
                    if (screenTouch.phase == TouchPhase.Ended) {
                        getCard();
                    }
                }
            }
        }

        private void getCard() {
            if (_deckListCards.Count > 0 && cards.Count < 5) {
                int randCardIndex = Random.Range(0, _deckListCards.Count);
                int id = _deckListCards[randCardIndex];
                _deckListCards.RemoveAt(randCardIndex);

                _tempCard = Instantiate(cardPrefab, hand.transform);
                _tempCard.transform.rotation = Quaternion.Euler(15, 180, 0);
                CardProperties cardProperties = _tempCard.GetComponent<CardProperties>();
                cardProperties.cardId = id;
                cardProperties.setMaterial();

                resizeDeck();

                _targetPosition = positionShowPlayer;
                _currentTimeToShowPlayer = 0;
                addCard(_tempCard.GetComponent<CardProperties>());
            }
        }

        private void resizeDeck() {
            Vector3 newSize = transform.localScale;

            newSize.y = _deckListCards.Count * _deckScale.y / _totalInitialCards;
            transform.localScale = newSize;

            if (_deckListCards.Count == 0) {
                GetComponent<Renderer>().enabled = false;
            }
        }
        
        private void reorganizeCards(){
            for(int i = 1; i < cards.Count; i++){
                Vector3 position = calcDistanceHandPosition(i, cards.Count + 1);
                if(i-1 < cards.Count){
                    cards[i-1].transform.position = position;
                }
            }
        
            _positionNextCard = calcDistanceHandPosition(cards.Count, cards.Count + 1);
        }

        private Vector3 calcDistanceHandPosition(int indice, int limit) {
            float distance = indice / (float) limit;

            return Vector3.Lerp(_minPosition, _maxPosition, distance);
        }

        private void addCard(CardProperties card){
            cards.Add(card);
            reorganizeCards();
        }
    }
}