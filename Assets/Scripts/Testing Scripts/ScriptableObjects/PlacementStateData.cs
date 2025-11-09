using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlacementData", menuName = "SO/Placement Data")]
    public class PlacementStateData : ScriptableObject
    {
        private enum PlacementState { Initialized, ChoosingPiece, ChoosingCell, AwaitingConfirm, Ready }

        [Header("Data")]
        [SerializeField] private PieceCatalog _catalog;

        [Header("Visualisation. <color=red>DO NOT MODIFY.</color>")]
        [SerializeField] private PlacementData[][] _placementDataStatus;
        [SerializeField][ReadOnly] private int _currentPlayerIndex;
        [SerializeField][ReadOnly] private int _currentSlotIndex;
        [SerializeField][ReadOnly] private PlacementState _currentState;

        public void InitializePlacementData(SelectionStateData selectionData)
        {
            var selectionDataList = selectionData.SelectionData;

            for (int playerID = 0; playerID < selectionDataList.Count; playerID++)
            {
                for (int slotID = 0; slotID < selectionDataList[playerID].PieceIDs.Length; slotID++)
                {
                    Debug.Log(selectionDataList[playerID].PieceIDs[slotID]);
                }
            }
        }
    }
}