using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public GameObject[] spawner;
    public GameObject[] weapons;
    public Animator anim;
    private int itemCount = 0;
    private bool played;


    private void Start()
    {
        anim = GetComponent<Animator>();
        spawnItem();
        played = false;
    }

    private void OnTriggerEnter(Collider hand)
    {

        if (hand.transform.CompareTag("Hand"))
        {
            Debug.Log("Open");
            anim.Play("Chest Open");

            if (played == false)
            {
                FindObjectOfType<AudioManager>().Play("ChestOpen");
                played = true;
            }
            
        }
    }
    
    private void spawnItem()
    {

        for(int i=0; i<spawner.Length; i++)
        {
            //Get random weapon
            int randomIndex = Random.Range(0, weapons.Length);

            //Spawn sword in chests place

            if(weapons[randomIndex].gameObject.tag == "Weapon")
            {
                //Spawn sword in chest
                Instantiate(weapons[randomIndex], spawner[i].transform.position, Quaternion.Euler(90, 25, 0));
   
                Debug.Log("Sword");
            }
            else
            {
                //Spawn potion in chest
                Instantiate(weapons[randomIndex], spawner[i].transform.position, Quaternion.identity);

                Debug.Log("Potion");
            }
        }
    }
    
   
}
