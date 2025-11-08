using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerSelectionManager : MonoBehaviour
    {
        [Header("INITIALIZATION STATUS")]
        [SerializeField][ReadOnly] private bool _initialized = false;

        [Header("Submanagers")]
        [SerializeField] private PlayerCountSelectorHandler _playerCountHandler;
        [SerializeField] private PlayerPieceSelectionHandler[] _pieceSelectionHandlers;

        [Header("Game Objects")]
        [SerializeField] private GameObject _mainDisplayObject;

        [Header("Components")]
        [SerializeField] private Button[] _buttonsToDisableWhileChoosingPiece;
        [SerializeField] private TMP_Text _pieceNameText;
        [SerializeField] private Button _playButton;

        [Header("Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private SelectionStateData _selectionStateData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Events")]
        [SerializeField] private GameEvent _playerAddPieceEvent;
        [SerializeField] private GameEvent _playerChoosePieceEvent;
        [SerializeField] private GameEvent _placementToSelectionTransitionEvent;

        private System.Random _randomGenerator;

        public void Initialize()
        {
            if (_initialized)
            {
                Debug.Log($"<color=magenta>Selection Manager already initialized</color>");
                return;
            }

            Debug.Log("<color=lime>Selection Manager Initialized</color>");
            _initialized = true;

            _playerCountHandler.Initialize();
            _pieceNameText.text = "";
            _playButton.interactable = false;

            _randomGenerator = new();
        }

        public void Enable()
        {
            Debug.Log("<color=lime>Selection Manager Enabled</color>");

            _playerAddPieceEvent.AddEvent(OnPressAddPiece);
            _playerChoosePieceEvent.AddEvent(OnPressChoosePiece);
        }

        public void Disable()
        {
            Debug.Log("<color=lime>Selection Manager Disabled</color>");

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

        public void OnIncreasePlayerCount_UI_BUTTON()
        {
            CheckIfCanPlay();
        }

        public void OnDecreasePlayerCount_UI_BUTTON()
        {
            CheckIfCanPlay();
        }

        public void OnPressPlay_UI_BUTTON()
        {
            // invoke event to transition from selection to placement.
            _placementToSelectionTransitionEvent.Raise();
        }

        public void OnPressReset_UI_BUTTON()
        {
            ResetPieces();
        }

        public void OnPressRandomize_UI_BUTTON()
        {
            // randomize the pieces for each player (need to reset first)
            ResetPieces();

            _selectionStateData.ResetData();

            for (int playerID = 0; playerID < _gameData.NumberOfPlayers; playerID++)
            {
                for (int slotID = 0; slotID < _gameData.PiecesPerPlayer; slotID++)
                {
                    int randomPieceID = _randomGenerator.Next(0, _catalog.NumberOfPieces);

                    _pieceSelectionHandlers[playerID].OnPieceChosen(slotID, _catalog.Get(randomPieceID).Icon);
                    _gameData.UpdatePlayerConfig(playerID, slotID, randomPieceID);
                }
            }

            // by randomizing, all pieces will be chosen and therefore placement is immediately possible.
            _playButton.interactable = true;
        }

        public void OnPressQuit_UI_BUTTON()
        {
            Debug.Log("<color=red>PRESSED QUIT BUTTON - WORK IN PROGRESS</color>");
        }

        private void ResetPieces()
        {
            // clear all of the pieces in the pre-game config
            _gameData.GenerateNewPlayerConfigs(Defaults.MAX_PLAYERS);
            _gameData.SetNumberOfPlayers(_gameData.NumberOfPlayers);

            // update the UI so that it shows that every piece is clear.
            for (int i = 0; i < _pieceSelectionHandlers.Length; i++)
            {
                _pieceSelectionHandlers[i].ClearChosenPieces();
            }

            // set play button as interactable since players cannot play until all pieces are chosen.
            _playButton.interactable = false;
        }

        private void OnPressAddPiece()
        {
            // disable increment buttons and play button
            for (int i = 0; i < _buttonsToDisableWhileChoosingPiece.Length; i++)
            {
                _buttonsToDisableWhileChoosingPiece[i].interactable = false;
            }
        }

        private void OnPressChoosePiece()
        {
            int playerID = _selectionStateData.PlayerID;
            int slotID = _selectionStateData.SlotID;
            int pieceID = _selectionStateData.PieceID;
            Sprite pieceSprite = _selectionStateData.PieceSprite;

            _pieceSelectionHandlers[playerID].OnPieceChosen(slotID, pieceSprite);
            _gameData.UpdatePlayerConfig(playerID, slotID, pieceID);

            for (int i = 0; i < _buttonsToDisableWhileChoosingPiece.Length; i++)
            {
                _buttonsToDisableWhileChoosingPiece[i].interactable = true;
            }

            CheckIfCanPlay();
        }

        private void CheckIfCanPlay()
        {
            bool playersReady = _gameData.PlayerSlotsAreFilled();
            _playButton.interactable = playersReady;
        }
    }
}