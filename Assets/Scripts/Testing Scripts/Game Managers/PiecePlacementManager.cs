using UnityEngine;

namespace Game
{
    public class PiecePlacementManager : MonoBehaviour
    {
        private enum PlacementState { ChoosingPiece, ChoosingCell, AwaitingConfirm }

        [Header("Components")]
        [SerializeField] private PlacementPiecesInterface[] _interfaces;
        [SerializeField] private GameManager _gameManager;

        [Header("Scriptable Objects")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Events")]
        [SerializeField] private GameEvent _clickCellEvent;

        [Header("Visualising state")]
        [SerializeField][ReadOnly] private PlacementState _currentState;
        [SerializeField][ReadOnly] private int _currentPlayerIndex;
        [SerializeField][ReadOnly] private int _currentSlotIndex;

        private int _piecesPlaced;
        private int _totalPiecesExpected;

        private Vector2 _positionOfNewlyPlacedPiece;

        public void Initialize()
        {
            _currentPlayerIndex = 0;
            _currentState = PlacementState.ChoosingPiece;
            int playerCount = _gameData.NumberOfPlayers;

            // reset interface statuses
            for (int h = 0; h < playerCount; h++)
            {
                _interfaces[h].ResetInterfaceStatus();
            }

            // only allow the current player index to press, disable the rest.
            EnablePlayerInterface(_currentPlayerIndex);

            for (int i = 1; i < playerCount; i++)
            {
                DisablePlayerInterface(i);
            }

            // hide the rest
            for (int j = playerCount; j < _interfaces.Length; j++)
            {
                HidePlayerInterface(j);
            }
        }

        public void Enable()
        {
            Debug.Log("ENABLED PLACEMENT MANAGER");
            _clickCellEvent.AddEvent(OnClickCell);

            _piecesPlaced = 0;
            _totalPiecesExpected = _gameData.NumberOfPlayers * _gameData.PiecesPerPlayer;

            // set the icons for all players.
            int playerCount = _gameData.NumberOfPlayers;

            for (int i = 0; i < playerCount; i++)
            {
                for (int j = 0; j < _gameData.PiecesPerPlayer; j++)
                {
                    int k = _gameData.GetPlayerPieceID(i, j);
                    _interfaces[i].SetPieceIcon(j, _catalog.Get(k).Icon);
                }
            }
        }

        public void Disable()
        {
            Debug.Log("DISABLED PLACEMENT MANAGER");
            _clickCellEvent.RemoveEvent(OnClickCell);
        }

        public void OnPressPieceButton_UI_Button(int slotID)
        {
            if (_currentState == PlacementState.AwaitingConfirm)
            {
                Debug.Log("cannot select a new piece while waiting for confirmation!");
                return;
            }

            Debug.Log($"slot id - {slotID}");

            _currentSlotIndex = slotID;

            // highlight the slot background with the player's color and unhighlight the rest.
            _interfaces[_currentPlayerIndex].HighlightSlotOnly(_currentSlotIndex);

            // allow clicking of the grid (basically when piece selected = true)
            _currentState = PlacementState.ChoosingCell;
        }

        public void OnPressConfirm_UI_BUTTON()
        {
            if (_currentState != PlacementState.AwaitingConfirm)
            {
                Debug.Log("cannot confirm until a piece is placed!");
                return;
            }

            // increment number of pieces placed
            _piecesPlaced++;

            Debug.Log($"{_piecesPlaced} / {_totalPiecesExpected} pieces placed.");

            // update the cell status.
            CellStatus status = _gameData.GetCellStatusAtPosition(_positionOfNewlyPlacedPiece);

            int pieceID = _gameData.GetPlayerPieceID(_currentPlayerIndex, _currentSlotIndex);

            status.SetPlayerID(_currentPlayerIndex);
            status.SetPieceID(pieceID);

            // also set the piece position. (need to initialize it first)
            _gameData.SetPlayerPiecePosition(_currentPlayerIndex, _currentSlotIndex, status.Cell.Position);

            // DISABLE THE SLOT BUTTON SO THE PLAYER CANNOT USE THAT PIECE ANYMORE.
            _interfaces[_currentPlayerIndex].DisableSlotButton(_currentSlotIndex);

            // disable the entire iterface
            DisablePlayerInterface(_currentPlayerIndex);

            // increase current player by 1 mod number of players
            _currentPlayerIndex = _piecesPlaced % _gameData.NumberOfPlayers;

            // enable the new interface
            EnablePlayerInterface(_currentPlayerIndex);

            // check if its the last piece.
            bool finalPieceConfirmed = _piecesPlaced == _totalPiecesExpected;
            if (finalPieceConfirmed)
            {
                ConfirmFinalPiecePlaced();
                return;
            }

            _currentState = PlacementState.ChoosingPiece;
        }

        private void ConfirmFinalPiecePlaced()
        {
            Debug.Log("ITS TIME TO GAMING");
            _gameManager.OnPressConfirmToPlay();
        }

        private void EnablePlayerInterface(int playerID)
        {
            _interfaces[playerID].Enable();
        }

        private void DisablePlayerInterface(int playerID)
        {
            _interfaces[playerID].Disable();
        }

        private void HidePlayerInterface(int playerID)
        {
            _interfaces[playerID].gameObject.SetActive(false);
        }

        private void OnClickCell()
        {
            switch (_currentState)
            {
                case PlacementState.ChoosingPiece: Debug.Log("must select a piece first!"); break;
                case PlacementState.ChoosingCell: UpdateBoardWithNewPiece(); break;
                case PlacementState.AwaitingConfirm: UndoPiecePlacement(); break;
            }
        }

        private void UpdateBoardWithNewPiece()
        {
            _positionOfNewlyPlacedPiece = _gameData.LastCellPositionClicked;

            CellStatus status = _gameData.LastClickedCellStatus;
            BoardCell cell = status.Cell;

            // IMPORTANT - NEED TO CHECK IF THAT CELL IS ALREADY OCCUPIED BY ANOTHER PLAYER.
            bool alreadyOccupied = status.PlayerID != Defaults.PLAYER_ID;
            if (alreadyOccupied)
            {
                Debug.Log("This cell is already occupied, choose another one!");
                return;
            }

            int pieceID = _gameData.GetPlayerPieceID(_currentPlayerIndex, _currentSlotIndex);

            // set the piece icon on the cell to be the selected piece
            cell.SetHighlightIconAsPiece(pieceID);

            // set the color on that cell to be the player's color
            cell.SetCellColorAsPlayerColor(_currentPlayerIndex);

            // enable the confirm button and update the state to "confirming" (WIP)
            _currentState = PlacementState.AwaitingConfirm;
        }

        private void UndoPiecePlacement()
        {
            // compare the position just clicked to the saved one
            bool clickedOnSamePosition = _gameData.LastCellPositionClicked == _positionOfNewlyPlacedPiece;

            if (!clickedOnSamePosition) return;

            CellStatus status = _gameData.LastClickedCellStatus;
            BoardCell cell = status.Cell;

            // unhighlight the current position (set default color and remove piece icon)
            cell.ResetCellColor();
            cell.ClearHighlight();

            // update the board status so that the clicked position doesn't have a player ID and a piece ID.
            status.SetPlayerID(Defaults.PLAYER_ID);
            status.SetPieceID(Defaults.PIECE_ID);

            // set state to choosing cell.
            _currentState = PlacementState.ChoosingCell;
        }
    }
}