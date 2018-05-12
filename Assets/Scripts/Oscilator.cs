using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period;
    Vector3 startingPos;
    void Start () {
        startingPos = transform.position;
	}
	
	void Update () {

        if (period <= Mathf.Epsilon) { return; } //No se deben comparar dos floats pq pueden variar minusculamente, en este caso como necesito 0 pues uso epsilon que es lo más bajo a lo qe puedo llegar
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f ;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
