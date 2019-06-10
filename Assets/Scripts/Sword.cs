using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public Knight knight;
    public Spider spider;
    public TitanBoss titan;


    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag.Equals("Knight")) {
            knight.health -= 10f;
            Debug.Log("sword damage");
        }
    }
}
