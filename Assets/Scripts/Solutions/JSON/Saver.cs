using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Saver : MonoBehaviour {

    [System.Serializable]
    struct Base {
        public float comboMax, comboTotal, timeElapsed;
        public string playerName;
    }

    private GameManager gm;
    private Base session;

    private void Awake() {
        gm = FindObjectOfType<GameManager>();

        InputField input = GetComponent<InputField>();
        input.Select();
        input.ActivateInputField();
        input.onEndEdit.AddListener(salvar);
    }

    public void salvar(string field) {

        Base dados = new Base();
        List<Base> l = new List<Base>();
        

        dados.playerName = field;
        dados.comboMax = gm.MaxCombo;
        dados.comboTotal = gm.TotalCombo;
        dados.timeElapsed = gm.TotalTime;

        File.WriteAllText("dados.json", JsonUtility.ToJson(dados));
    }

}