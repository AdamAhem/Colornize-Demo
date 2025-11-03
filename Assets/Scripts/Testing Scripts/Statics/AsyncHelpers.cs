using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public static class AsyncHelpers
    {
        public static async Task Wait(float seconds)
        {
            float elapsedTime = 0f;
            while (elapsedTime < seconds)
            {
                await Task.Yield();
                elapsedTime += Time.deltaTime;
            }
        }

        public static async Task WaitUnscaled(float seconds)
        {
            float elapsedTime = 0f;
            while (elapsedTime < seconds)
            {
                await Task.Yield();
                elapsedTime += Time.unscaledDeltaTime;
            }
        }
    }
}