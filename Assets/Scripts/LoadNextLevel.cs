using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("MainCamera"))
        {
            Debug.Log("next");
            SceneManager.LoadScene(2);
        }
    }
}
