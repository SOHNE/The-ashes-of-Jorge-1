using UnityEngine;

public class ComboManager : MonoBehaviour {

    private int maxCombo;
    private int totalDeaths;
    public int MaxCombo => maxCombo;
    public int TotalDeaths => totalDeaths;

    public void CalcCombo(int newTry) {
        if (newTry > MaxCombo) { maxCombo = newTry; }
    }

    public void CalcDeaths(){
        totalDeaths++;
    }

}