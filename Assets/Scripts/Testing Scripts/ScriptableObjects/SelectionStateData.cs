using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SelectionStateData", menuName = "SO/SelectionStateData")]
    public class SelectionStateData : ScriptableObject
    {
        [Header("Events")]
        [SerializeField] private GameEvent _addPieceToPlayerEvent;
        [SerializeField] private GameEvent _choosePieceEvent;

        [Space(25)]

        [Header("Current State")]
        [SerializeField][ReadOnly] private bool _playerAddingPiece = false;
        [SerializeField][ReadOnly] private int _currentPlayerID;
        [SerializeField][ReadOnly] private int _currentSlotD;
        [SerializeField][ReadOnly] private int _currentPieceID;
        [SerializeField][ReadOnly] private Sprite _pieceSprite;

        public int PlayerID => _currentPlayerID;
        public int SlotID => _currentSlotD;
        public int PieceID => _currentPieceID;

        public Sprite PieceSprite => _pieceSprite;

        public bool PlayerAddingPiece => _playerAddingPiece;

        public void ResetData()
        {
            _playerAddingPiece = false;
            _currentPlayerID = 0;
            _currentSlotD = 0;
        }

        public void SetPlayerAndPieceSelected(int playerID, int pieceSlotID)
        {
            if (_playerAddingPiece)
            {
                Debug.Log("A player is currently choosing a piece. Cannot allow this new player do to so until they make a choice.");
                return;
            }

            _playerAddingPiece = true;
            _currentPlayerID = playerID;
            _currentSlotD = pieceSlotID;

            _addPieceToPlayerEvent.Raise();
        }

        public void ChoosePieceForSelectedPlayer(int pieceID, Sprite pieceSprite)
        {
            // check if this is valid first. if so, then raise choose piece event.

            if (!_playerAddingPiece)
            {
                Debug.Log("No player is currently choosing a piece.");
                return;
            }

            _currentPieceID = pieceID;
            _pieceSprite = pieceSprite;

            _choosePieceEvent.Raise();

            ResetData();
        }
    }
}