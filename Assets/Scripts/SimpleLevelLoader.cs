//   Simple Event-Based Level Loader
//   By ThrowLab Games
//   January 2026

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThrowLab.Systems.Utilities
{
    public class SimpleLevelLoader : MonoBehaviour
    {
        /* =================[#]  CONFIGURATION  [#]================= */

        [Header("Level Selection")]
        [SerializeField] private string levelName;

        [Header("Debug Settings")]
        [SerializeField] private bool showInfoLogs;

        /* =================[#]  INTERNAL VALUES/REFERENCES  [#]================= */

        // Internal references
        private AsyncOperation asyncLevelLoad;

        /* =================[#]  LEVEL LOADER API  [#]================= */

        public void StartBackgroundLoad()
        {
            asyncLevelLoad = SceneManager.LoadSceneAsync(levelName);
            if (asyncLevelLoad == null) // If the scene is not found
            {
                Debug.LogError($"[ERROR] SimpleLevelLoader: Specified level name \"{levelName}\" is not valid! Unable to load level!");
                return;
            }

            asyncLevelLoad.allowSceneActivation = false;
            if (showInfoLogs) Debug.Log($"[INFO] SimpleLevelLoader: Now loading level \"{levelName}\" in the background.");
        }

        public void LoadLevel()
        {
            if (asyncLevelLoad == null)
            {
                Debug.LogError("[ERROR] SimpleLevelLoader: A level has not been preloaded and therefore cannot be loaded.\n" +
                    "Make sure you've called StartBackgroundLoad() with a valid level name before calling this function.");
                return;
            }

            StartCoroutine(WaitForLevelLoadFinish());
        }

        /* =================[#]  SEQUENCE COROUTINES  [#]================= */

        private IEnumerator WaitForLevelLoadFinish()
        {
            yield return new WaitUntil(() => asyncLevelLoad.progress >= 0.9f);
            if (showInfoLogs) Debug.Log($"[INFO] SimpleLevelLoader: Loading queued level \"{levelName}\" now.");
            asyncLevelLoad.allowSceneActivation = true;
        }
    }
}