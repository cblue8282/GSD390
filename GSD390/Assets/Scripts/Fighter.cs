using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int maxHp = 30;
    public int currHp;

    void Awake()
    {
        // Ensure every new instance starts full
        currHp = maxHp;
    }

    // Returns true if fighter dies
    public bool TakeDamage(int damage)
    {
        currHp -= damage;
        if (currHp <= 0)
        {
            currHp = 0;
            return true;
        }
        return false;
    }

    public void Heal(int amount)
    {
        currHp += amount;
        if (currHp > maxHp) currHp = maxHp;
    }
}
