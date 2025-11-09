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
        [SerializeField] private Sprite _defaultSelectedPieceSprite;

        [Header("Visual config")]
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

        [Header("Data")]
        [SerializeField] private SelectionStateData _selectionStateData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Events")]
        [SerializeField] private GameEvent _playerAddPieceEvent;

        public void OnPressAddPiece_UI_BUTTON(int slotID)
        {
            _selectionStateData.SetPlayerAndSlotID(_playerID, slotID);
            _playerAddPieceEvent.Raise();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ClearSelectedPieces()
        {
            // need to loop through all images and buttons, set the color to default and icon to default.
            for (int i = 0; i < _buttonBackgroundImages.Length; i++)
            {
                _buttonBackgroundImages[i].color = _defaultButtonColor;
                _selectedPieceImages[i].sprite = _defaultSelectedPieceSprite;
            }
        }

        public void SetActiveColor(int slotID)
        {
            _buttonBackgroundImages[slotID].color = _colors.List[_playerID];
        }

        public void SetInactiveColor(int slotID)
        {
            _buttonBackgroundImages[slotID].color = _defaultButtonColor;
        }

        public void SetPieceIcon(int slotID, int pieceID)
        {
            _selectedPieceImages[slotID].sprite = _catalog.Get(pieceID).Icon;
        }
    }
}