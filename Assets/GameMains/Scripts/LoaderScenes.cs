using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameMains.Scripts
{
    public class LoaderScenes : MonoBehaviour
    {
        public void LoadLevel(int sceneIndex)
        {
            //anim.Play("CloseDoors");
            StartCoroutine(LoadLevelAsync(sceneIndex));
        }

        private IEnumerator LoadLevelAsync(int sceneIndex)
        {
            yield return new WaitForSeconds(0.5f);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                //loadBar.progress = 1 - progress;
                yield return null;
                //loadBar.saveRotation();
            }
            //anim.Play("OpenDoors");
        }
    }
}