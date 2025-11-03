using UnityEngine;

namespace Game
{
    public class PiecePlacementManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlacementPiecesInterface[] _interfaces;

        [Header("Scriptable Objects")]
        [SerializeField] private GameInstanceData _gameData;
        [SerializeField] private PieceCatalog _catalog;

        [Header("Events")]
        [SerializeField] private GameEvent _clickCellEvent;

        [Header("Visualising state")]
        [SerializeField][ReadOnly] private bool _pieceSelected;
        [SerializeField][ReadOnly] private int _currentPlayerIndex;
        [SerializeField][ReadOnly] private int _currentPlayerSlotSelected;


        public void Initialize()
        {
            _currentPlayerIndex = 0;
            int playerCount = _gameData.NumberOfPlayers;

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
            Debug.Log($"slot id - {slotID}");

            _currentPlayerSlotSelected = slotID;

            // highlight the slot background with the player's color
            _interfaces[_currentPlayerIndex].HighlightSlot(_currentPlayerSlotSelected);

            // allow clicking of the grid (basically when piece selected = true)
            _pieceSelected = true;


        }

        public void OnPressConfirm_UI_BUTTON()
        {
            Debug.Log("confirm pressed.");
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
            Debug.Log($"last clicked cell: {_gameData.LastClickedCell}");
        }
    }
}