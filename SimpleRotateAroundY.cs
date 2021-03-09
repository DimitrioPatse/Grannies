using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotateAroundY : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed / 10);
    }
}
