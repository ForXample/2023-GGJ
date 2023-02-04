using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    // Start is called before the first frame update
    public float RotationSpeedXAxis;
    public float RotationSpeedYAxis;

    private Transform CurrentTransform;
    void Start()
    {
        CurrentTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTransform.Rotate(Vector3.up, RotationSpeedYAxis * Time.deltaTime,Space.Self);
        CurrentTransform.Rotate(Vector3.right, RotationSpeedXAxis * Time.deltaTime, Space.Self);
    }
}
