using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] InputAction movement;
    [SerializeField] InputAction shoot;
    [SerializeField] float xSpeed = 1.0f;
    [SerializeField] float ySpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable() {
        movement.Enable();
        shoot.Enable();
    }

    void OnDisable() {
        movement.Disable();
        shoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVal = movement.ReadValue<Vector2>();
        float xMove = moveVal.x;
        float yMove = moveVal.y;

        float xOffset = xMove * xSpeed * Time.deltaTime;
        float yOffset = yMove * ySpeed * Time.deltaTime;

        transform.localPosition = new Vector3(
            transform.localPosition.x + xOffset, 
            transform.localPosition.y + yOffset, 
            transform.localPosition.z
        );

    }
}
