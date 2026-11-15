using TMPro;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GPSManager gpsManager;
    public TMP_Text mapText;
    
    private void Update()
    {
        if (gpsManager.SpaceNum < 0 || gpsManager.SpaceNum > 99)
        {
            mapText.text = "no signal";
        }

        else if (gpsManager.SpaceNum == 0)
        {
            mapText.text = "signal from Space0";
        }
        
        else if (gpsManager.SpaceNum == 1)
        {
            mapText.text = "signal from Space1";
        }

        else if (gpsManager.SpaceNum == 2)
        {
            mapText.text = "signal from Space2";
        }

        else if (gpsManager.SpaceNum == 3)
        {
            mapText.text = "signal from Space3";
        }
    }
}
