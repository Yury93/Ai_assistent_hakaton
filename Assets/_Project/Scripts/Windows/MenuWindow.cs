using JetBrains.Annotations;
using System; 
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow  : Window
{
    [SerializeField] private Button transportButton, mapButton, travelButton, gamesButton;
    [SerializeField] private TransportWindow transportWindow;
    [SerializeField] private TravelWindow travelWindow;
    [SerializeField] private MapWindow mapWindow;
    [SerializeField] private GameWindow gameWindow;
    [SerializeField] private GameObject panelMenu, ScrollRect,audioRec;
    public GameWindow GameWindow => gameWindow;
    public void Init()
    {
        transportButton.onClick.AddListener(transportWindow.OpenWindow);
        travelButton.onClick.AddListener(travelWindow.OpenWindow);
        mapButton.onClick.AddListener(mapWindow.OpenWindow);
        gamesButton.onClick.AddListener(gameWindow.OpenWindow);

        transportWindow.onOpen += OnOpenChildWindow;
        mapWindow.onOpen += OnOpenChildWindow;
        gameWindow.onOpen += OnOpenChildWindow;
        travelWindow.onOpen += OnOpenChildWindow;

        transportWindow.onClose += OnCloseChildWindow;
        mapWindow.onClose += OnCloseChildWindow;
        gameWindow.onClose += OnCloseChildWindow;
        travelWindow.onClose += OnCloseChildWindow;

        panelMenu.gameObject.SetActive(true);
        ScrollRect.gameObject.SetActive(true);

        transportWindow.gameObject.SetActive(false);
        mapWindow.gameObject.SetActive(false);
        gameWindow.gameObject.SetActive(false);
        travelWindow.gameObject.SetActive(false);

        gameWindow.Init();
        mapWindow.Init();
    }

    private void OnOpenChildWindow(Window window)
    {
        panelMenu.gameObject.SetActive(false);
            ScrollRect.gameObject.SetActive(false);
        audioRec.gameObject.SetActive(false);   
    }
    private void OnCloseChildWindow(Window window)
    {
        panelMenu.gameObject.SetActive(true);
        ScrollRect.gameObject.SetActive(true);
        audioRec.gameObject.SetActive(true);
    }
}
public class Window : MonoBehaviour
{
    public bool IsOpened { get; private set; }
    public Action<Window> onOpen;
    public Action<Window> onClose;
    public virtual void OpenWindow()
    {
        gameObject.SetActive(true);
        IsOpened = true;
        onOpen?.Invoke(this);
       
    }
    public virtual void CloseWindow()
    {
        gameObject.SetActive(false);
        IsOpened = true;
        onClose?.Invoke(this);
    }
}
