using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Gameplay Data", menuName = "SO/Gameplay Data")]
    public class GameplayStateData : ScriptableObject
    {
        [Header("Defaults")]
        [SerializeField][Min(1)] private int _maxMoves;
        [SerializeField][Min(0)] private int _defaultFirstPlayerID;

        [Header("State Visualisation <color=red>DO NOT MODIFY.</color>")]
        [SerializeField] private PlayerGameplayData[] _playerGameplayData;
        [SerializeField][ReadOnly] private int _currentPlayerID;
        [SerializeField][ReadOnly] private int _movesLeft;
        [SerializeField][ReadOnly] private int _maxScore;
        [SerializeField][ReadOnly] private int _currentTotalScore;
        [SerializeField][ReadOnly] private bool _pieceSelected;
        [SerializeField][ReadOnly] private bool _allowMoves;

        [SerializeField] private Coordinate _selectedPiecePosition;

        public bool MovesAllowed => _allowMoves;
        public bool MaxScoreReached => _currentTotalScore >= _maxScore;
        public int CurrentPlayerID => _currentPlayerID;
        public int MovesLeft => _movesLeft;
        public bool HasMaxMoves => _movesLeft == _maxMoves;
        public bool PieceSelected => _pieceSelected;
        public Coordinate SelectedPiecePosition => _selectedPiecePosition;
        public int TotalScore => _currentTotalScore;


        public void ResetAllData()
        {
            _playerGameplayData = null;
            ResetPlayerMovesData();
        }

        public void ResetPlayerMovesData()
        {
            _currentPlayerID = _defaultFirstPlayerID;
            _movesLeft = _maxMoves;
            _pieceSelected = false;
            _selectedPiecePosition = default;
        }

        public void ResetScoreData(int initialScore, int maxScore)
        {
            _currentTotalScore = initialScore;
            _maxScore = maxScore;
        }

        public void SetTotalScore(int totalScore) => _currentTotalScore = totalScore;

        public void GenerateNewScoreboard(PlacementStateData placementStateData)
        {
            int playerCount = placementStateData.PlayerCount;

            _playerGameplayData ??= new PlayerGameplayData[playerCount];

            for (int playerID = 0; playerID < playerCount; playerID++)
            {
                // initial score is always the number of pieces per player.
                int pieceCount = placementStateData.GetPlayerPlacementData(playerID).Length;

                PlacementData[] placementData = placementStateData.GetPlayerPlacementData(playerID);

                if (_playerGameplayData[playerID] == null)
                {
                    PlayerGameplayData gameplayData = new(pieceCount, pieceCount);
                    for (int slotID = 0; slotID < pieceCount; slotID++)
                    {
                        var data = placementData[slotID];
                        gameplayData.SetPiecePositionAtIndex(data.PieceID, data.Position, slotID);
                    }
                    _playerGameplayData[playerID] = gameplayData;
                }
                else
                {
                    PlayerGameplayData gameplayData = _playerGameplayData[playerID];
                    for (int slotID = 0; slotID < pieceCount; slotID++)
                    {
                        var data = placementData[slotID];
                        gameplayData.SetScore(pieceCount);
                        gameplayData.SetPiecePositionAtIndex(data.PieceID, data.Position, slotID);
                    }
                }
            }
        }

        public void SetPieceSelected(bool selected) => _pieceSelected = selected;

        public void SetSelectedPiecePosition(Coordinate position) => _selectedPiecePosition = position;

        public void DeductMoves(int moveCost) => _movesLeft -= moveCost;

        public void AllowMoves(bool allow) => _allowMoves = allow;

        public void EndCurrentPlayerTurn()
        {
            _currentPlayerID = (_currentPlayerID + 1) % _playerGameplayData.Length;
            _movesLeft = _maxMoves;
            _pieceSelected = false;
            _selectedPiecePosition = default;
        }

        public PlayerGameplayData GetCurrentPlayerData() => GetPlayerData(_currentPlayerID);

        public PlayerGameplayData GetPlayerData(int playerID) => _playerGameplayData[playerID];

        public int ScoreSum() => _playerGameplayData.Sum(x => x.Score);

        public int GetMaxScore() => _playerGameplayData.Max(x => x.Score);

        public int[] GetPlayersWithScore(int score)
        {
            int[] players;
            int numberOfPlayersWithScore = 0;
            for (int i = 0; i < _playerGameplayData.Length; i++)
            {
                var data = _playerGameplayData[i];

                if (data.Score == score) numberOfPlayersWithScore++;
            }

            players = new int[numberOfPlayersWithScore];
            int counter = 0;
            for (int i = 0; i < _playerGameplayData.Length; i++)
            {
                var data = _playerGameplayData[i];
                if (data.Score == score)
                {
                    players[counter] = i;
                    counter++;
                }
            }
            return players;
        }
    }
}