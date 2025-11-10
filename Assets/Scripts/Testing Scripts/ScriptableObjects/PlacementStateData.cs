using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlacementData", menuName = "SO/Placement Data")]
    public class PlacementStateData : ScriptableObject
    {
        //private enum PlacementState { Initialized, ChoosingPiece, ChoosingCell, AwaitingConfirm, Ready }
        [Header("Defaults")]
        [SerializeField] private int _firstPlayerID;

        [Header("Data")]
        [SerializeField] private PieceCatalog _catalog;

        [Header("Visualisation. <color=red>DO NOT MODIFY.</color>")]
        [SerializeField] private PlacementData[][] _placementData;
        [SerializeField][ReadOnly] private int _currentPlayerID;
        [SerializeField][ReadOnly] private int _recentlySelectedSlotID;
        [SerializeField][ReadOnly] private int _previouslySelectedSlotID;
        [SerializeField][ReadOnly] private int _piecesPlaced;
        [SerializeField][ReadOnly] private int _expectedPieces;

        public int CurrentPlayerID => _currentPlayerID;
        public int RecentSlotID => _recentlySelectedSlotID;
        public int PreviousSlotID => _previouslySelectedSlotID;
        public int PiecesPlaced => _piecesPlaced;
        public int ExpectedPieces => _expectedPieces;

        public void SetCurrentPlayerID(int playerID) => _currentPlayerID = playerID;

        public void ResetCurrentData()
        {
            _currentPlayerID = _firstPlayerID;
            ResetRecentClicksData();
            ResetPiecesPlaced();
        }

        public void ResetRecentClicksData()
        {
            _recentlySelectedSlotID = Defaults.SLOT_ID;
            _previouslySelectedSlotID = Defaults.SLOT_ID;
        }

        public void IncrementPiecesPlacecBy1()
        {
            _piecesPlaced++;
        }

        public void ResetPiecesPlaced()
        {
            _piecesPlaced = 0;
        }

        public void InitializePlacementData(SelectionStateData selectionData)
        {
            var selectionDataList = selectionData.SelectionData;

            int playerCount = selectionData.PlayerCount;
            int pieceCount = selectionData.PiecesPerPlayer;

            // initialize new placement data jagged array (1 for each player)
            _placementData = new PlacementData[playerCount][];

            for (int playerID = 0; playerID < playerCount; playerID++)
            {
                PlayerSelectionData playerSelectionData = selectionDataList[playerID];

                // initialize new placement data array (1 for each slot)
                _placementData[playerID] = new PlacementData[pieceCount];

                for (int slotID = 0; slotID < pieceCount; slotID++)
                {
                    int pieceID = playerSelectionData.PieceIDs[slotID];
                    _placementData[playerID][slotID] = new PlacementData(pieceID);
                }
            }

            _expectedPieces = playerCount * pieceCount;
        }

        public void UpdateLastClickedPiecePlacementButton(int slotID)
        {
            _recentlySelectedSlotID = slotID;
        }

        public void UpdatePreviousData()
        {
            _previouslySelectedSlotID = _recentlySelectedSlotID;
        }

        public void SetPlayerPieceAtSlotAsPlaced(int playerID, int slotID, Coordinate placedPosition)
        {
            var data = _placementData[playerID][slotID];

            data.SetAsPlaced(true);
            data.SetPosition(placedPosition);
        }

        public PlacementData[] GetPlayerPlacementData(int playerID) => _placementData[playerID];

        public int GetPlayerPieceAtSlot(int playerID, int slotID) => _placementData[playerID][slotID].PieceID;

        public bool IsRecentPlayerPiecePlaced()
        {
            return _placementData[_currentPlayerID][_recentlySelectedSlotID].IsPlaced;
        }

        public bool IsPreviousPlayerPiecePlaced()
        {
            if (_previouslySelectedSlotID == Defaults.SLOT_ID) return false;
            return _placementData[_currentPlayerID][_previouslySelectedSlotID].IsPlaced;
        }

        public bool IsPiecePlaced(int playerID, int slotID) => GetPlayerPlacementData(playerID)[slotID].IsPlaced;
    }
}