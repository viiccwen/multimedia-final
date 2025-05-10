using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class HandPositionVisualizer : MonoBehaviour
{
    private KinectSensor sensor;
    private BodyFrameReader bodyReader;
    private Body[] bodies;
    private CoordinateMapper coordinateMapper;
    public CursorAdapter cursorAdapter;

    private int screenWidth;
    private int screenHeight;
    private Vector2 screenCenter;

    public RectTransform handPointerUI;
    public RectTransform centerPointerUI;

    // new: for smooth move
    private Vector2 lastScreenPos;
    private bool hasLastPos = false;

    // new: smooth speed
    [Range(0.01f, 0.5f)]
    public float smoothSpeed = 0.2f;

    // new: offset
    public float yOffset = 100f;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

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

        if (centerPointerUI != null)
        {
            centerPointerUI.position = screenCenter;
        }
        else
        {
            Debug.LogWarning("Center Pointer UI is not set!");
        }

        if (handPointerUI == null)
        {
            Debug.LogWarning("Hand Pointer UI is not set!");
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

                    // invalid value check
                    if (float.IsInfinity(colorPoint.X) || float.IsInfinity(colorPoint.Y) ||
                        float.IsNaN(colorPoint.X) || float.IsNaN(colorPoint.Y))
                    {
                        if (handPointerUI != null)
                            handPointerUI.gameObject.SetActive(false);

                        hasLastPos = false;
                        continue;
                    }

                    if (handPointerUI != null)
                        handPointerUI.gameObject.SetActive(true);

                    // transform screen position
                    float screenX = Mathf.Clamp(colorPoint.X / 1920f * screenWidth, 0, screenWidth);
                    float screenY = Mathf.Clamp((1 - (colorPoint.Y / 1080f)) * screenHeight + yOffset, 0, screenHeight);
                    Vector2 targetScreenPos = new Vector2(screenX, screenY);

                    // smooth move
                    if (!hasLastPos)
                    {
                        lastScreenPos = targetScreenPos;
                        hasLastPos = true;
                    }

                    Vector2 smoothPos = Vector2.Lerp(lastScreenPos, targetScreenPos, smoothSpeed);

                    Vector2 relativePos = targetScreenPos - screenCenter;

                    handPointerUI.position = smoothPos;

                    // Control player position
                    cursorAdapter.UpdatePosition(relativePos);

                    lastScreenPos = smoothPos;
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
