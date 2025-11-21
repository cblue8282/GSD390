using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Slider hpSlider;
    public void SetHUD(Fighter fighter)
    {
        hpSlider.maxValue = fighter.maxHp;
        hpSlider.value = fighter.currHp;
    }

    public void SetHP(int hp) { hpSlider.value = hp; }
}
