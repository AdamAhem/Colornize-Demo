using TMPro;
using UnityEngine;

namespace Game
{
    public class PlayerCountSelectorHandler : MonoBehaviour
    {
        [Header("Player count")]
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private int _defaultPlayerNumber;
        [SerializeField] private int _maxPlayers;
        [SerializeField] private int _minPlayers;

        [Header("Piece selection")]
        [SerializeField] private PlayerPieceSelectionHandler[] _pieceSelectionHandlers;

        [Header("Game Data")]
        [SerializeField] private GameInstanceData _gameData;

        public void Initialize()
        {
            UpdatePlayerCountDataAndText(_defaultPlayerNumber);
            UpdatePlayerDisplayPanel(_defaultPlayerNumber);
        }

        public void IncreasePlayerNumber_UI_BUTTON()
        {
            int newAmount = Mathf.Clamp(_gameData.NumberOfPlayers + 1, _minPlayers, _maxPlayers);
            UpdatePlayerCountDataAndText(newAmount);
            UpdatePlayerDisplayPanel(newAmount);
        }

        public void DecreasePlayerNumber_UI_BUTTON()
        {
            int newAmount = Mathf.Clamp(_gameData.NumberOfPlayers - 1, _minPlayers, _maxPlayers);
            UpdatePlayerCountDataAndText(newAmount);
            UpdatePlayerDisplayPanel(newAmount);
        }

        private void UpdatePlayerCountDataAndText(int amount)
        {
            _gameData.SetNumberOfPlayers(amount);
            _numberText.text = amount.ToString();
        }

        private void UpdatePlayerDisplayPanel(int playerCount)
        {
            // a player panel can be either ACTIVATED OR DEACTIVATED.
            // when playercount changes, the first panels from the left will be activated (if not already so)
            // then the last right will be deactivated (if not already so)

            for (int i = 0; i < _pieceSelectionHandlers.Length; i++)
            {
                if (i < playerCount) _pieceSelectionHandlers[i].SetActive();
                else _pieceSelectionHandlers[i].SetInactive();
            }
        }
    }
}