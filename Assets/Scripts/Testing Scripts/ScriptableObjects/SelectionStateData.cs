using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SelectionStateData", menuName = "SO/SelectionStateData")]
    public class SelectionStateData : ScriptableObject
    {
        // this scriptable object keeps track of the pieces (and their slots) that each player has selected and
        [Header("Defaults")]
        [SerializeField] private int _defaultNumberOfPlayers;
        [SerializeField] private int _piecesPerPlayer;

        [Header("Events")]
        [SerializeField] private GameEvent _addPieceToPlayerEvent;
        [SerializeField] private GameEvent _choosePieceEvent;

        [Space(25)]

        [Header("Current State")]
        [SerializeField][ReadOnly] private int _recentlySelectedPlayerID;
        [SerializeField][ReadOnly] private int _recentlySelectedSlotID;
        [SerializeField][ReadOnly] private int _recentlySelectedPieceID;
        private int _previousPlayerID;
        private int _previousSlotID;

        [Header("Pre Game Config. <color=red>DO NOT MODIFY.</color>")]
        [SerializeField] private List<PlayerSelectionData> _playerConfig;

        public int RecentPlayerID => _recentlySelectedPlayerID;
        public int RecentSlotID => _recentlySelectedSlotID;
        public int RecentPieceID => _recentlySelectedPieceID;
        public int PreviousPlayerID => _previousPlayerID;
        public int PreviousSlotID => _previousSlotID;
        public int PlayerCount => _playerConfig.Count;
        public int PiecesPerPlayer => _piecesPerPlayer;

        public bool PlayerConfigAvailable => _playerConfig != null;
        public bool PlayerCountMaximized => PlayerCount == Defaults.MAX_PLAYERS;

        public bool PlayerCountMinimized => PlayerCount == Defaults.MIN_PLAYERS;

        public bool HasPreviousSelection => PreviousPlayerID != Defaults.PLAYER_ID;

        public void ResetAllDataToDefaults()
        {
            ResetCurrentSelections();
            _playerConfig = null;
        }

        public void InitializePlayerConfigs()
        {
            _playerConfig = new List<PlayerSelectionData>(Defaults.MAX_PLAYERS);
            for (int i = 0; i < _defaultNumberOfPlayers; i++)
            {
                _playerConfig.Add(new PlayerSelectionData(i, _piecesPerPlayer));
            }
        }

        public void UpdatePlayerConfig(int playerID, int slotID, int pieceID)
        {
            var config = _playerConfig[playerID];
            config.UpdateSlotWithPiece(slotID, pieceID);
        }

        public void UpdatePlayerConfigWithRecentChoices()
        {
            if (_recentlySelectedPlayerID == Defaults.PLAYER_ID) return;
            UpdatePlayerConfig(_recentlySelectedPlayerID, _recentlySelectedSlotID, _recentlySelectedPieceID);
        }

        public void IncreasePlayerCountBy1()
        {
            if (PlayerCountMaximized) return;
            _playerConfig.Add(new PlayerSelectionData(_playerConfig.Count, _piecesPerPlayer));
        }

        public void DecreasePlayerCountBy1()
        {
            if (PlayerCountMinimized) return;
            _playerConfig.RemoveAt(_playerConfig.Count - 1);
        }

        public void SetPlayerAndSlotID(int playerID, int slotID)
        {
            _recentlySelectedPlayerID = playerID;
            _recentlySelectedSlotID = slotID;
        }

        public void SetPieceID(int pieceID)
        {
            _recentlySelectedPieceID = pieceID;
        }

        public void ResetCurrentSelections()
        {
            _recentlySelectedPlayerID = Defaults.PLAYER_ID;
            _recentlySelectedSlotID = Defaults.SLOT_ID;
            _recentlySelectedPieceID = Defaults.PIECE_ID;

            _previousPlayerID = Defaults.PLAYER_ID;
            _previousSlotID = Defaults.SLOT_ID;
        }

        public void UpdatePreviousIDs()
        {
            _previousPlayerID = _recentlySelectedPlayerID;
            _previousSlotID = _recentlySelectedSlotID;
        }

        public void ResetPlayerData()
        {
            for (int i = 0; i < _playerConfig.Count; i++)
            {
                _playerConfig[i].ResetPieces();
            }
        }

        public bool IsEveryPlayerReady() => _playerConfig.All(x => x.IsReady());
    }
}