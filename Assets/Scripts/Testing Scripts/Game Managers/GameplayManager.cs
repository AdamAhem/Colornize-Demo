using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerScoreDisplayHandler[] _playerScores;

        [Header("Game Objects")]
        [SerializeField] private GameObject _mainDisplayObject;
        [SerializeField] private Button _endGameButton;
        [SerializeField] private WinnerMessageManager _winnerMessageManager;

        [Header("Data")]
        [SerializeField] private PlacementStateData _placementStateData;
        [SerializeField] private GameplayStateData _gameplayStateData;
        [SerializeField] private BoardStatus _boardStatus;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Events")]
        [SerializeField] private CoordinateEvent _cellClickedEvent;
        [SerializeField] private GameEvent _gameplayToPlacementTransitionEvent;

        #region Manager methods

        public void Enable()
        {
            Debug.Log("<color=lime>Gameplay Manager Enabled</color>");

            // DATA INITIALIZATION:

            // reset all gameplay data.
            _gameplayStateData.ResetAllData();
            _gameplayStateData.GenerateNewScoreboard(_placementStateData);

            SetupBoardWithPlacementSettings();

            Coordinate boardSize = _boardStatus.Dimensions;
            int columns = boardSize.X;
            int rows = boardSize.Y;

            int maxScore = rows * columns;
            int initialScore = _gameplayStateData.ScoreSum();

            _gameplayStateData.ResetScoreData(initialScore, maxScore);

            // EVENT OBSERVATION
            _cellClickedEvent.AddEvent(OnClickCell);

            _endGameButton.interactable = true;
            _gameplayStateData.AllowMoves(true);
            _winnerMessageManager.Hide();
        }

        public void Disable()
        {
            Debug.Log("<color=lime>Gameplay Manager Disabled</color>");

            _cellClickedEvent.RemoveEvent(OnClickCell);
            _gameplayStateData.AllowMoves(false);
            _winnerMessageManager.Hide();
        }

        public void Show()
        {
            _mainDisplayObject.SetActive(true);
        }

        public void Hide()
        {
            // hide player number buttons
            if (_mainDisplayObject == null) return; //main display object can sometimes destroy itself when exiting playmode.
            _mainDisplayObject.SetActive(false);
        }

        #endregion

        #region Unity event button methods

        public void OnPressReturn_UI_BUTTON()
        {
            ResetGame();
            _gameplayToPlacementTransitionEvent.Raise();
        }

        public void OnPressRestart_UI_BUTTON()
        {
            ResetGame();
            SetupBoardWithPlacementData();
            SetupBoardWithPlacementSettings();

            _endGameButton.interactable = true;
            _gameplayStateData.AllowMoves(true);
            _winnerMessageManager.Hide();
        }

        public void OnPressEndGame_UI_BUTTON()
        {
            ConcludeGame();
        }

        #endregion

        #region Click cell event methods

        private void OnClickCell(Coordinate clickedPosition)
        {
            if (!_gameplayStateData.MovesAllowed) return;

            CellStatus clickedStatus = _boardStatus.GetCellStatusAtPosition(clickedPosition);
            int currentPlayerID = _gameplayStateData.CurrentPlayerID;

            bool pieceSelected = _gameplayStateData.PieceSelected;
            bool clickedOnOwnColor = clickedStatus.PlayerID == currentPlayerID;
            bool clickedOnGrayCell = !clickedStatus.IsColored;

            // case 1: clicked in a cell that isn't occupied by the player while NO piece is selected. In this case, exit early.
            if (!clickedOnOwnColor && !pieceSelected) return;

            // case 2: clicked on own cell when no piece is selected.
            else if (clickedOnOwnColor && !pieceSelected)
            {
                _gameplayStateData.SetPieceSelected(true);
                _gameplayStateData.SetSelectedPiecePosition(clickedPosition);

                // here, need to highlight move of this position. (logic is the same, but it shall be done elsewhere)
                _boardStatus.HighlightPieceMove(currentPlayerID, clickedStatus.PieceID, clickedPosition);
            }

            // case 3: clicked on GRAY cell when piece is selected.
            else if (clickedOnGrayCell && pieceSelected)
            {
                PerformMove(_gameplayStateData.SelectedPiecePosition, clickedPosition);
            }

            // case 4: clicked on own color AND own piece has been selected.
            else if (clickedOnOwnColor && pieceSelected)
            {
                bool samePieceClicked = clickedPosition == _gameplayStateData.SelectedPiecePosition;
                bool occupiedCellClicked = clickedStatus.IsOccupied;

                if ((!_gameplayStateData.HasMaxMoves && occupiedCellClicked) || (occupiedCellClicked && !samePieceClicked)) return;

                else if (samePieceClicked)
                {
                    // unhighlight
                    _boardStatus.UnhighlightPieceMove(currentPlayerID, clickedStatus.PieceID, clickedPosition);

                    // set piece as not selected
                    _gameplayStateData.SetPieceSelected(false);

                    // set last piece position as default.
                    _gameplayStateData.SetSelectedPiecePosition(default);
                }
                else
                {
                    // if this happens, then the player clicked on an unoccupied cell with their color.
                    PerformMove(_gameplayStateData.SelectedPiecePosition, clickedPosition);
                }
            }
        }

        #endregion

        #region Utility methods

        private void SetupBoardWithPlacementSettings()
        {
            int numberOfPlayers = _placementStateData.PlayerCount;
            int currentPlayerID = _gameplayStateData.CurrentPlayerID;

            for (int playerID = 0; playerID < _playerScores.Length; playerID++)
            {
                var scoreboard = _playerScores[playerID];

                // set all player scores to 0
                scoreboard.ResetScore();

                // only show the scoboards of participating players
                if (playerID < numberOfPlayers)
                {
                    scoreboard.Show();

                    // NEED TO DIM THE ICONS OF OTHER PLAYERS OTHER THAN FIRST PLAYER.
                    PlayerGameplayData data = _gameplayStateData.GetPlayerData(playerID);

                    for (int i = 0; i < data.PiecePositions.Length; i++)
                    {
                        BoardCell cell = _boardStatus.GetCellStatusAtPosition(data.PiecePositions[i].Position).Cell;
                        cell.SetHighlightAsActive(playerID == currentPlayerID);
                    }

                    scoreboard.SetScore(data.Score);
                }
                else scoreboard.Hide();

                // indicate that the current player gets the first move.
                scoreboard.SetAsActiveColor(playerID == currentPlayerID);
            }
        }

        private void PerformMove(Coordinate currentPosition, Coordinate targetPosition)
        {
            CellStatus current = _boardStatus.GetCellStatusAtPosition(currentPosition);

            int pieceID = current.PieceID;
            bool isValidMove = _boardStatus.IsValidMove(pieceID, targetPosition, currentPosition);

            if (!isValidMove)
            {
                Debug.Log("invalid move.");
                return;
            }

            CellStatus target = _boardStatus.GetCellStatusAtPosition(targetPosition);

            int currentPlayerID = _gameplayStateData.CurrentPlayerID;

            // move about to be made - unhighlight move at piece position.
            _boardStatus.UnhighlightPieceMove(currentPlayerID, pieceID, currentPosition);

            PieceInfo pieceInfo = _catalog.Get(pieceID);
            int clickedPositionPlayerID = target.PlayerID;

            // if it is a valid move, then the selected piece can perform a move. Obtain the move context.
            MoveContext moveContext = pieceInfo.GetMoveContext(_gameplayStateData.MovesLeft, currentPlayerID, clickedPositionPlayerID);

            switch (moveContext.Type)
            {
                case MoveType.Translate: DoTranslateMove(currentPosition, targetPosition); break;
                case MoveType.Color: DoColorMove(currentPosition, targetPosition); break;
                case MoveType.Both:
                    DoColorMove(currentPosition, targetPosition);
                    DoTranslateMove(currentPosition, targetPosition);
                    break;
            }

            // reduce amount of moves left.
            _gameplayStateData.DeductMoves(moveContext.Cost);

            // turn over when number of moves <= 0.
            if (_gameplayStateData.MovesLeft <= 0)
            {
                // dim current player pieces
                PlayerGameplayData data = _gameplayStateData.GetPlayerData(currentPlayerID);
                for (int i = 0; i < data.PiecePositions.Length; i++)
                {
                    _boardStatus.GetCellStatusAtPosition(data.PiecePositions[i].Position).Cell.SetHighlightAsActive(false);
                }

                _playerScores[currentPlayerID].SetAsActiveColor(false);

                _gameplayStateData.EndCurrentPlayerTurn();

                int newPlayerID = _gameplayStateData.CurrentPlayerID;

                PlayerGameplayData newData = _gameplayStateData.GetPlayerData(newPlayerID);
                for (int i = 0; i < newData.PiecePositions.Length; i++)
                {
                    _boardStatus.GetCellStatusAtPosition(newData.PiecePositions[i].Position).Cell.SetHighlightAsActive(true);
                }

                _playerScores[newPlayerID].SetAsActiveColor(true);
            }

            else
            {
                Coordinate position = _gameplayStateData.SelectedPiecePosition;
                _boardStatus.HighlightPieceMove(currentPlayerID, _boardStatus.GetCellStatusAtPosition(position).PieceID, position);
            }

            CheckForWinners();
        }

        private void DoTranslateMove(Coordinate currentPosition, Coordinate targetPosition)
        {
            PlayerGameplayData gameplayData = _gameplayStateData.GetCurrentPlayerData();

            CellStatus current = _boardStatus.GetCellStatusAtPosition(currentPosition);
            CellStatus target = _boardStatus.GetCellStatusAtPosition(targetPosition);

            int currentPlayerID = _gameplayStateData.CurrentPlayerID;
            int pieceID = current.PieceID;

            for (int i = 0; i < gameplayData.PiecePositions.Length; i++)
            {
                var x = gameplayData.PiecePositions[i];
                bool samePos = x.Position == currentPosition;

                if (samePos)
                {
                    gameplayData.UpdatePiecePosition(i, targetPosition);
                    break;
                }
            }

            // CELL STATUS DATA UPDATE: swap positions.
            current.SetAsUnoccupied();
            target.SetPlayerAndPieceID(currentPlayerID, pieceID);

            _gameplayStateData.SetSelectedPiecePosition(targetPosition);

            // VISUAL UPDATE: move piece from first board to second board
            current.Cell.ClearHighlightIcon();
            target.Cell.SetHighlightIconAsPiece(pieceID);
        }

        private void DoColorMove(Coordinate currentPosition, Coordinate targetPosition)
        {
            PlayerGameplayData gameplayData = _gameplayStateData.GetCurrentPlayerData();

            CellStatus current = _boardStatus.GetCellStatusAtPosition(currentPosition);
            CellStatus target = _boardStatus.GetCellStatusAtPosition(targetPosition);

            int currentPlayerID = _gameplayStateData.CurrentPlayerID;
            int pieceID = current.PieceID;

            bool sameColor = target.PlayerID == currentPlayerID;

            target.SetPlayerID(currentPlayerID);
            target.Cell.SetCellColorAsPlayerColor(currentPlayerID);

            if (sameColor) return;

            int point = 1;

            gameplayData.AddPoint(point);
            _playerScores[currentPlayerID].SetScore(gameplayData.Score);
            _gameplayStateData.SetTotalScore(_gameplayStateData.TotalScore + point);
        }

        private void CheckForWinners()
        {
            if (!_gameplayStateData.MaxScoreReached) return;
            ConcludeGame();
        }

        private int[] GetWinners() => _gameplayStateData.GetPlayersWithScore(_gameplayStateData.GetMaxScore());

        private void ResetGame()
        {
            // 1) reset gameplay data and clean up scoreboard.
            _gameplayStateData.ResetPlayerMovesData();
            _gameplayStateData.GenerateNewScoreboard(_placementStateData);

            // 2) clear board
            _boardStatus.ClearBoard();
        }

        private void SetupBoardWithPlacementData()
        {
            for (int playerID = 0; playerID < _placementStateData.PlayerCount; playerID++)
            {
                PlacementData[] placementData = _placementStateData.GetPlayerPlacementData(playerID);

                for (int slotID = 0; slotID < placementData.Length; slotID++)
                {
                    PlacementData data = placementData[slotID];
                    Coordinate position = data.Position;
                    int pieceID = data.PieceID;

                    CellStatus status = _boardStatus.GetCellStatusAtPosition(position);
                    status.SetPlayerAndPieceID(playerID, pieceID);

                    BoardCell cell = status.Cell;
                    cell.SetCellColorAsPlayerColor(playerID);
                    cell.SetHighlightIconAsPiece(pieceID);
                }
            }
        }

        private void ConcludeGame()
        {
            int[] winners = GetWinners();

            _gameplayStateData.ResetPlayerMovesData();

            _boardStatus.ClearAllPossibleMoves();
            _boardStatus.HighlightAllPiecesAsActive();

            HighlightWinners(winners);

            _endGameButton.interactable = false;
            _gameplayStateData.AllowMoves(false);
        }

        private void HighlightWinners(int[] winners)
        {
            // set color scheme of all players to default.
            for (int i = 0; i < _playerScores.Length; i++)
            {
                var score = _playerScores[i];
                score.SetAsActiveColor(false);
            }

            // go through the indices of winners and highlight only those
            for (int j = 0; j < winners.Length; j++)
            {
                var winnerScore = _playerScores[winners[j]];
                winnerScore.SetAsActiveColor(true);
            }

            _winnerMessageManager.DisplayWinners(winners);
        }

        #endregion
    }
}