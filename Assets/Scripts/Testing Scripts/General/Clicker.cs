using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class Clicker : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("clicked successfully");
            Debug.Log(eventData);
        }
    }
}