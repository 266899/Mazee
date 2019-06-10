using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{

    public List<GameObject> enemies;
    public TMPro.TextMeshProUGUI counter;

    // Update is called once per frame
    void Update()
    {
        counter.text = enemies.Count.ToString();
        CheckAliveEnemies();
    }

    void CheckAliveEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!(enemies[i].activeInHierarchy))
            {
                enemies.Remove(enemies[i]);
            }
        }
    }
}
