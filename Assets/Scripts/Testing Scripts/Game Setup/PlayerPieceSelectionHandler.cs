using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerPieceSelectionHandler : MonoBehaviour
    {
        [SerializeField] private int _playerID;

        [Header("Slots")]
        [SerializeField] private Image[] _buttonBackgroundImages;
        [SerializeField] private Image[] _selectedPieceImages;
        [SerializeField] private GameObject[] _plusButtonObjects;

        [Header("Player panel visuals")]
        [SerializeField] private Image _playerPanelImage;
        [SerializeField] private Image _colorImage;

        [SerializeField] private GameObject[] _objectsShownWhileActive;
        [SerializeField] private TMP_Text _playerNumberText;

        [SerializeField] private Color _activePanelImageColor;
        [SerializeField] private Color _inactivePanelImageColor;

        [SerializeField] private Color _activeTextColor;
        [SerializeField] private Color _inactiveTextColor;

        [SerializeField] private float _activeTextHeight;
        [SerializeField] private float _inactiveTextHeight;

        [SerializeField] private Color _defaultButtonColor;
        [SerializeField] private Colors _colors;

        [Header("Selection State")]
        [SerializeField] private SelectionStateData _selectionStateData;

        public void SetActive()
        {
            // display 3 piece icons and color selector and set player name position to top
            for (int i = 0; i < _objectsShownWhileActive.Length; i++)
            {
                _objectsShownWhileActive[i].SetActive(true);
            }

            _playerPanelImage.color = _activePanelImageColor;
            _playerNumberText.color = _activeTextColor;
            _colorImage.color = _colors.List[_playerID];

            Vector2 textPos = _playerNumberText.rectTransform.anchoredPosition;

            _playerNumberText.rectTransform.anchoredPosition = new Vector2(textPos.x, _activeTextHeight);

            // additionally, need to reset all of the slots to display the plus button, set the image to none and the color to be zero.
            for (int i = 0; i < _selectedPieceImages.Length; i++)
            {
                var pieceImage = _selectedPieceImages[i];
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

            _buttonBackgroundImages[slotID].color = _colors.List[_playerID];

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