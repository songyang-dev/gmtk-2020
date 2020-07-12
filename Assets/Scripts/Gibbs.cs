using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibbs : MonoBehaviour
{
    public float minForce = 10f, maxForce = 20f;

    Rigidbody[] rbs;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;

        rbs = GetComponentsInChildren<Rigidbody>();

        foreach (var rb in rbs)
        {
            AddRandomForce(rb);
        }

        Destroy(gameObject, 4.0f);
    }

    void AddRandomForce(Rigidbody rb)
    {
        float randomForward = Random.Range(-1f, 1f);
        float randomSide = Random.Range(-1f, 1f);
        float randomUp = Random.Range(1f, 2f);

        Vector3 forceVector = new Vector3(randomSide, randomUp, randomForward).normalized;

        forceVector *= Random.Range(minForce, maxForce);

        rb.AddForce(forceVector, ForceMode.VelocityChange);
    }
   
}
