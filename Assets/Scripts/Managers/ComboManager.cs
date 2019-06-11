using UnityEngine;

public class ComboManager : MonoBehaviour {

    private int maxCombo;
    private int totalCombo;
    public int MaxCombo => maxCombo;
    public int TotalCombo => totalCombo;

    public void Calc(int newTry) {
        totalCombo++;
        if (newTry > MaxCombo) { maxCombo = newTry; }
    }

}