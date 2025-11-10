using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PiecePlacementManager : MonoBehaviour
    {
        [Header("INITIALIZATION STATUS")]
        [SerializeField][ReadOnly] private bool _initialized = false;

        [Header("Components")]
        [SerializeField] private PlacementPiecesInterface[] _interfaces;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _confirmButton;

        [Header("Game Objects")]
        [SerializeField] private GameObject _mainDisplayObject;

        [Header("Data")]
        [SerializeField] private SelectionStateData _selectionStateData;
        [SerializeField] private PlacementStateData _placementStateData;
        [SerializeField] private BoardStatus _boardStatus;

        [Header("Events")]
        [SerializeField] private CoordinateEvent _clickCellEvent;
        [SerializeField] private GameEvent _clickedPiecePlacementButtonEvent;
        [SerializeField] private GameEvent _placementToSelectionTransitionEvent;
        [SerializeField] private GameEvent _placementToGameplayTransitionEvent;

        public void Initialize()
        {
            if (_initialized)
            {
                Debug.Log($"<color=magenta>Placement Manager already initialized</color>");
                return;
            }

            Debug.Log("<color=lime>Placement Manager Initialized</color>");

            _initialized = true;

            // this initialize method may not be necessary at all.
        }

        public void Enable()
        {
            Debug.Log("<color=lime>Placement Manager Enabled</color>");
            _clickCellEvent.AddEvent(OnClickCell);
            _clickedPiecePlacementButtonEvent.AddEvent(OnClickedPieceButton);

            // reset (most) placement data
            _placementStateData.ResetCurrentData();

            // UPDATING DATA - generating a new jagged array of data for keeping track of if each player's piece is placed, and at which position
            _placementStateData.InitializePlacementData(_selectionStateData);

            int playerCount = _selectionStateData.PlayerCount;
            int piecesPerPlayer = _selectionStateData.PiecesPerPlayer;

            // UPDATING VISUALS - displaying only the interfaces of the players participating while hiding the rest.
            for (int playerID = 0; playerID < _interfaces.Length; playerID++)
            {
                var playerInterface = _interfaces[playerID];

                if (playerID < playerCount)
                {
                    bool isFirstPlayerToChoose = playerID == _placementStateData.CurrentPlayerID;
                    if (isFirstPlayerToChoose) playerInterface.DisplayActiveColorScheme();

                    for (int slotID = 0; slotID < piecesPerPlayer; slotID++)
                    {
                        // setting the piece ID of the slots correctly.
                        playerInterface.SetSlotPieceIcon(slotID, _placementStateData.GetPlayerPieceAtSlot(playerID, slotID));

                        // need to check if this playerID is the default first one who gets to choose their player.
                        playerInterface.ToggleSlotButtonInteraction(slotID, isFirstPlayerToChoose);
                    }
                    playerInterface.Show();
                }
                else playerInterface.Hide();
            }

            _playButton.interactable = false;
            _confirmButton.interactable = false;
        }

        public void Disable()
        {
            Debug.Log("<color=lime>Placement Manager Disabled</color>");
            _clickCellEvent.RemoveEvent(OnClickCell);
            _clickedPiecePlacementButtonEvent.RemoveEvent(OnClickedPieceButton);
        }

        public void Show()
        {
            _mainDisplayObject.SetActive(true);
        }

        public void Hide()
        {
            if (_mainDisplayObject == null) return; //sometimes this gets destroyed when exiting playmode.
            _mainDisplayObject.SetActive(false);
        }

        #region Unity Event Button Methods;

        public void OnPressPlay_UI_BUTTON()
        {
            Debug.Log("pressed play.");
        }

        public void OnPressReset_UI_BUTTON()
        {
            ResetPlacement();

            _playButton.interactable = false;
            _confirmButton.interactable = false;
        }

        public void OnPressConfirm_UI_BUTTON()
        {
            _confirmButton.interactable = false;

            // reset recent placement data
            _placementStateData.ResetRecentClicksData();
            _placementStateData.IncrementPiecesPlacecBy1();

            // CHECK IF ENOUGH PIECES ARE PLACED.
            int expectedPieces = _placementStateData.ExpectedPieces;
            int currentPieces = _placementStateData.PiecesPlaced;

            if (currentPieces == expectedPieces)
            {
                _playButton.interactable = true;
                return;
            }

            int previousPlayerID = _placementStateData.CurrentPlayerID;
            int nextPlayerID = (previousPlayerID + 1) % _selectionStateData.PlayerCount;

            // update placement data current player
            _placementStateData.SetCurrentPlayerID(nextPlayerID);

            var previousInterface = _interfaces[previousPlayerID];
            var nextInterface = _interfaces[nextPlayerID];

            // disable previous player interface (DO NOT CHANGE COLOR OF PIECE BUTTONS)
            previousInterface.DisplayInactiveColorScheme();
            previousInterface.ToggleAllButtonsInteraction(false);

            // enable next player interface (ONLY UP TO PIECES NOT PLACED YET)
            nextInterface.DisplayActiveColorScheme();

            // HERE, NEED TO ONLY SELECTIVELY TOGGLE BUTTONS OF PIECES THE PLAYER HASN'T PLACED YET.
            PlacementData[] nextPlayerData = _placementStateData.GetPlayerPlacementData(nextPlayerID);

            for (int slotID = 0; slotID < nextPlayerData.Length; slotID++)
            {
                bool isPlaced = nextPlayerData[slotID].IsPlaced;
                nextInterface.ToggleSlotButtonInteraction(slotID, !isPlaced);
            }
        }

        public void OnPressRandomize_UI_BUTTON()
        {
            ResetPlacement();

            int playerCount = _selectionStateData.PlayerCount;
            int pieceCount = _selectionStateData.PiecesPerPlayer;

            for (int playerID = 0; playerID < playerCount; playerID++)
            {
                for (int slotID = 0; slotID < pieceCount; slotID++)
                {
                    // choose a random (unoccupied) cell position (can be done in a while loop)
                    // things to update in the loop: COLORING THE BOARD AND PLACEMENT DATA.

                    PlacementData data = _placementStateData.GetPlayerPlacementData(playerID)[slotID];

                    int c = 0;
                    int max = 1000;

                    // searching for a new position

                    Coordinate randomPosition = Coordinate.GetRandomWithinBBOX(Coordinate.Zero, _boardStatus.Dimensions);
                    CellStatus status = _boardStatus.GetCellStatusAtPosition(randomPosition);

                    // get a random position and its corresponding status.
                    // check in the while loop if its occupied.

                    // if it is, then get another one.
                    // if it isn't, then update the board cell with the color/icon AND update placement data isplaced + coordinate.

                    while (c < max && status.IsOccupied)
                    {
                        c++;
                        randomPosition = Coordinate.GetRandomWithinBBOX(Coordinate.Zero, _boardStatus.Dimensions);
                        status = _boardStatus.GetCellStatusAtPosition(randomPosition);
                    }

                    if (c >= max)
                    {
                        Debug.LogWarning($"failsafe counter reached max value - {max}. getting next unoccupied cell");
                        randomPosition = _boardStatus.GetNextFreePosition();
                        status = _boardStatus.GetCellStatusAtPosition(randomPosition);
                    }

                    // DATA - placement data
                    data.SetPosition(randomPosition);
                    data.SetAsPlaced(true);

                    int pieceID = data.PieceID;

                    // DATA - updating cell status
                    status.SetPlayerAndPieceID(playerID, pieceID);

                    // VISUAL - updating cell visuals
                    BoardCell cell = status.Cell;
                    cell.SetCellColorAsPlayerColor(playerID);
                    cell.SetHighlightIconAsPiece(pieceID);

                }
            }

            // note that this places ALL pieces onto the board, meaning that the game is ready to play.
            // as such, all interfaces must be disabled (do not touch color scheme)
            // and the play button should be enabled (and confirm button disabled)

            for (int playerID = 0; playerID < _selectionStateData.PlayerCount; playerID++)
            {
                var playerInterface = _interfaces[playerID];
                // toggle all interactables false
                playerInterface.ToggleAllButtonsInteraction(false);

                // set all icon button colors to active
                playerInterface.ToggleAllSlotButtonActiveColor(true);
            }

            _playButton.interactable = true;
            _confirmButton.interactable = false;
        }

        public void OnPressReturn_UI_BUTTON()
        {
            ResetPlacement();
            _placementToSelectionTransitionEvent.Raise();
        }

        public void OnPressQuit_UI_BUTTON()
        {
            Debug.Log("<color=red>PRESSED QUIT BUTTON - WORK IN PROGRESS</color>");
        }

        public void OnPressPieceButton_UI_Button(int slotID)
        {
            Debug.Log($"clicked piece button from player: (current player) -> slot {slotID} ");
        }

        #endregion

        private void OnClickCell(Coordinate position)
        {
            int recentPlayerID = _placementStateData.CurrentPlayerID;
            int recentSlotID = _placementStateData.RecentSlotID;

            // case 1: no piece selected: DO NOTHING, REGARDLESS OF WHICH CELL IS CLICKED.
            bool pieceChosen = _placementStateData.RecentSlotID != Defaults.SLOT_ID;
            if (!pieceChosen) return;

            // all cases moving forward include a piece already being chosen.
            CellStatus clickedCellStatus = _boardStatus.GetCellStatusAtPosition(position);
            BoardCell cell = clickedCellStatus.Cell;
            int cellOccupantID = clickedCellStatus.PlayerID;

            // case 2: clicked on empty cell (cell occupant ID == -1)
            if (cellOccupantID == Defaults.PLAYER_ID)
            {
                PlacementData placementData = _placementStateData.GetPlayerPlacementData(recentPlayerID)[recentSlotID];
                int pieceID = placementData.PieceID;
                bool pieceAlreadyPlaced = placementData.IsPlaced;

                if (pieceAlreadyPlaced)
                {
                    // need to move that piece elsewhere (get its coordinate)
                    Coordinate previousClickedCellPosition = placementData.Position;

                    CellStatus previousCellStatus = _boardStatus.GetCellStatusAtPosition(previousClickedCellPosition);
                    BoardCell previousCell = previousCellStatus.Cell;

                    previousCellStatus.ResetIDs();

                    previousCell.ResetCellColor();

                    previousCell.ResetHighlightIcon();
                }

                // UPDATE DATA - update cell status player ID and piece ID
                clickedCellStatus.SetPlayerAndPieceID(recentPlayerID, pieceID);

                // UPDATE DATA - placement data (bool flag is placed AND coordinate)
                _placementStateData.SetPlayerPieceAtSlotAsPlaced(recentPlayerID, recentSlotID, position);

                // VISUAL UPDATE - color the cell with the color of the player.
                cell.SetCellColorAsPlayerColor(recentPlayerID);

                // VISUAL UPDATE - set the cell's icon as the placed piece.
                cell.SetHighlightIconAsPiece(pieceID);
            }

            _confirmButton.interactable = true;
        }

        private void OnClickedPieceButton()
        {
            int currentSlotID = _placementStateData.RecentSlotID;
            int previousSlotID = _placementStateData.PreviousSlotID;

            if (currentSlotID == previousSlotID) return;
            // edge case: current slot == previous slot (return early, no need to run any logic)

            int currentPlayerID = _placementStateData.CurrentPlayerID;

            bool recentPiecePlacedOnBoard = _placementStateData.IsRecentPlayerPiecePlaced();
            bool previousPiecePlacedOnBoard = _placementStateData.IsPreviousPlayerPiecePlaced();
            bool pieceButtonPreviouslyClicked = _placementStateData.PreviousSlotID != Defaults.SLOT_ID;

            var buttonsInterface = _interfaces[currentPlayerID];

            if (previousPiecePlacedOnBoard)
            {
                // here, need to swap out piece on the cell that was last clicked (in data container and visually)

                int newPieceID = _placementStateData.GetPlayerPieceAtSlot(currentPlayerID, currentSlotID);

                // DATA (current slot ID not placed anymore and new slot id is placed)
                PlacementData[] data = _placementStateData.GetPlayerPlacementData(currentPlayerID);

                Coordinate piecePosition = data[previousSlotID].Position;

                data[previousSlotID].SetAsPlaced(false);
                data[previousSlotID].ResetPosition();

                data[currentSlotID].SetAsPlaced(true);
                data[currentSlotID].SetPosition(piecePosition);

                // DATA (update the board status at previous position to hold this new piece)
                CellStatus status = _boardStatus.GetCellStatusAtPosition(piecePosition);
                BoardCell cell = status.Cell;

                status.SetPieceID(newPieceID);

                // VISUAL - update icon only.
                cell.SetHighlightIconAsPiece(newPieceID);
            }

            if (!recentPiecePlacedOnBoard)
            {
                if (pieceButtonPreviouslyClicked) buttonsInterface.ToggleSlotButtonActiveColor(previousSlotID, false);
                buttonsInterface.ToggleSlotButtonActiveColor(currentSlotID, true);
            }

            // after all logic has been ran, update the previous slot ID.
            _placementStateData.UpdatePreviousData();
        }

        private void ResetPlacement()
        {
            _placementStateData.ResetCurrentData();

            for (int playerID = 0; playerID < _selectionStateData.PlayerCount; playerID++)
            {
                // resetting placement data and board data (plus board visuals)
                PlacementData[] playerPlacementData = _placementStateData.GetPlayerPlacementData(playerID);

                for (int slotID = 0; slotID < playerPlacementData.Length; slotID++)
                {
                    PlacementData data = playerPlacementData[slotID];

                    Coordinate position = data.Position;

                    CellStatus status = _boardStatus.GetCellStatusAtPosition(position);
                    BoardCell cell = status.Cell;

                    status.ResetIDs();

                    cell.ResetHighlightIcon();
                    cell.ResetCellColor();

                    data.ResetData();
                }

                // resetting player interface visuals
                var playerInterface = _interfaces[playerID];
                bool isFirstPlayerToChoose = playerID == _placementStateData.CurrentPlayerID;

                playerInterface.ToggleAllButtonsInteraction(isFirstPlayerToChoose);
                playerInterface.DisplayColorScheme(isFirstPlayerToChoose);
                playerInterface.ToggleAllSlotButtonActiveColor(false);
            }
        }
    }
}