using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    [SerializeField]Vector3 movementVector;
    
    [SerializeField][Range(0,1)] float movementRatio;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = movementVector * movementRatio;
        transform.position = startPosition + offset;

    }
}
