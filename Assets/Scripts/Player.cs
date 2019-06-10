using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float playerHealth;
    public Canvas youDiedCanvas;
    private bool played;
    public AudioSource playerDied;

    // Use this for initialization
    void Start () {
        youDiedCanvas.enabled = false;
        played = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth > 100)
        {
            playerHealth = 100;
        }

        if (playerHealth <= 0)
        {
            youDiedCanvas.enabled = true;

            if (!playerDied.isPlaying && !played)
            {
                playerDied.Play();
                played = true;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("NextLevelTrigger"))
        {
            Debug.Log("next");
            SceneManager.LoadScene(2);
        }
    }
}
