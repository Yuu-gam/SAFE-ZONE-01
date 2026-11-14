using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GPSManager gpsManager;
    public GameObject mapObject;

    private void Start()
    {
        mapObject.SetActive(false);
    }

    private void Update()
    {
        if(gpsManager.SpaceNum < 0 || gpsManager.SpaceNum > 99)
        {
            mapObject.SetActive(true);
        }
    }
}
