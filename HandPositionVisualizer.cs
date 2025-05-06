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
        // �ù��ѪR��
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

        // ��l��Kinect
        sensor = KinectSensor.GetDefault();

        if (sensor != null)
        {
            bodyReader = sensor.BodyFrameSource.OpenReader();
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

                    // �ন Unity �@�ɮy��
                    float scaleFactor = 25.0f; // �դj�@�I�]3 ~ 5 �ۤv�լݬݳ̵ΪA���ȡ^

                    // ��l��m���W���v
                    Vector3 unityPos = new Vector3(position.X * scaleFactor, position.Y * scaleFactor, position.Z);

                    // �ഫ�ù��y��
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(unityPos);

                    // �ˬd Z �b�AZ < 0 �ɤ����
                    if (screenPos.z < 0)
                    {
                        Debug.Log("����b��v�����A�����");
                        if (handPointerUI != null)
                            handPointerUI.gameObject.SetActive(false);
                        continue;
                    }
                    else
                    {
                        if (handPointerUI != null)
                            handPointerUI.gameObject.SetActive(true);
                    }

                    // Clamp ����W�X�ù�
                    screenPos.x = Mathf.Clamp(screenPos.x, 0, screenWidth);
                    screenPos.y = Mathf.Clamp(screenPos.y, 0, screenHeight);

                    // ��s UI ��m
                    if (handPointerUI != null)
                    {
                        handPointerUI.position = screenPos;
                    }

                    // Debug Console ��X
                    Debug.Log($"����ù��y�Ц�m: X = {screenPos.x}, Y = {screenPos.y}");
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
