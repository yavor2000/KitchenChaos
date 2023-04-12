using UnityEngine;
using UnityEngine.SceneManagement;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initiailze()
    {
        Debug.Log("Bootstrapper init");
        // Initialize default service locator.
        // ServiceLocator.Initiailze();

        // Register all your services next.
        
        // ServiceLocator.Current.Register<Player>(new Player());

        // Application is ready to start, load your main scene.
        // SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
    }
}
