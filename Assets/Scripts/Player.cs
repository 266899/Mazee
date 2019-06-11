using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float playerHealth;
    public Canvas youDiedCanvas;
    private bool played;
    public AudioSource playerDied;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        youDiedCanvas.enabled = false;
        played = false;
        rb = GetComponent<Rigidbody>();
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
                StartCoroutine(LoadNextLevelAfterDelay());
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("NextLevelTrigger"))
        {
            SceneManager.LoadScene(2);
        }

    }

    IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }
}
