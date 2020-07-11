using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    private PlayerInteraction inputActions;
    
    private Vector2 movementInput;

    [SerializeField]
    private float moveSpeed = 10f;
    private Vector3 inputDirection;
    private Vector3 moveVector;
    Vector2 lookPositon;

    private Quaternion currentRotation;
    // Start is called before the first frame update
    private void Awake()
    {
        inputActions = new PlayerInteraction();
        inputActions.Player.Move.performed += context => movementInput = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float h = movementInput.x;
        float v = movementInput.y;

        Vector3 targetInput = new Vector3(h, 0, v);
        inputDirection = Vector3.Lerp(inputDirection, targetInput, Time.deltaTime * 10f);

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 desiredDirection = camForward * inputDirection.z + camRight * inputDirection.x;

        Move(desiredDirection);
        Turn(desiredDirection);
        AnimatePlayer(desiredDirection);
    }

    void Move(Vector3 desiredDirection)
    {
        moveVector.Set(desiredDirection.x, 0f, desiredDirection.z);
        moveVector = moveVector * moveSpeed * Time.deltaTime;
        transform.position += moveVector;
    }

    void Turn(Vector3 desiredDirection)
    {
        if((desiredDirection.x > 0.1 || desiredDirection.x < -0.1) || (desiredDirection.z > 0.1 || desiredDirection.z < -0.1)){
            currentRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = currentRotation;
        }
        else
        {
            transform.rotation = currentRotation;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
       
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void AnimatePlayer(Vector3 desiredDirection)
    {
        if (!anim)
            return;

        Vector3 movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
        float forw = Vector3.Dot(movement, transform.forward);
        anim.SetFloat("Forward", forw);
    }
}
