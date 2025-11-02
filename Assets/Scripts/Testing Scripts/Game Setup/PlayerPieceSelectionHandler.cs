using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerPieceSelectionHandler : MonoBehaviour
    {
        // by default, they are all active (because in the editor their add buttons are visible)
        [SerializeField][ReadOnly] private bool _isActive = true;

        [SerializeField] private int _playerID;

        [Header("Slots")]
        [SerializeField] private Image[] _buttonBackgroundImages;
        [SerializeField] private Image[] _selectedPieceImages;
        [SerializeField] private GameObject[] _plusButtonObjects;

        [Header("Player panel visuals")]
        [SerializeField] private Image _playerPanelImage;

        [SerializeField] private GameObject[] _objectsShownWhileActive;
        [SerializeField] private TMP_Text _playerNumberText;

        [SerializeField] private Color _activePanelImageColor;
        [SerializeField] private Color _inactivePanelImageColor;

        [SerializeField] private Color _activeTextColor;
        [SerializeField] private Color _inactiveTextColor;

        [SerializeField] private float _activeTextHeight;
        [SerializeField] private float _inactiveTextHeight;

        [SerializeField] private Color _defaultButtonColor;
        [SerializeField] private Color _playerColor;

        [Header("Selection State")]
        [SerializeField] private SelectionStateData _selectionStateData;

        public void SetActive()
        {
            if (_isActive)
            {
                //Debug.Log($"<color=#00ffff>{gameObject.name} is already active");
                return;
            }

            //Debug.Log($"<color=#00ff00>{gameObject.name} - activated");
            _isActive = true;

            // display 3 piece icons and color selector and set player name position to top
            for (int i = 0; i < _objectsShownWhileActive.Length; i++)
            {
                _objectsShownWhileActive[i].SetActive(true);
            }

            _playerPanelImage.color = _activePanelImageColor;
            _playerNumberText.color = _activeTextColor;

            Vector2 textPos = _playerNumberText.rectTransform.anchoredPosition;

            _playerNumberText.rectTransform.anchoredPosition = new Vector2(textPos.x, _activeTextHeight);

            // additionally, need to reset all of the slots to display the plus button, set the image to none and the color to be zero.
            for (int i = 0; i < _selectedPieceImages.Length; i++)
            {
                var pieceImage = _selectedPieceImages[i];

                Debug.Log("this happened");

                pieceImage.color = Color.clear;
                pieceImage.sprite = null;
            }

            for (int i = 0; i < _plusButtonObjects.Length; i++)
            {
                _plusButtonObjects[i].SetActive(true);
            }
        }

        public void SetInactive()
        {
            if (!_isActive)
            {
                //Debug.Log($"<color=#ffff00>{gameObject.name} is already inactive");
                return;
            }

            //Debug.Log($"<color=#ff0000>{gameObject.name} - deactivated");
            _isActive = false;

            // hide 3 piece icons and color selector and set player name position to center
            for (int i = 0; i < _objectsShownWhileActive.Length; i++)
            {
                _objectsShownWhileActive[i].SetActive(false);
            }

            _playerPanelImage.color = _inactivePanelImageColor;
            _playerNumberText.color = _inactiveTextColor;

            Vector2 textPos = _playerNumberText.rectTransform.anchoredPosition;

            _playerNumberText.rectTransform.anchoredPosition = new Vector2(textPos.x, _inactiveTextHeight);
        }

        public void OnPressAddPiece_UI_BUTTON(int slotID)
        {
            if (_selectionStateData.PlayerAddingPiece) return;

            _buttonBackgroundImages[slotID].color = _playerColor;

            _selectionStateData.SetPlayerAndPieceSelected(_playerID, slotID);
        }

        public void OnPieceChosen(int slotID, Sprite pieceSprite)
        {
            _buttonBackgroundImages[slotID].color = _defaultButtonColor;

            _plusButtonObjects[slotID].SetActive(false);
            var pieceImage = _selectedPieceImages[slotID];

            // change the plus button image to be that of what was chosen (dictionary)
            pieceImage.color = Color.white;
            pieceImage.sprite = pieceSprite;
        }
    }
}