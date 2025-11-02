using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerSelectionManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerCountSelectorHandler _playerCountHandler;
        [SerializeField] private Button[] _buttonsToDisableWhileChoosingPiece;
        [SerializeField] private PlayerPieceSelectionHandler[] _pieceSelectionHandlers;
        [SerializeField] private TMP_Text _pieceNameText;

        [Header("Events")]
        [SerializeField] private GameEvent _playerAddPieceEvent;
        [SerializeField] private GameEvent _playerChoosePieceEvent;

        [Header("Data")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private SelectionStateData _selectionStateData;

        public void Initialize()
        {
            _playerCountHandler.Initialize();
            _pieceNameText.text = "";
        }

        public void Enable()
        {
            Debug.Log("Player Selection Manager Enabled");

            _playerAddPieceEvent.AddEvent(OnPressAddPiece);
            _playerChoosePieceEvent.AddEvent(OnPressChoosePiece);
        }

        public void Disable()
        {
            Debug.Log("Player Selection Manager Disabled");

            _playerAddPieceEvent.RemoveEvent(OnPressAddPiece);
            _playerChoosePieceEvent.RemoveEvent(OnPressChoosePiece);
        }

        private void OnPressAddPiece()
        {
            // disable increment buttons and play button
            for (int i = 0; i < _buttonsToDisableWhileChoosingPiece.Length; i++)
            {
                _buttonsToDisableWhileChoosingPiece[i].enabled = false;
            }
        }

        private void OnPressChoosePiece()
        {
            // update the game instance data on the player slots.
            // each player has 3 slots -> update the current index with the piece index.
            
            int playerID = _selectionStateData.PlayerID;
            int slotID = _selectionStateData.SlotID;
            int pieceID = _selectionStateData.PieceID;
            Sprite pieceSprite = _selectionStateData.PieceSprite;

            // here, need to reset the BUTTON COLOR of the player slot that was selected.
            _pieceSelectionHandlers[playerID].OnPieceChosen(slotID, pieceSprite);

            Debug.Log($"Player {playerID + 1} has added piece no.{pieceID + 1} to slot {slotID + 1}.");

            _gameData.UpdatePlayerConfig(playerID, slotID, pieceID);


            // enable increment and [IF ENOUGH PIECES ARE SELECTED] play button
            for (int i = 0; i < _buttonsToDisableWhileChoosingPiece.Length; i++)
            {
                _buttonsToDisableWhileChoosingPiece[i].enabled = true;
            }
        }
    }
}