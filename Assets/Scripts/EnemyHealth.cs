using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100.0f;
    public GameObject explodingParts;
    public GameObject enemyModel;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
            currentHealth -= 10.0f;
            Debug.Log("damaged");
            if(currentHealth == 0f)
            {
                Debug.Log("dead");
                explodingParts.SetActive(true);
            enemyModel.SetActive(false);
                Destroy(gameObject, 3.0f);
            }
        }
    }
