using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OthersManager : MonoBehaviour {

    #region "Vars"
    public Player player;
    private int Attackers;
    private bool push;
    [SerializeField] private int MaxAttackers = 2;
    private int maxAttackers;
    [SerializeField] private List<Enemy> EnemiesNearby;
    #endregion

    private void FixedUpdate() {
        Catch();
        Calculate();
    }

    private void Catch() {
        EnemiesNearby.Clear();

        foreach (Transform child in transform) {
            if (child.GetComponent<CharacterBase>().IsDead || !child.gameObject.activeSelf) { continue; }

            EnemiesNearby.Add(child.GetComponent<Enemy>());
        }
    }

    private void Calculate() {
        EnemiesNearby = InsertionSort(EnemiesNearby);

        Attackers = 0;

        maxAttackers = player.IsDead ? 0 : MaxAttackers;

        foreach (Enemy raged in EnemiesNearby) {

            raged.Attacking = (Attackers <= maxAttackers - 1);

            Attackers++;

            if (push) { raged.isFalling = true; raged.Push(); }
        }
        push = false;
    }

    public void PushAll() => push = true;

    #region "Sort"
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
    #endregion
}