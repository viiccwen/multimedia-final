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
        // �ù��ѪR��
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

        // ��l��Kinect
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

        // ��l�ƿù����߫���
        if (centerPointerUI != null)
        {
            centerPointerUI.position = screenCenter;
        }
        else
        {
            Debug.LogWarning("Center Pointer UI ���]�m�I");
        }

        if (handPointerUI == null)
        {
            Debug.LogWarning("Hand Pointer UI ���]�m�I");
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

                    // colorPoint.X, colorPoint.Y �i��|�O NaN�]��l�ܤ���ɡ^
                    if (float.IsInfinity(colorPoint.X) || float.IsInfinity(colorPoint.Y))
                    {
                        handPointerUI.gameObject.SetActive(false);
                    }
                    else
                    {
                        handPointerUI.gameObject.SetActive(true);

                        // �N�m��e���y���ഫ���ù��y��
                        float screenX = Mathf.Clamp(colorPoint.X / 1920f * screenWidth, 0, screenWidth);
                        float screenY = Mathf.Clamp((1 - (colorPoint.Y / 1080f)) * screenHeight, 0, screenHeight);
                        Vector2 screenPos =  new Vector2(screenX, screenY);

                        handPointerUI.position = screenPos;
                    }

                    // Debug Console ��X
                    Debug.Log($"����ù��y�Ц�m: X = {colorPoint.X}, Y = {colorPoint.Y}");
                    Debug.Log($"�P���d�򤤤��I: X = {screenCenter.x}, Y = {screenCenter.y}");
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
