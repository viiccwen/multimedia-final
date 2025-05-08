using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class HandPositionVisualizer : MonoBehaviour
{
    private KinectSensor sensor;
    private BodyFrameReader bodyReader;
    private Body[] bodies;
    private CoordinateMapper coordinateMapper;

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
            coordinateMapper = sensor.CoordinateMapper;

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

                    ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(position);

                    // colorPoint.X, colorPoint.Y 可能會是 NaN（當追蹤不到時）
                    if (float.IsInfinity(colorPoint.X) || float.IsInfinity(colorPoint.Y))
                    {
                        handPointerUI.gameObject.SetActive(false);
                    }
                    else
                    {
                        handPointerUI.gameObject.SetActive(true);

                        // 將彩色畫面座標轉換成螢幕座標
                        float screenX = Mathf.Clamp(colorPoint.X / 1920f * screenWidth, 0, screenWidth);
                        float screenY = Mathf.Clamp((1 - (colorPoint.Y / 1080f)) * screenHeight, 0, screenHeight);
                        Vector2 screenPos =  new Vector2(screenX, screenY);

                        handPointerUI.position = screenPos;
                    }

                    // Debug Console 輸出
                    Debug.Log($"手指螢幕座標位置: X = {colorPoint.X}, Y = {colorPoint.Y}");
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
