using UnityEngine;

public class Fighter : MonoBehaviour
{
    public string Name;
    public int Damage;
    public int maxHp;
    public int currHp;

    public bool TakeDamage(int dmg)
    {
        currHp -= dmg;
        if (currHp <= 0) return true; else return false;
    }

    public void Heal(int amount)
    {
        currHp += amount;
        if (currHp > maxHp) currHp = maxHp;
    }
}
