using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;    // ����϶����ƶ��ٶ�
    public float zoomSpeed = 2f;    // �����ֵ������ٶ�
    public float minZoom = 5f;      // ��С����ֵ
    public float maxZoom = 20f;     // �������ֵ

    public Slider zoomSlider;       // ������ʾ�Ϳ������ŵĻ���

    private Vector3 dragOrigin;

    void Start()
    {
        // ��ʼ������Slider
        zoomSlider.minValue = minZoom;
        zoomSlider.maxValue = maxZoom;
        zoomSlider.value = Camera.main.orthographicSize;

        // ����Sliderֵ�仯
        zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChanged);
    }

    void Update()
    {
        // ��������϶�
        HandleMouseDrag();

        // ��������
        HandleZoom();
    }

    void HandleMouseDrag()
    {
        // ��������UIԪ���ϣ��򲻴�������϶�
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * moveSpeed, pos.y * moveSpeed, 0);

        transform.Translate(-move, Space.World);

        dragOrigin = Input.mousePosition;
    }

    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scrollData * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

        // ����Sliderֵ
        zoomSlider.value = Camera.main.orthographicSize;
    }

    void OnZoomSliderValueChanged(float value)
    {
        Camera.main.orthographicSize = value;
    }
}
