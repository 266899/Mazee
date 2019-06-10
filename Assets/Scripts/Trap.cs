using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    public float damage;
    public Player player;

    public void OnTriggerEnter(Collider body)
    {
        if (body.transform.CompareTag("MainCamera"))
        {
            player.playerHealth = -damage;
            Debug.Log("swx");
        }

        }


    }
