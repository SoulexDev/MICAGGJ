using UnityEngine;

public enum PlayerState { Idle, Walk, Sprint }
public class PlayerController : StateMachine<PlayerController>
{
    public CharacterController characterController;
    public float walkSpeed = 2.5f;
    public float runSpeed = 6;

    public Vector3 playerCenter => transform.position + Vector3.up * characterController.height;

    public Vector3 groundNormal;

    public Vector2 inputVector;
    public Vector3 moveVector;

    public bool isMoving;
    public bool isUsingInput;

    private void Awake()
    {
        stateDictionary.Add(PlayerState.Idle, new PlayerIdle());
        stateDictionary.Add(PlayerState.Walk, new PlayerWalk());
        stateDictionary.Add(PlayerState.Sprint, new PlayerSprint());

        SwitchState(PlayerState.Idle);
    }
    public override void Update()
    {
        GroundCheck();
            
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");

        moveVector = Vector3.Cross(CameraController.Instance.GetRight(), groundNormal) * inputVector.y - 
            Vector3.Cross(CameraController.Instance.GetForward(), groundNormal) * inputVector.x;

        moveVector = Vector3.ClampMagnitude(moveVector, 1);

        isUsingInput = inputVector.x != 0 || inputVector.y != 0;
        isMoving = Mathf.Abs(Mathf.Max(characterController.velocity.x, characterController.velocity.z)) > 0.1f;

        base.Update();

        characterController.Move(Vector3.down * 15 * Time.deltaTime);
    }
    public void GroundCheck()
    {
        if (Physics.Raycast(playerCenter, Vector3.down, out RaycastHit hit, 4, GameManager.playerRayIgnoreMask))
            groundNormal = hit.normal;
        else
            groundNormal = Vector3.up;
    }
}