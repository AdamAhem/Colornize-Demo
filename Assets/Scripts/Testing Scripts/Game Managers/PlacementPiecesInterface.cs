using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlacementPiecesInterface : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button[] _pieceButtons;
        [SerializeField] private Image[] _pieceIcons;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _playerText;

        [Header("Data")]
        [SerializeField] private int _playerID;
        [SerializeField] private Colors _colors;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Colors")]
        [SerializeField] private Color _inactiveTextColor;
        [SerializeField] private Color _inactiveBackgroundColor;
        [SerializeField] private Color _defaultButtonBackgroundColor;
        [SerializeField][Range(0, 1)] private float _activeBackgroundBrightness;

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public void DisplayColorScheme(bool active)
        {
            if (active) DisplayActiveColorScheme();
            else DisplayInactiveColorScheme();
        }

        public void DisplayInactiveColorScheme()
        {
            _background.color = _inactiveBackgroundColor;
            _playerText.color = _inactiveTextColor;
        }

        public void DisplayActiveColorScheme()
        {
            Color activeColor = _colors.List[_playerID];

            _background.color = activeColor * _activeBackgroundBrightness + new Color(0, 0, 0, 1) * (1 - _activeBackgroundBrightness);
            _playerText.color = activeColor;
        }

        public void ToggleAllButtonsInteraction(bool interactable)
        {
            for (int i = 0; i < _pieceButtons.Length; i++) ToggleSlotButtonInteraction(i, interactable);
        }

        public void ToggleAllSlotButtonActiveColor(bool active)
        {
            for (int i = 0; i < _pieceButtons.Length; i++) ToggleSlotButtonActiveColor(i, active);
        }

        public void ToggleSlotButtonInteraction(int slotID, bool interactable) => _pieceButtons[slotID].interactable = interactable;

        public void ToggleSlotButtonActiveColor(int slotID, bool active)
        {
            _pieceButtons[slotID].image.color = active ? _colors.List[_playerID] : _defaultButtonBackgroundColor;
        }

        public void SetSlotPieceIcon(int slotID, int pieceID) => _pieceIcons[slotID].sprite = _catalog.Get(pieceID).Icon;
    }
}