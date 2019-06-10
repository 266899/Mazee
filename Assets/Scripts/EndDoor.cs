using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{

    private Animator animator;
    private bool played;
    public AudioSource doorsOpening;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        played = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(TitanBoss.alive))
        {
            animator.Play("EndDoorAnimation");
            if (!doorsOpening.isPlaying && played == false)
            {
                doorsOpening.Play();
                played = true;
            }
        }
    }
    
}
