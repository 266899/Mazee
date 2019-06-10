
using UnityEngine;

public class Potion : MonoBehaviour
{

    private static float potionCount;

    private void Start()
    {
        potionCount = 0;
    }

    public void RemovePotion()
    {
        potionCount -=1;
        if (potionCount < 0)
        {
            potionCount = 0;
        }
    }

    public void AddPotion()
    {
        potionCount += 1;
    }

    public float GetPotionCount()
    {
        return potionCount;
    }
}
