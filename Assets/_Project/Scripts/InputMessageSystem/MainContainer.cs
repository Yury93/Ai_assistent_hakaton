using Spine.Unity;
using System.Linq;
using UnityEngine;

public class MainContainer : MonoBehaviour
{
    [field:SerializeField] public InputMessageSytem InputMessageSystem { get;private set; }
    [field: SerializeField] public YandexMap YandexMap { get; private set; }
    [field: SerializeField] public TextToSpeech TextToSpeech { get; private set; }
    [field: SerializeField] public MenuWindow MenuWindow { get; private set; }
    [field: SerializeField] public CharController CharController { get; private set; }
    public Spine.Unity.SkeletonGraphic skeletonGraphic;
    public GameObject dojd, snow;
    public ChatHandler ChatHandler { get; private set; }
    public static MainContainer instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ChatHandler = new ChatHandler(this);
        ChatHandler.Init();
        InputMessageSystem.Init();
        YandexMap.Init();
        TextToSpeech.Init();
        MenuWindow.Init();
        CharController.Init();
        var config = ConfigsContainer.Instance?.Hakaton?.entities?.Where(t=>t.TypeConfig == "whether")?.ToList();
        if (config != null)
        {
            if (config[0].Voice == "none")
            { snow.gameObject.SetActive(false); dojd.gameObject.SetActive(false); skeletonGraphic.initialSkinName = "skin_1"; }
            if (config[0].Voice == "winter")
            { snow.gameObject.SetActive(true); skeletonGraphic.initialSkinName = "skin_2"; }
            if (config[0].Voice == "rain")
            { dojd.gameObject.SetActive(true); skeletonGraphic.initialSkinName = "skin_3"; }

            Debug.Log(config[0].Voice);
        }
        else
        skeletonGraphic.initialSkinName = "skin_2";
    }
    private void OnDestroy()
    {
        ChatHandler?.Dispose();
    }
}
