using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class BackToMain : MonoBehaviour {
    KeywordRecognizer keywordRecognizer;
    GameObject bug;
    
    // Defines which function to call when a keyword is recognized.
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;
    // Use this for initialization
    void Start () {
        bug = GameObject.Find("/Canvas/Debug");
        bug.SetActive(false);
        keywordCollection = new Dictionary<string, KeywordAction>();

        // Add keyword to start manipulation.
        keywordCollection.Add("Main Menu", Back);
        keywordCollection.Add("Bug", Debug);


        // Initialize KeywordRecognizer with the previously added keywords.
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void Back(PhraseRecognizedEventArgs args)
    {
        SceneManager.LoadSceneAsync("MainMenu2", LoadSceneMode.Single);
    }

    private void Debug(PhraseRecognizedEventArgs args)
    {
        if(bug.activeSelf == false)
        {
            bug.SetActive(true);
        }
        else
            bug.SetActive(false);

    }
}
