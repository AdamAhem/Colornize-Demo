using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlacementPiecesInterface : MonoBehaviour
    {
        [SerializeField] private Button[] _pieceButtons;
        [SerializeField] private Image[] _pieceIcons;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _playerText;

        [SerializeField] private int _playerID;
        [SerializeField] private Colors _colors;

        [SerializeField] private Color _inactiveTextColor;

        [SerializeField] private Color _inactiveBackgroundColor;

        [SerializeField] private Color _defaultButtonBackgroundColor;

        [Header("Game Data")]
        [SerializeField] private GameInstanceData _instanceData;

        // interface status
        private bool[] _piecePlaced;

        public void Enable()
        {
            // set buttons to active
            for (int i = 0; i < _pieceButtons.Length; i++)
            {
                if (_piecePlaced[i] is true) continue;
                _pieceButtons[i].interactable = true;
            }

            // set player text to its color (based on colors)
            _playerText.color = _colors.List[_playerID];

            // set background image to color (based on colors)
            _background.color = _colors.List[_playerID] * 0.5f + new Color(0, 0, 0, 0.5f);
        }

        public void Disable()
        {
            // set buttons to inactive
            for (int i = 0; i < _pieceButtons.Length; i++) _pieceButtons[i].interactable = false;

            // set player text to default (black)
            _playerText.color = _inactiveTextColor;

            // set background image to default (gray)
            _background.color = _inactiveBackgroundColor;
        }

        public void ResetInterfaceStatus()
        {
            _piecePlaced = new bool[_instanceData.PiecesPerPlayer];
        }

        public void SetPieceIcon(int slotID, Sprite iconSprite)
        {
            _pieceIcons[slotID].sprite = iconSprite;
        }

        public void HighlightSlot(int slotID)
        {
            _pieceButtons[slotID].image.color = _colors.List[_playerID];
        }

        public void HighlightSlotOnly(int slotID)
        {
            for (int i = 0; i < _pieceButtons.Length; i++)
            {
                var color = (i == slotID || _piecePlaced[i]) ? _colors.List[_playerID] : _defaultButtonBackgroundColor;
                _pieceButtons[i].image.color = color;
            }
        }

        public void UnhighlightSlot(int slotID)
        {
            _pieceButtons[slotID].image.color = _defaultButtonBackgroundColor;
        }

        public void DisableSlotButton(int slotID)
        {
            _pieceButtons[slotID].interactable = false;
            _piecePlaced[slotID] = true;
        }
    }
}