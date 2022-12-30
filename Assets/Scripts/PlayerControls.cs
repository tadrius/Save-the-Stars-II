using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [Header("Keybindings")]
    [Tooltip("Keybindings to move up, down, left, and right.")]
    [SerializeField] InputAction movement;
    [Tooltip("Keybinding to shoot.")]
    [SerializeField] InputAction shoot;
    [Header("Movement Settings")]
    [Tooltip("How quickly the player can move vertically and horizontally.")]
    [SerializeField] float translateSpeed = 7.5f;
    [Tooltip("How much the player rotates based on their local position.")]
    [SerializeField] float positionRotateFactor = 7.5f;
    [Tooltip("How much the player rotates as they move.")]
    [SerializeField] float controlRotateFactor = 15.0f;
    [Tooltip("How far right the player can move.")]
    [SerializeField] float xTranslateMax = 3.75f;
    [Tooltip("How far left the player can move.")]
    [SerializeField] float xTranslateMin = -3.75f;
    [Tooltip("How far up the player can move.")]
    [SerializeField] float yTranslateMax = 2.75f;
    [Tooltip("How far down the player can move.")]
    [SerializeField] float yTranslateMin = -1.75f;
    [Tooltip("How quickly the ship rotates based as the player moves.")]
    [SerializeField] float rotationSpeed = 3.5f;
    [Header("Other Settings")]
    [Tooltip("An array of game objects, each with a particle system component for projectiles.")]
    [SerializeField] Weapon[] weapons;

    float xMove = 0.0f, yMove = 0.0f;
    float normalizedPitch = 0.5f, normalizedRoll = 0.5f;

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
        UpdateMoveFields();
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void UpdateMoveFields() {
        Vector2 moveVal = movement.ReadValue<Vector2>();
        xMove = moveVal.x;
        yMove = moveVal.y;
    }

    private void ProcessRotation() 
    {
        // calculate rotations based on position
        float positionPitch = -transform.localPosition.y * positionRotateFactor;
        float positionYaw = transform.localPosition.x * positionRotateFactor;
        float positionRoll = 0.0f;

        // calculate rotation ranges based on control
        float minControlPitch = -controlRotateFactor;
        float minControlRoll = -controlRotateFactor;
        
        float maxControlPitch = controlRotateFactor;
        float maxControlRoll = controlRotateFactor;

        // calculate pitch and roll normalized
        normalizedPitch = getNormalizedControlRotation(yMove, normalizedPitch);
        normalizedRoll = getNormalizedControlRotation(xMove, normalizedRoll);

        // use normalized rotation values to lerp between the min and max control rotation values
        float controlPitch = Mathf.Lerp(minControlPitch, maxControlPitch, normalizedPitch);
        float controlYaw = 0.0f;
        float controlRoll = Mathf.Lerp(minControlRoll, maxControlRoll, normalizedRoll);

        // apply final rotations
        transform.localRotation = Quaternion.Euler(
            positionPitch - controlPitch, 
            positionYaw - controlYaw, 
            positionRoll - controlRoll);
    }

    private float getNormalizedControlRotation(float control, float curNormalizedRotation) {
        float deltaRotationSpeed = rotationSpeed * Time.deltaTime;
        if (curNormalizedRotation > 0.5f) {
            if (control > 0.0f) {
                curNormalizedRotation = Mathf.Min(curNormalizedRotation + deltaRotationSpeed, 1.0f);
            } else if (control < 0.0f) {
                curNormalizedRotation = curNormalizedRotation - 2.0f * deltaRotationSpeed;
            } else {
                curNormalizedRotation = Mathf.Max(curNormalizedRotation - deltaRotationSpeed, 0.5f);
            }
        } else if (curNormalizedRotation < 0.5f) {
            if (control > 0.0f) {
                curNormalizedRotation = curNormalizedRotation + 2.0f * deltaRotationSpeed;
            } else if (control < 0.0f) {
                curNormalizedRotation = Mathf.Max(curNormalizedRotation - deltaRotationSpeed, 0.0f);
            } else {
                curNormalizedRotation = Mathf.Min(curNormalizedRotation + deltaRotationSpeed, 0.5f);
            }
        } else {
            if (control > 0.0f) {
                curNormalizedRotation = Mathf.Min(curNormalizedRotation + deltaRotationSpeed, 1.0f);
            } else if (control < 0.0f) {
                curNormalizedRotation = Mathf.Max(curNormalizedRotation - deltaRotationSpeed, 0.0f);
            }            
        }
        return curNormalizedRotation;
    }

    private void ProcessTranslation()
    {
        float xOffset = xMove * translateSpeed * Time.deltaTime;
        float yOffset = yMove * translateSpeed * Time.deltaTime;

        float xPos = transform.localPosition.x + xOffset;
        float yPos = transform.localPosition.y + yOffset;

        float clampedXPos = Mathf.Clamp(xPos, xTranslateMin, xTranslateMax);
        float clampedYPos = Mathf.Clamp(yPos, yTranslateMin, yTranslateMax);

        transform.localPosition = new Vector3(
            clampedXPos,
            clampedYPos,
            transform.localPosition.z
        );
    }

    private void ProcessFiring()
    {
        if (shoot.IsPressed()) {
            SetWeaponsActive(true);
        } else {
            SetWeaponsActive(false);
        }
    }

    
    public void SetWeaponsActive(bool isActive) {
        foreach (Weapon weapon in weapons) {
            weapon.SetWeaponsActive(isActive);
        }
    }
}
