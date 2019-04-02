using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleRayCaster : MonoBehaviour
{
    public Transform VisualizerTransform;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Ray sent");
            if (Physics.Raycast(inputRay, out hit))
            {
                VisualizerTransform.position = hit.point;
                Debug.Log("Ray received");
            }
        }
    }

}
