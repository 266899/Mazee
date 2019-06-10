using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Player playerScript;
    private Image healthBar;
    private float currentHealth;
    public Potion potion;
    public TMPro.TextMeshProUGUI potionCounter;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = playerScript.playerHealth / 100f;
        potionCounter.text = potion.GetPotionCount().ToString();
    }
}
