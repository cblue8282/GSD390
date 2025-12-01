using UnityEngine;

[CreateAssetMenu]
public class FighterData : ScriptableObject
{
    public int wins;

    public void ResetData()
    {
        wins = 0;
    }
}