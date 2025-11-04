using UnityEngine;

namespace Game
{
    public class Quitter : MonoBehaviour
    {
        public void OnPressQuit()
        {
            Application.Quit();
        }
    }
}