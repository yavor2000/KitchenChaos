using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocator
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initiailze()
        {
            // Initialize default service locator.
            ServiceLocator.Initiailze();

            // Register all your services next.
            ServiceLocator.Current.Register<IGameService>(new PlayerService());

            // Application is ready to start, load your main scene.
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
    }
}