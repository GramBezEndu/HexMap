using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.05f;

    private void LateUpdate()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
        {
            movement += new Vector2(speed, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement -= new Vector2(speed, 0f);
        }

        if (Input.GetKey(KeyCode.W))
        {
            movement += new Vector2(0f, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= new Vector2(0f, speed);
        }

        transform.position += (Vector3)movement;
    }
}
