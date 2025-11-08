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
        }

        public void MarkPieceAsSelected(int slotID)
        {
            _piecePlaced[slotID] = true;
        }

        public void ResetInterface()
        {
            ResetInterfaceStatus();
            _playerText.color = _inactiveTextColor;
            _background.color = _inactiveBackgroundColor;

            for (int i = 0; i < _pieceButtons.Length; i++)
            {
                UnhighlightSlot(i);
                DisableSlotButton(i);
            }
        }

        // what does this thing need to keep track of?

        // ok new approach: DO NOT KEEP TRACK OF WHICH PIECES HAVE BEEN CHOSEN OVER HERE. USE A SCRIPTABLE OBJECT TO DO THAT.

        // scriptable object will be used to keep track of:
        //  1) which piece has been placed on the board and where on the board.

        // this script ONLY handles visual updates.



        // VISUAL METHODS:
        //  1) show the whole thing
        //  2) hide the whole thing
        //  3) set one button (based on SLOT ID) to be interactable or not
        //  4) set the background color (either inactive, selected or disabled) of one button (based on SLOT ID)
        //  5) set the piece icon (based on SLOT ID) (this is only called when going from SELECTION TO PLACEMENT)

        //  inactive: default (gray). its when the button is not the one being clicked/selected.
        //  selected: based on player color. its the one that is clicked.
        //  disabled: ONLY when this piece has been placed on the board will it be colored disabled (based on player color)

    }
}