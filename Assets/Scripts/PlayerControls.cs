using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    private static string Horizontal = "Horizontal";
    private static string Vertical = "Vertical";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxis(Horizontal);
        float verticalAxis = Input.GetAxis(Vertical);
        Debug.Log("Horizontal Axis: " + horizontalAxis);
        Debug.Log("Vertical Axis: " + verticalAxis);
    }
}
