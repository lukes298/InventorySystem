using UnityEngine;
using System.Collections;

public class ModelScript : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float mouseSensitivity = 0.4f;
    public float rotationDampener = 1f;
    private Vector3 mouseRef;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isRotating;
	
    void OnMouseDown()
    {
        // start rotating when mouse is down
        isRotating = true;
        //Store the mouse pos in reference
        mouseRef = Input.mousePosition;
    }

    void OnMouseUp()
    {
        isRotating = false;
    }
	
	void Update ()
    {   
        if (isRotating)
        {
            mouseOffset = Input.mousePosition - mouseRef;
            rotation.y = (mouseOffset.x + mouseOffset.y) * -mouseSensitivity;
            transform.Rotate(rotation);
            mouseRef = Input.mousePosition;
        }
        else
        {
            float sign = rotation.y < 0 ? - 1 : 1;

            transform.rotation *= Quaternion.AngleAxis(rotationSpeed * rotation.y, Vector3.up);

            rotation.y -= rotationDampener * sign * Time.deltaTime;
            rotation.y = Mathf.Abs(rotation.y) <= 1 ? sign : rotation.y;

            
        }

	}
}
