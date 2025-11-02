using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerPieceSelectionHandler : MonoBehaviour
    {
        // by default, they are all active (because in the editor their add buttons are visible)
        [SerializeField][ReadOnly] private bool _isActive = true;

        [SerializeField] private Image _playerPanelImage;
        [SerializeField] private GameObject[] _objectsShownWhileActive;
        [SerializeField] private TMP_Text _playerNumberText;

        [SerializeField] private Color _activePanelImageColor;
        [SerializeField] private Color _inactivePanelImageColor;

        [SerializeField] private Color _activeTextColor;
        [SerializeField] private Color _inactiveTextColor;

        [SerializeField] private float _activeTextHeight;
        [SerializeField] private float _inactiveTextHeight;

        public void SetActive()
        {
            if (_isActive)
            {
                Debug.Log($"<color=#00ffff>{gameObject.name} is already active");
                return;
            }

            Debug.Log($"<color=#00ff00>{gameObject.name} - activated");
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
        }

        public void SetInactive()
        {
            if (!_isActive)
            {
                Debug.Log($"<color=#ffff00>{gameObject.name} is already inactive");
                return;
            }

            Debug.Log($"<color=#ff0000>{gameObject.name} - deactivated");
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
    }
}