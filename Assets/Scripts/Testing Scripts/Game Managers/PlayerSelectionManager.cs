using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerSelectionManager : MonoBehaviour
    {
        [Header("INITIALIZATION STATUS")]
        [SerializeField][ReadOnly] private bool _initialized = false;

        [Header("Game Objects")]
        [SerializeField] private GameObject _mainDisplayObject;

        [Header("Components")]
        [SerializeField] private PlayerPieceSelectionHandler[] _pieceSelectionHandlers;
        [SerializeField] private Button[] _buttonsToDisableWhileChoosingPiece;
        [SerializeField] private TMP_Text _pieceNameText;
        [SerializeField] private TMP_Text _playerCounterText;
        [SerializeField] private Button _playButton;

        [Header("Data")]
        [SerializeField] private SelectionStateData _selectionStateData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Events")]
        [SerializeField] private GameEvent _playerAddPieceEvent;
        [SerializeField] private GameEvent _playerChoosePieceEvent;
        [SerializeField] private GameEvent _placementToSelectionTransitionEvent;

        private System.Random _randomGenerator;

        #region Manager methods

        public void Initialize()
        {
            if (_initialized)
            {
                Debug.Log($"<color=magenta>Selection Manager already initialized</color>");
                return;
            }

            Debug.Log("<color=lime>Selection Manager Initialized</color>");
            _initialized = true;

            _randomGenerator = new();
        }

        public void Enable()
        {
            Debug.Log("<color=lime>Selection Manager Enabled</color>");

            // NEED TO DO SOME ADDITIONAL CHECKS HERE.

            if (!_selectionStateData.PlayerConfigAvailable)
            {
                InitializePlayerSelectionData();
                SetPlayable(false);
            }


            _pieceNameText.text = "";

            _playerAddPieceEvent.AddEvent(OnPressAddPiece);
            _playerChoosePieceEvent.AddEvent(OnPressChoosePiece);
        }

        public void Disable()
        {
            // in case there's a currently selected player's slot, disable it (visually)
            if (_selectionStateData.RecentPlayerID != Defaults.PLAYER_ID)
            {
                _pieceSelectionHandlers[_selectionStateData.RecentPlayerID].SetInactiveColor(_selectionStateData.RecentSlotID);
            }

            Debug.Log("<color=lime>Selection Manager Disabled</color>");

            _selectionStateData.ResetCurrentSelections();

            _playerAddPieceEvent.RemoveEvent(OnPressAddPiece);
            _playerChoosePieceEvent.RemoveEvent(OnPressChoosePiece);
        }

        public void Show()
        {
            Debug.Log($"<color=lime>Showing Selection Manager</color>");
            _mainDisplayObject.SetActive(true);
        }

        public void Hide()
        {
            Debug.Log($"<color=lime>Hiding Selection Manager</color>");
            if (_mainDisplayObject == null) return; //sometimes when exiting playmode, this object gets destroyed.
            _mainDisplayObject.SetActive(false);
        }

        #endregion

        #region Unity Event Button Methods

        public void OnIncreasePlayerCount_UI_BUTTON()
        {
            if (_selectionStateData.PlayerCountMaximized) return;

            int newPlayerIndex = _selectionStateData.PlayerCount;

            // update selection state data
            _selectionStateData.IncreasePlayerCountBy1();

            // update visuals (set counter +1 and show next player interface)
            _playerCounterText.text = _selectionStateData.PlayerCount.ToString();

            _pieceSelectionHandlers[newPlayerIndex].Show();

            // when increasing player count, the new player will always have no pieces selected by default, so cannot play.
            SetPlayable(false);
        }

        public void OnDecreasePlayerCount_UI_BUTTON()
        {
            if (_selectionStateData.PlayerCountMinimized) return;

            // update selection state data
            _selectionStateData.DecreasePlayerCountBy1();

            int playerID = _selectionStateData.RecentPlayerID;
            int currentPlayerIndex = _selectionStateData.PlayerCount;

            // edge case - if a player's slot is selected, and this slot is part of the player's interface to hide, then need to reset currenlty selected IDs.
            if (playerID != Defaults.PLAYER_ID && playerID >= currentPlayerIndex)
            {
                _selectionStateData.ResetCurrentSelections();
            }

            // update visuals (set counter -1 and hide (AND RESET) next player interface)
            _playerCounterText.text = _selectionStateData.PlayerCount.ToString();

            var visualHandler = _pieceSelectionHandlers[currentPlayerIndex];
            visualHandler.Hide();
            visualHandler.ClearSelectedPieces();

            // check if can play
            CheckIfCanPlay();
        }

        public void OnPressPlay_UI_BUTTON()
        {
            //invoke event to transition from selection to placement.
           _placementToSelectionTransitionEvent.Raise();
        }

        public void OnPressReset_UI_BUTTON()
        {
            ResetPieces();
        }

        public void OnPressRandomize_UI_BUTTON()
        {
            // reset the pieces first.
            ResetPieces();

            // reset current selections (if any)
            _selectionStateData.ResetCurrentSelections();

            for (int playerID = 0; playerID < _selectionStateData.PlayerCount; playerID++)
            {
                for (int slotID = 0; slotID < _selectionStateData.PiecesPerPlayer; slotID++)
                {
                    // generate random piece ID
                    int randomPieceID = _randomGenerator.Next(0, _catalog.NumberOfPieces);

                    // update state data
                    _selectionStateData.UpdatePlayerConfig(playerID, slotID, randomPieceID);

                    // update visuals
                    _pieceSelectionHandlers[playerID].SetPieceIcon(slotID, randomPieceID);
                }
            }

            // by randomizing, all pieces will be chosen and therefore placement is immediately possible.
            SetPlayable(true);
        }

        public void OnPressQuit_UI_BUTTON()
        {
            Debug.Log("<color=red>PRESSED QUIT BUTTON - WORK IN PROGRESS</color>");
        }

        #endregion

        #region Private methods

        private void InitializePlayerSelectionData()
        {
            _selectionStateData.InitializePlayerConfigs();

            int playerCount = _selectionStateData.PlayerCount;

            _playerCounterText.text = playerCount.ToString();

            // hide remaining player interfaces.
            for (int i = playerCount; i < _pieceSelectionHandlers.Length; i++)
            {
                _pieceSelectionHandlers[i].Hide();
            }
        }

        private void ResetPieces()
        {
            // reset current selections (if there are any)
            _selectionStateData.ResetCurrentSelections();

            // reset ALL data in the selection state
            _selectionStateData.ResetPlayerData();

            // update ALL visuals
            for (int i = 0; i < _selectionStateData.PlayerCount; i++)
            {
                _pieceSelectionHandlers[i].ClearSelectedPieces();
            }

            // set play button as interactable since players cannot play until all pieces are chosen.
            SetPlayable(false);
        }

        private void OnPressAddPiece()
        {
            if (_selectionStateData.HasPreviousSelection)
            {
                // discolor previous selection
                _pieceSelectionHandlers[_selectionStateData.PreviousPlayerID].SetInactiveColor(_selectionStateData.PreviousSlotID);
            }

            // color recent selection
            _pieceSelectionHandlers[_selectionStateData.RecentPlayerID].SetActiveColor(_selectionStateData.RecentSlotID);

            // update previous IDs to state data.
            _selectionStateData.UpdatePreviousIDs();
        }

        private void OnPressChoosePiece()
        {
            int playerID = _selectionStateData.RecentPlayerID;
            if (playerID == Defaults.PLAYER_ID) return;

            Debug.Log("clicked on choose piece.");
            // update the selection state data with the choice this player made.
            _selectionStateData.UpdatePlayerConfigWithRecentChoices();

            // update the visuals for the player's interface (set piece icon at player ID at slot chosen AND reset its color)
            int slotID = _selectionStateData.RecentSlotID;
            int pieceID = _selectionStateData.RecentPieceID;

            var visualHandler = _pieceSelectionHandlers[playerID];
            visualHandler.SetPieceIcon(slotID, pieceID);
            visualHandler.SetInactiveColor(slotID);

            // reset selection state data (recent/previous clicked stuff)
            _selectionStateData.ResetCurrentSelections();

            CheckIfCanPlay();
        }

        private void CheckIfCanPlay()
        {
            SetPlayable(_selectionStateData.IsEveryPlayerReady());
        }

        private void SetPlayable(bool playable)
        {
            _playButton.interactable = playable;
        }

        #endregion
    }
}