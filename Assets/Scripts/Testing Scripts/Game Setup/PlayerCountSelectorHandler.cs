using TMPro;
using UnityEngine;

namespace Game
{
    public class PlayerCountSelectorHandler : MonoBehaviour
    {
        [Header("Player count")]
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private int _defaultPlayerNumber;
        [SerializeField] private int _minPlayers;

        [Header("Piece selection")]
        [SerializeField] private PlayerPieceSelectionHandler[] _pieceSelectionHandlers;

        [Header("Game Data")]
        [SerializeField] private GameInstanceData _gameData;

        public void Initialize()
        {
            Debug.Log("<color=lime>Player Counter Initialized</color>");

            _gameData.GenerateNewPlayerConfigs(Defaults.MAX_PLAYERS);

            UpdatePlayerCountDataAndText(_defaultPlayerNumber);
            UpdatePlayerDisplayPanel(_defaultPlayerNumber);
        }

        public void IncreasePlayerNumber_UI_BUTTON()
        {
            int newAmount = Mathf.Clamp(_gameData.NumberOfPlayers + 1, _minPlayers, Defaults.MAX_PLAYERS);
            UpdatePlayerCountDataAndText(newAmount);
            UpdatePlayerDisplayPanel(newAmount);
        }

        public void DecreasePlayerNumber_UI_BUTTON()
        {
            int newAmount = Mathf.Clamp(_gameData.NumberOfPlayers - 1, _minPlayers, Defaults.MAX_PLAYERS);
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
            for (int i = 0; i < _pieceSelectionHandlers.Length; i++)
            {
                if (i < playerCount) _pieceSelectionHandlers[i].SetActive();
                else
                {
                    var handler = _pieceSelectionHandlers[i];
                    handler.SetInactive();
                    handler.ClearChosenPieces();
                }
            }
        }
    }
}