using UnityEngine;
using System.Collections;

public class GPSManager : MonoBehaviour
{
    public double UserLatitude;  // 위도
    public double UserLongitude; // 경도
    public int SpaceNum; // 건물

    // 장소, 시작-끝 위도, 시작-끝 경도
    private double[,,] SpaceGPS = new double[4, 2, 2];

    void Awake()
    {
        // 공학관
        SpaceGPS[0, 0, 0] = 37.6018320408996;
        SpaceGPS[0, 0, 1] = 37.601384077026474;
        SpaceGPS[0, 1, 0] = 126.95405990910596;
        SpaceGPS[0, 1, 1] = 126.9547821080415;
        // 자하관
        SpaceGPS[1, 0, 0] = 37.60124859560085;
        SpaceGPS[1, 0, 1] = 37.600807467029576;
        SpaceGPS[1, 1, 0] = 126.95392154556986;
        SpaceGPS[1, 1, 1] = 126.95484474013583;
        // 월해관
        SpaceGPS[2, 0, 0] = 37.60409196707935;
        SpaceGPS[2, 0, 1] = 37.60348412577691;
        SpaceGPS[2, 1, 0] = 126.95587613377928;
        SpaceGPS[2, 1, 1] = 126.95675981224217;
        // 미백관
        SpaceGPS[3, 0, 0] = 37.6033121297319;
        SpaceGPS[3, 0, 1] = 37.60283940788239;
        SpaceGPS[3, 1, 0] = 126.95460824117957;
        SpaceGPS[3, 1, 1] = 126.95538991962945;

        SpaceNum = 100; 
    }

    private IEnumerator Start()
    {
        /*
        // GPS 지원 여부 확인
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("GPS 사용 권한이 없거나 위치 서비스가 비활성화되어 있습니다.");
            yield break;
        }
        */
        // GPS 서비스 시작
        Input.location.Start(1f, 1f);

        // GPS 초기화 대기
        int wait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && wait > 0)
        {
            yield return new WaitForSeconds(1);
            wait--;
        }

        // 초기화 시간 초과
        if (wait <= 0)
        {
            Debug.LogError("GPS 초기화 시간 초과");
            yield break;
        }

        // GPS 오류 확인
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("GPS 위치 정보를 가져오지 못했습니다.");
            yield break;
        }
        
        // GPS 위치 정보 가져오기
        LocationInfo location = Input.location.lastData;

        UserLatitude = location.latitude;
        UserLongitude = location.longitude;


        Debug.Log("위도: " + UserLatitude);
        Debug.Log("경도: " + UserLongitude);
        
        for(int i = 0; i < 4; i++)
        {
            if(UserLatitude >= SpaceGPS[i, 0, 0] && UserLongitude >= SpaceGPS[i, 1, 0])
            {
                if(UserLatitude <= SpaceGPS[i, 1, 0] && UserLongitude <= SpaceGPS[i, 1, 1])
                {
                    SpaceNum = i;
                }
            }
        }

        Debug.Log(SpaceNum);

    }
}
