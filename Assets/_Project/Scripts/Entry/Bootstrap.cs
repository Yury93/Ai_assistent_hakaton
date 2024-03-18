using Core.Configs;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

 
public class Bootstrap : MonoBehaviour
{
    [field: SerializeField] public Configs Configs { get;private set; }
    [SerializeField] private ConfigsContainer container;
    [SerializeField] private bool loadScene;
    private async void Awake()
    {
        try
        {
            await Configs.Load();
        } 
        catch (Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            container.Init();
            container.CreateConfigHakaton(Configs.hakaton);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        await UniTask.Delay(500);
        if (loadScene)
            await SceneLoader.AsyncLoadScene(SceneLoader.MAIN);
    }
}
