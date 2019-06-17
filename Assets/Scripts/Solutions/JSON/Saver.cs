using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Saver : MonoBehaviour {

    [System.Serializable]
    struct Base {
        public int MaxCombo, TotalDeaths, TotalTime;
    }

    private string path = @"JSON";
    private GameManager gm;
    private Base session;

    private void Awake() {
        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

        gm = FindObjectOfType<GameManager>();

        InputField input = GetComponent<InputField>();
        input.Select();
        input.ActivateInputField();
        input.onEndEdit.AddListener(salvar);
    }

    public void salvar(string field) {
        Base dados = new Base();

        dados.MaxCombo = gm.MaxCombo;
        dados.TotalDeaths = gm.TotalDeaths;
        dados.TotalTime = gm.TotalTime;;

        string file = string.Format(@"{0}/{1}.json", path, field);

        File.WriteAllText(file, JsonUtility.ToJson(dados));

        SceneManager.LoadScene("Credits", LoadSceneMode.Single);

        try {
            Process myProcess = new Process();
            /*
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            */
            string Exe = "ADS.exe";
            string Jar = "ADS.jar";

            // "https://www.genuinecoder.com/convert-java-jar-to-exe/"
            string JavaWPath = @"C:\Program Files\Java\j2re1.4.2_04\bin\javaw.exe";
            string JarArg = " -jar '{0}'";

            if (File.Exists(Exe)) {
                myProcess.StartInfo.FileName = Exe;
            } else if (File.Exists(Jar)) {
                myProcess.StartInfo.FileName = JavaWPath;
                myProcess.StartInfo.Arguments = string.Format(JarArg, Jar);
            }

            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
            myProcess.WaitForExit();
            int ExitCode = myProcess.ExitCode;
            //print(ExitCode);
        } catch { return; }
    }

}