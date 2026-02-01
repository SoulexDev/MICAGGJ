using UnityEngine;

public enum PlayerState { Idle, Walk, Sprint }
public class PlayerController : StateMachine<PlayerController>
{
    public CharacterController characterController;
    public Character characterData;

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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        var body = hit.collider.attachedRigidbody;
        // no rigidbody
        if (body == null || body.isKinematic)
            return;
        // Only push rigidbodies in the right layers
        //var bodyLayerMask = 1 << body.gameObject.layer;
        //if ((bodyLayerMask & pushLayers) == 0)
        //return;

        //Debug.Log(hit.moveDirection);
        //Debug.Log(hit.point);

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
            return;
        // Calculate push direction from move direction, we only push objects to the sides
        // never up and down
        var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        Debug.Log("adding force");

        // push with move speed but never more than walkspeed
        body.AddForceAtPosition(pushDir * 0.15f, hit.point, ForceMode.Impulse);
    }
}