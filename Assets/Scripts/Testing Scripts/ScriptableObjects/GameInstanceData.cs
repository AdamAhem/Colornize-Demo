using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameInstanceData", menuName = "SO/GameInstanceData")]
    public class GameInstanceData : ScriptableObject
    {
        [Header("FIXED CONSTANTS")]
        [SerializeField] private int _piecesPerPlayer;

        [Header("DO NOT CHANGE UNLESS TESTING")]
        [SerializeField] private int _numberOfPlayers;

        [Header("<color=#ff0000> --- READONLY --- DO NOT ADD OR REMOVE ELEMENTS MANUALLY FROM THE EDITOR</color>")]
        [SerializeField] private List<PreGamePlayerConfig> _playerConfig;

        [Header("Most Recent Clicked Cell")]
        [SerializeField][ReadOnly] private int _lastClickedRow;
        [SerializeField][ReadOnly] private int _lastClickedColumn;

        public void SetLastClickedCellInfo(int row, int column)
        {
            _lastClickedRow = row;
            _lastClickedColumn = column;
        }

        public Vector2 LastClickedCell => new Vector2(_lastClickedRow, _lastClickedColumn);

        public void ResetData()
        {
            _numberOfPlayers = 0;
            _playerConfig = null;

            _lastClickedRow = 0;
            _lastClickedColumn = 0;
        }

        public void GenerateNewPlayerConfigs(int playerCount)
        {
            _playerConfig = new List<PreGamePlayerConfig>(playerCount);
        }

        public void UpdatePlayerConfig(int playerID, int slotID, int pieceID)
        {
            var config = _playerConfig[playerID];
            config.SetPieceID(slotID, pieceID);
        }

        public int PiecesPerPlayer => _piecesPerPlayer;

        public int NumberOfPlayers => _numberOfPlayers;

        public void SetNumberOfPlayers(int number)
        {
            _numberOfPlayers = number;

            int configCount = _playerConfig.Count;

            if (_numberOfPlayers < configCount)
            {
                for (int i = configCount; i > _numberOfPlayers; i--)
                {
                    int j = i - 1;
                    Debug.Log($"removing: {j}");
                    _playerConfig.RemoveAt(j);
                }
            }
            else if (_numberOfPlayers > configCount)
            {
                for (int i = 0; i < _numberOfPlayers - configCount; i++)
                {
                    int j = i + configCount;
                    Debug.Log($"adding: {j}");
                    _playerConfig.Add(new PreGamePlayerConfig(j, _piecesPerPlayer));
                }
            }
        }

        public bool PlayerSlotsAreFilled()
        {
            return _playerConfig.All(player => player.IsReady() is true);
        }

        public int GetPlayerPieceID(int playerID, int slotID)
        {
            return _playerConfig[playerID].GetPieceID(slotID);
        }
    }
}