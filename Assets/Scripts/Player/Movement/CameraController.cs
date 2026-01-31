using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private float m_CamX, m_CamY;

    [SerializeField] private Transform m_CameraTarget;
    [SerializeField] private Transform m_CameraRotationHandle;

    private void Awake()
    {
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        m_CamX -= Input.GetAxisRaw("Mouse Y");
        m_CamY += Input.GetAxisRaw("Mouse X");

        m_CamX = Mathf.Clamp(m_CamX, -90, 90);

        m_CameraRotationHandle.rotation = Quaternion.Euler(m_CamX, m_CamY, 0);
        m_CameraRotationHandle.position = m_CameraTarget.position;
    }
    public Vector3 GetForward()
    {
        return Quaternion.AngleAxis(m_CamY, Vector3.up) * Vector3.forward;
    }
    public Vector3 GetRight()
    {
        return Quaternion.AngleAxis(m_CamY, Vector3.up) * Vector3.right;
    }
}