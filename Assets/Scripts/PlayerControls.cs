using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] InputAction movement;
    [SerializeField] InputAction shoot;
    [SerializeField] float xSpeed = 10.0f;
    [SerializeField] float ySpeed = 10.0f;
    [SerializeField] [Min(0.0f)] float xRange = 4.0f;
    [SerializeField] [Min(0.0f)] float yRange = 4.0f;


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

        float xPos = transform.localPosition.x + xOffset;
        float yPos = transform.localPosition.y + yOffset;

        float clampedXPos = Mathf.Clamp(xPos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(yPos, 0, yRange); // y min as 0 since ship starts at bottom of screen

        transform.localPosition = new Vector3(
            clampedXPos, 
            clampedYPos, 
            transform.localPosition.z
        );

    }
}
