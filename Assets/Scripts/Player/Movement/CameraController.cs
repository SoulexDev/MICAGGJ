using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private float m_CamX, m_CamY;

    [SerializeField] private Transform m_CameraTarget;
    [SerializeField] private Transform m_CameraRotationHandle;
    [SerializeField] private Transform m_CameraAnimationHandle;

    [SerializeField] private AnimationCurve m_StepXRotation;
    [SerializeField] private AnimationCurve m_StepZRotation;
    [SerializeField] private AnimationCurve m_StepYPosition;

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

        PlayerController controller = Player.Instance.playerController;

        Vector3 charVel = controller.moveVector;
        charVel.y = 0;
        float amount = charVel.magnitude;
        float time = Time.time * amount * 0.5f;
        float loop = time - Mathf.Floor(time);

        Quaternion rot = Quaternion.Euler(m_StepXRotation.Evaluate(loop), 0, 
            m_StepZRotation.Evaluate(loop));

        Vector3 pos = Vector3.down * m_StepYPosition.Evaluate(loop);

        m_CameraAnimationHandle.localRotation = Quaternion.Lerp(Quaternion.identity, rot, amount);
        m_CameraAnimationHandle.localPosition = Vector3.Lerp(Vector3.zero, pos, amount);
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