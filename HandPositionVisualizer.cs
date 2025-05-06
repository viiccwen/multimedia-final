using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class HandPositionVisualizer : MonoBehaviour
{
    private KinectSensor sensor;
    private BodyFrameReader bodyReader;
    private Body[] bodies;

    private int screenWidth;
    private int screenHeight;
    private Vector2 screenCenter;

    public RectTransform handPointerUI;
    public RectTransform centerPointerUI;

    void Start()
    {
        // 螢幕解析度
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

        // 初始化Kinect
        sensor = KinectSensor.GetDefault();

        if (sensor != null)
        {
            bodyReader = sensor.BodyFrameSource.OpenReader();
            if (!sensor.IsOpen)
            {
                sensor.Open();
            }
        }

        // 初始化螢幕中心指標
        if (centerPointerUI != null)
        {
            centerPointerUI.position = screenCenter;
        }
        else
        {
            Debug.LogWarning("Center Pointer UI 未設置！");
        }

        if (handPointerUI == null)
        {
            Debug.LogWarning("Hand Pointer UI 未設置！");
        }
    }

    void Update()
    {
        if (bodyReader == null) return;

        var frame = bodyReader.AcquireLatestFrame();
        if (frame == null) return;

        if (bodies == null)
        {
            bodies = new Body[sensor.BodyFrameSource.BodyCount];
        }

        frame.GetAndRefreshBodyData(bodies);
        frame.Dispose();
        frame = null;

        foreach (var body in bodies)
        {
            if (body != null && body.IsTracked)
            {
                Windows.Kinect.Joint handTip = body.Joints[JointType.HandTipRight];

                if (handTip.TrackingState == TrackingState.Tracked)
                {
                    CameraSpacePoint position = handTip.Position;

                    // 轉成 Unity 世界座標
                    float scaleFactor = 25.0f; // 調大一點（3 ~ 5 自己試看看最舒服的值）

                    // 原始位置乘上倍率
                    Vector3 unityPos = new Vector3(position.X * scaleFactor, position.Y * scaleFactor, position.Z);

                    // 轉換螢幕座標
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(unityPos);

                    // 檢查 Z 軸，Z < 0 時不顯示
                    if (screenPos.z < 0)
                    {
                        Debug.Log("手指在攝影機後方，不顯示");
                        if (handPointerUI != null)
                            handPointerUI.gameObject.SetActive(false);
                        continue;
                    }
                    else
                    {
                        if (handPointerUI != null)
                            handPointerUI.gameObject.SetActive(true);
                    }

                    // Clamp 防止超出螢幕
                    screenPos.x = Mathf.Clamp(screenPos.x, 0, screenWidth);
                    screenPos.y = Mathf.Clamp(screenPos.y, 0, screenHeight);

                    // 更新 UI 位置
                    if (handPointerUI != null)
                    {
                        handPointerUI.position = screenPos;
                    }

                    // Debug Console 輸出
                    Debug.Log($"手指螢幕座標位置: X = {screenPos.x}, Y = {screenPos.y}");
                    Debug.Log($"感測範圍中心點: X = {screenCenter.x}, Y = {screenCenter.y}");
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (bodyReader != null)
        {
            bodyReader.Dispose();
            bodyReader = null;
        }

        if (sensor != null && sensor.IsOpen)
        {
            sensor.Close();
            sensor = null;
        }
    }
}
