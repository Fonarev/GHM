using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameMains.Scripts
{
    public class LoaderScenes 
    {
        public void LoadLevel(string name)
        {
            //anim.Play("CloseDoors");
            HandlerCoroutine.StartRoutine(LoadLevelAsync(name));
        }

        private IEnumerator LoadLevelAsync(string name)
        {
            yield return new WaitForSeconds(0.5f);

            AsyncOperation operation = SceneManager.LoadSceneAsync(name);

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