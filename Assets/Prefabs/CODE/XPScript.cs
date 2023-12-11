using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPScript : MonoBehaviour
{
    public Slider slider;
    public int lvlOfPlayer;
    public int expPoints;

    public void GetXP()
    {
        expPoints = GetComponent<PlayerXP>().GetXP();
        lvlOfPlayer = GetComponent<PlayerXP>().level;
    }

    public void SetMaxXP(int expPoints)
    {
        slider.maxValue = expPoints;
    }

    public void SetXP(int expPoints)
    {
        slider.value = expPoints;
    }

    public void SetLevel(int level)
    {
        lvlOfPlayer = level;
    }

    public int getLevel()
    {
        return lvlOfPlayer;
    }
}
