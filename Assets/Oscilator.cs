using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    [SerializeField]Vector3 movementVector= new Vector3(15,0,0);    
    [SerializeField] float period = 2f;

    float movementRatio;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //mathf is the smallest float
        //do not compare with zero as float can differ verry small
        if (period <= Mathf.Epsilon) { return; }
        
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementRatio = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementRatio;
        transform.position = startPosition + offset;

    }
}
