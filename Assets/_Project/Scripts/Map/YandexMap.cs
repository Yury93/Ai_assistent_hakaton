using System.Collections;
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI;
using YandexMaps;
public class YandexMap : MonoBehaviour
{ 
    [SerializeField] private RawImage map;
    [SerializeField] private Map.TypeMap typeMap;
    [SerializeField] private Map.TypeMapLayer mapLayer;
    [SerializeField] private List<Vector2> markers = new List<Vector2>();
    [SerializeField] private float lan, lon;
    [SerializeField] private int size;
    public bool isLoad;
 

    public void Init()
    {
        if(isLoad)
       LoadMap();
    }
    private void LoadMap()
    {
        Map.EnabledLayer = true;
        Map.Size = 4;
        Map.SetTypeMapLayer = mapLayer;
        Map.SetTypeMap = typeMap;
        Map.SetMarker = markers;
        Map.Longitude = lon;
        Map.Latitude = lan;
        Map.Size = size;
        Map.LoadMap();
        StartCoroutine(LoadTexture());
    }
    [ContextMenu("update")]
    public void UpdateMap()
    {
        Map.EnabledLayer = true;
        Map.Size = 4;
        Map.SetTypeMapLayer = mapLayer;
        Map.SetTypeMap = typeMap;
        Map.SetMarker = markers;
        Map.Longitude = lon;
        Map.Latitude = lan;
        Map.Size = size;
        Map.UpdateLoadMap();
        StartCoroutine(LoadTexture());
    }
    IEnumerator LoadTexture()
    {
      yield return new WaitForSeconds(1);
        map.texture = Map.GetTexture;
    }
}
