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

        public void UseInActiveColors()
        {
            _background.color = _inactiveBackgroundColor;
            _playerText.color = _inactiveTextColor;
        }

        public void UseActiveColor()
        {
            Color activeColor = _colors.List[_playerID];

            _background.color = activeColor * _activeBackgroundBrightness + new Color(0, 0, 0, 1) * (1 - _activeBackgroundBrightness);
            _playerText.color = activeColor;
        }

        public void EnableSlotButtonInteraction(int slotID) => _pieceButtons[slotID].interactable = true;

        public void DisableSlotButtonInteraction(int slotID) => _pieceButtons[slotID].interactable = false;

        public void SetInactiveSlotColor(int slotID) => _pieceButtons[slotID].image.color = _defaultButtonBackgroundColor;

        public void SetSelectedSlotColor(int slotID) => _pieceButtons[slotID].image.color = _colors.List[_playerID];

        public void SetSlotPieceIcon(int slotID, int pieceID) => _pieceButtons[slotID].image.sprite = _catalog.Get(pieceID).Icon;
    }
}