using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerScoreDisplayHandler[] _playerScores;

        [Header("Config")]
        [SerializeField][Min(1)] private int _maxMoves;

        [Header("Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Events")]
        [SerializeField] private GameEvent _cellClickedEvent;

        [Header("State Visualisation")]
        [SerializeField][ReadOnly] private int _playerTurnIndex;
        [SerializeField][ReadOnly] private int _movesLeft;
        [SerializeField][ReadOnly] private bool _pieceSelected;
        [SerializeField][ReadOnly] private Vector2 _identifiedPiecePosition;
        [SerializeField][ReadOnly] private Vector2[] _currentPossibleMoves;

        [Header("ONLY FOR TESTING")]
        [SerializeField] private bool _usePremadeSettings;
        [SerializeField] private PremadeGameSettings _premadePiecePlacement;
        [SerializeField] private PieceSetting[] _premadeColoredCells;

        public void Initialize()
        {
            Debug.Log("GAMEPLAY MANAGER INITIALIZED");

            _gameData.SetNumberOfPlayers(_premadePiecePlacement.Settings.Length);

            _playerTurnIndex = 0;
            _movesLeft = _maxMoves;
            _pieceSelected = false;
            _identifiedPiecePosition = default;
            _currentPossibleMoves = null;

            if (_usePremadeSettings)
            {
                // setup pieces
                var settings = _premadePiecePlacement.Settings;

                for (int i = 0; i < settings.Length; i++)
                {
                    int playerID = settings[i].PlayerID;

                    for (int j = 0; j < settings[i].Pieces.Length; j++)
                    {
                        var piece = settings[i].Pieces[j];

                        CellStatus status = _gameData.GetCellStatusAtPosition(piece.row, piece.column);
                        BoardCell cell = status.Cell;

                        status.SetPlayerID(playerID);
                        status.SetPieceID(piece.pieceID);

                        cell.SetHighlightIconAsPiece(piece.pieceID);
                        cell.SetCellColorAsPlayerColor(playerID);

                        _gameData.SetPlayerPiecePosition(playerID, j, piece.row, piece.column);
                    }
                }

                // setup colors

                for (int k = 0; k < _premadeColoredCells.Length; k++)
                {
                    var setting = _premadeColoredCells[k];

                    CellStatus status = _gameData.GetCellStatusAtPosition(setting.row, setting.column);
                    status.SetPlayerID(setting.pieceID);

                    status.Cell.SetCellColorAsPlayerColor(status.PlayerID);
                }

            }

            _playerScores[_playerTurnIndex].Enable();

            for (int i = 1; i < _gameData.NumberOfPlayers; i++)
            {
                Vector2[] piecePositions = _gameData.PlayerPiecePositions[i];

                for (int j = 0; j < piecePositions.Length; j++)
                {
                    CellStatus status = _gameData.GetCellStatusAtPosition(piecePositions[j]);
                    status.Cell.SetInActiveHighlight();
                }

                _playerScores[i].Disable();
            }

            for (int i = _gameData.NumberOfPlayers; i < _playerScores.Length; i++)
            {
                _playerScores[i].Hide();
            }
        }

        public void Enable()
        {
            Debug.Log("GAMEPLAY MANAGER ENABLED");

            for (int i = 0; i < _playerScores.Length; i++) _playerScores[i].ResetScore();

            _cellClickedEvent.AddEvent(OnCellClicked);
        }

        public void Disable()
        {
            Debug.Log("GAMEPLAY MANAGER DISABLED");

            _cellClickedEvent.RemoveEvent(OnCellClicked);
        }

        private void OnCellClicked()
        {
            CellStatus status = _gameData.LastClickedCellStatus;

            // check if the status' piece ID is not default (empty)
            bool cellOccupied = status.PieceID != Defaults.PIECE_ID;

            if (!_pieceSelected && cellOccupied)
            {
                OnClickOccupiedCellWithNoPieceSelected(status);
            }

            else if (_pieceSelected && cellOccupied)
            {
                OnClickOccupiedCellWithPieceSelected(status);
            }

            else if (_pieceSelected && !cellOccupied)
            {
                OnClickUnoccupiedCellWithPieceSelected(status);
            }
        }

        private void OnClickOccupiedCellWithNoPieceSelected(CellStatus status)
        {
            // POSSIBILITIES:
            // either the current player picked their own piece or they picked another player's piece.
            // only continue if the player picked THEIR OWN piece.

            bool belongsToCurrentPlayer = status.PlayerID == _playerTurnIndex;
            if (!belongsToCurrentPlayer) return;

            // here, need to highlight possible moves.
            _currentPossibleMoves = status.GetAndHighlightPossibleMoves();

            // set piece selected to be true
            _pieceSelected = true;
            _identifiedPiecePosition = _gameData.LastCellPositionClicked;
        }

        private void OnClickOccupiedCellWithPieceSelected(CellStatus status)
        {
            bool belongsToCurrentPlayer = status.PlayerID == _playerTurnIndex;
            if (!belongsToCurrentPlayer) return;

            // check if the player clicked on the same piece that they picked before.
            bool pickedSamePieceAsBefore = _identifiedPiecePosition == _gameData.LastCellPositionClicked;

            if (pickedSamePieceAsBefore)
            {
                // EDGE CASE - picked same piece WHILE MOVES LEFT IS NOT AT MAX MOVES.
                // this means that the previous move was made by the same player and they must CONSUME ALL MOVES ON THIS SPECIFIC PIECE (commit)

                if (_movesLeft < _maxMoves)
                {
                    Debug.Log("FINISH YOUR MOVES.");
                    return;
                }

                Debug.Log("picked same piece as before - UNHIGHLIGHT POSSIBLE MOVES AND ALLOW THE CURRENT PLAYER TO SELECT ANOTHER PIECE.");

                status.Cell.UnhighlightPossibleMoves(status.PlayerID, status.PieceID);

                _currentPossibleMoves = null;
                _pieceSelected = false;

                _identifiedPiecePosition = default;
            }
            else
            {
                Debug.Log("picked another one of the player's own pieces - DO NOTHING");
            }
        }

        private void OnClickUnoccupiedCellWithPieceSelected(CellStatus status)
        {
            // PIECE SELECTED - MEANS NEED TO CHECK IF A MOVE CAN BE MADE ON THE CLICKED SPOT.
            // note that the valid positions are already calculated based on the surrounding colors.

            // so only need to check if the move is valid.

            bool validPosition = _currentPossibleMoves.Contains(_gameData.LastCellPositionClicked);

            if (!validPosition)
            {
                Debug.Log("invalid target cell!");
                return;
            }

            // 1) unhighlight moves at current position.
            CellStatus cellStatusAtIdentifiedPosition = _gameData.GetCellStatusAtPosition(_identifiedPiecePosition);

            int identifiedPieceID = cellStatusAtIdentifiedPosition.PieceID;
            cellStatusAtIdentifiedPosition.Cell.UnhighlightPossibleMoves(_playerTurnIndex, identifiedPieceID);

            // 2) calculate the cost of the move about to be made.

            PieceInfo currentPiece = _catalog.Get(identifiedPieceID);
            MoveContext move = currentPiece.GetMoveCostAndType(_movesLeft, _playerTurnIndex, status.PlayerID);

            Vector2 clickedPosition = _gameData.LastCellPositionClicked;

            switch (move.Type)
            {
                case MoveType.Move: MovePiece(_identifiedPiecePosition, clickedPosition, identifiedPieceID); break;
                case MoveType.Color: ColorCell(clickedPosition, _playerTurnIndex); break;
                case MoveType.Both:
                    {
                        MovePiece(_identifiedPiecePosition, clickedPosition, identifiedPieceID);
                        ColorCell(clickedPosition, _playerTurnIndex);
                    }
                    break;
            }

            _movesLeft -= move.Cost;

            if (_movesLeft < 0)
            {
                Debug.LogWarning("This shouldn't happen!");
                _movesLeft = 0;
            }

            else if (_movesLeft > 0)
            {
                Debug.Log("STILL YOUR TURN.");

                // highlight moves at current position.
                _identifiedPiecePosition = _gameData.LastCellPositionClicked;
                _currentPossibleMoves = _gameData.LastClickedCellStatus.GetAndHighlightPossibleMoves();
            }
            else
            {
                Debug.Log("NEXT PLAYER'S TURN.");
                MoveToNextPlayer();
            }
        }

        private void MovePiece(Vector2 currentPosition, Vector2 targetPosition, int pieceID)
        {
            // get cell status at current and target positions
            CellStatus currentCellStatus = _gameData.GetCellStatusAtPosition(currentPosition);
            BoardCell currentCell = currentCellStatus.Cell;

            CellStatus targetCellStatus = _gameData.GetCellStatusAtPosition(targetPosition);
            BoardCell targetCell = targetCellStatus.Cell;

            // update status first
            int pieceIDAtCurrentCell = currentCellStatus.PieceID;

            currentCellStatus.SetPieceID(Defaults.PIECE_ID);
            targetCellStatus.SetPieceID(pieceIDAtCurrentCell);

            // update the player's piece position in the game manager.
            Vector2[] playerPiecePositions = _gameData.PlayerPiecePositions[_playerTurnIndex];

            for (int i = 0; i < playerPiecePositions.Length; i++)
            {
                if (playerPiecePositions[i] == currentCell.Position)
                {
                    playerPiecePositions[i] = targetCell.Position;
                    break;
                }
            }

            // update board visuals
            currentCell.ClearHighlight();
            targetCell.SetHighlightIconAsPiece(pieceID);
        }

        private void ColorCell(Vector2 cellPosition, int playerID)
        {
            CellStatus status = _gameData.GetCellStatusAtPosition(cellPosition);
            BoardCell cell = status.Cell;

            int idBeforeColoring = status.PlayerID;

            status.SetPlayerID(playerID);
            cell.SetCellColorAsPlayerColor(playerID);

            // INCREASE THE SCORE OF THE CURRENT PLAYER BY 1 EVERY TIME THEY COLOR AN EMPTY CELL WITH THEIR COLOR.
            if (idBeforeColoring == Defaults.PLAYER_ID) IncreaseScore(_playerTurnIndex);
        }

        private void MoveToNextPlayer()
        {
            // IMPT: WHERE TO FIND THEIR PIECES ON THE BOARD?


            // VISUAL UPDATES FOR CURRENT PLAYER:

            // 1) set all their pieces' alpha to be less than 1 (0.5)
            Vector2[] currentPlayerPositions = _gameData.PlayerPiecePositions[_playerTurnIndex];
            for (int i = 0; i < currentPlayerPositions.Length; i++)
            {
                _gameData.GetCellStatusAtPosition(currentPlayerPositions[i]).Cell.SetInActiveHighlight();
            }


            // 2) unhighlight their score display handler
            _playerScores[_playerTurnIndex].Disable();


            _playerTurnIndex = (_playerTurnIndex + 1) % _gameData.NumberOfPlayers;
            _movesLeft = _maxMoves;

            _pieceSelected = false;
            _identifiedPiecePosition = default;
            _currentPossibleMoves = null;

            // VISUAL UPDATES FOR NEXT PLAYER:

            // 1) set all their pieces' alpha to be 1
            Vector2[] newPlayerPositions = _gameData.PlayerPiecePositions[_playerTurnIndex];
            for (int i = 0; i < newPlayerPositions.Length; i++)
            {
                _gameData.GetCellStatusAtPosition(newPlayerPositions[i]).Cell.SetActiveHighlight();
            }

            // 2) highlight their score display handler
            _playerScores[_playerTurnIndex].Enable();
        }

        private void IncreaseScore(int playerID)
        {
            _playerScores[playerID].AddPoint();
        }
    }
}