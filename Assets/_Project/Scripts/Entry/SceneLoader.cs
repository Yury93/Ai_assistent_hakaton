using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR

#endif
public class SceneLoader
{ 
    public const string MAIN = "Main";
    public static Action onChangeScene;
    public static async UniTask AsyncLoadScene(string nameScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;
        onChangeScene?.Invoke();
        while (operation.progress < 0.9f)
        {
            await UniTask.Yield();
        } 
        operation.allowSceneActivation = true; 
    }
    public static async UniTask UnloadAdditiveScene(string nameScene)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(nameScene);
        operation.allowSceneActivation = false;
        onChangeScene?.Invoke();
        while (operation.progress < 0.9f)
        {
            await UniTask.Yield();
        }
        operation.allowSceneActivation = true; 
    }
    public static List<Scene> GetActiveScenes()
    { 
        List<Scene> result = new List<Scene>();
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i); 
            if(scene == SceneManager.GetActiveScene())
            {
                result.Add(scene);
            }
        }
        return result;
    }
}
