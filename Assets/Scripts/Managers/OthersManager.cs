using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OthersManager : MonoBehaviour {
    [SerializeField] private List<Enemy> Enemies;
    private int Attackers;

    private void FixedUpdate() {
        Catch();
    }

    public void Catch() {
        Enemies.Clear();

        foreach (Transform child in transform) {
            if (child.gameObject.GetComponent<CharacterBase>().IsDead || !child.gameObject.activeSelf) { continue; }

            Enemies.Add(child.GetComponent<Enemy>());
            Do();
        }
    }

    private void Do() {
        Enemies = InsertionSort(Enemies);

        Attackers = 0;

        foreach (Enemy raged in Enemies) {

            raged.Attacking = (Attackers <= 1);
            Attackers++;
        }
    }

    private static List<Enemy> InsertionSort(List<Enemy> input) {
        for (int i = 0; i < input.Count - 1; i++) {
            for (int j = i + 1; j > 0; j--) {
                if (input[j - 1].PlayerDistance.sqrMagnitude <= input[j].PlayerDistance.sqrMagnitude) { continue; }

                Enemy temp = input[j - 1];
                input[j - 1] = input[j];
                input[j] = temp;
            }
        }
        return input;
    }
}