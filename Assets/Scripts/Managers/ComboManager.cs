using UnityEngine;

public class ComboManager : MonoBehaviour {
    
    public int Attacks = 0;
    public int TotalValidCombos = 0;
    public int TotalCombos = 0;
    
    public int calc => TotalCombos / TotalValidCombos;

    private void FixedUpdate() {
        
    }

}