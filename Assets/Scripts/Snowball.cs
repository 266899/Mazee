using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public Animator anim;

    public void OnTriggerEnter(Collider other)
    {
        anim.Play("Snowball");
    }
    
    
}
