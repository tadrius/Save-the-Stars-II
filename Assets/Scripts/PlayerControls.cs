using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] InputAction movement;
    [SerializeField] InputAction shoot;
    [SerializeField] GameObject[] weapons;
    [SerializeField] float translateSpeed = 7.5f;
    [SerializeField] float positionRotateFactor = 7.5f;
    [SerializeField] float controlRotateFactor = 15.0f;
    [SerializeField] float xTranslateMax = 3.75f;
    [SerializeField] float xTranslateMin = -3.75f;
    [SerializeField] float yTranslateMax = 2.25f;
    [SerializeField] float yTranslateMin = -2.25f;
    [SerializeField] float smoothingSpeed = 3.5f;

    float xMove = 0.0f, yMove = 0.0f;
    float pitchSmoothing = 0.5f, rollSmoothing = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        DeactivateWeapons();
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

        // calculate rotations based on control
        float minControlPitch = -controlRotateFactor;
        float minControlRoll = -controlRotateFactor;
        
        float maxControlPitch = controlRotateFactor;
        float maxControlRoll = controlRotateFactor;

        pitchSmoothing = processSmoothing(yMove, pitchSmoothing);
        rollSmoothing = processSmoothing(xMove, rollSmoothing);

        float controlPitch = Mathf.Lerp(minControlPitch, maxControlPitch, pitchSmoothing);
        float controlYaw = 0.0f;
        float controlRoll = Mathf.Lerp(minControlRoll, maxControlRoll, rollSmoothing);

        // apply rotation
        transform.localRotation = Quaternion.Euler(
            positionPitch - controlPitch, 
            positionYaw - controlYaw, 
            positionRoll - controlRoll);
    }

    private float processSmoothing(float control, float curSmoothing) {
        float deltaSmoothingSpeed = smoothingSpeed * Time.deltaTime;
        if (curSmoothing > 0.5f) {
            if (control > 0.0f) {
                curSmoothing = Mathf.Min(curSmoothing + deltaSmoothingSpeed, 1.0f);
            } else if (control < 0.0f) {
                curSmoothing = curSmoothing - 2.0f * deltaSmoothingSpeed;
            } else {
                curSmoothing = Mathf.Max(curSmoothing - deltaSmoothingSpeed, 0.5f);
            }
        } else if (curSmoothing < 0.5f) {
            if (control > 0.0f) {
                curSmoothing = curSmoothing + 2.0f * deltaSmoothingSpeed;
            } else if (control < 0.0f) {
                curSmoothing = Mathf.Max(curSmoothing - deltaSmoothingSpeed, 0.0f);
            } else {
                curSmoothing = Mathf.Min(curSmoothing + deltaSmoothingSpeed, 0.5f);
            }
        } else {
            if (control > 0.0f) {
                curSmoothing = Mathf.Min(curSmoothing + deltaSmoothingSpeed, 1.0f);
            } else if (control < 0.0f) {
                curSmoothing = Mathf.Max(curSmoothing - deltaSmoothingSpeed, 0.0f);
            }            
        }
        return curSmoothing;
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
            ActivateWeapons();
        } else {
            DeactivateWeapons();
        }
    }

    
    private void ActivateWeapons() {
        foreach (GameObject weapon in weapons) {
            weapon.SetActive(true);
        }
    }

    private void DeactivateWeapons() {
        foreach (GameObject weapon in weapons) {
            weapon.SetActive(false);
        }
    }
}
