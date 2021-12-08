using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity.InputModule;

using UnityEngine.EventSystems;

public class ShowAnswer : MonoBehaviour
{
    GameObject canvas;
    //public GameObject btn;
    GameObject answPanel;
    GameObject confirmPanel;
    //public GameObject startBtn;
    string guess;


    HUD capturePhoto;
    KeywordRecognizer keywordRecognizer;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;
    // Use this for initialization
    void Start () {
        /*keywordCollection = new Dictionary<string, KeywordAction>();
        keywordCollection.Add("Snap", Screenshot2);

        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();*/

        capturePhoto = new HUD();
        //capturePhoto.Start();
        canvas = GameObject.Find("Canvas");
        //answPanel = canvas.transform.GetChild(1).gameObject;
        //answ = GameObject.Find("Answer");
        //startBtn = canvas.transform.GetChild(3).gameObject;
        //startBtn.GetComponent<Button>().onClick.AddListener(photo);
        //btn = GameObject.Find("happy");
        //button.GetComponent<Button>();
        //btn.GetComponent<Button>().onClick.AddListener(Show);
        //button.onClick.AddListener();
	}
    void Update()
    {
        // right here
        /*btn = EventSystem.current.currentSelectedGameObject;
        if(btn !=null)
        {
            btn.GetComponent<Button>().onClick.AddListener(Show);
        }*/
        
    }



    public void OnInputDown(InputEventData eventData)
    { Show(); }
    public void OnInputUp(InputEventData eventData)
    { }

    public void Show()
    {
       /* GameObject.Find("ResponsePanel").transform.Find("happiness").gameObject.SetActive(false);
        GameObject.Find("ResponsePanel").transform.Find("neutral").gameObject.SetActive(false);
        //Debug.Log("pass on ---> " + HUD.instance.passon);
        if (HUD.instance != null  && name == HUD.instance.passon)
        {

            GameObject.Find("Correct").transform.Find("Plane").gameObject.SetActive(true);

        }
        else
        {
            GameObject.Find("Incorrect").transform.Find("Plane").gameObject.SetActive(true);
        }

        Invoke("correct", 3f);*/

    }
    public void Screenshot2(PhraseRecognizedEventArgs args)
    {
        capturePhoto.Screenshot2(args);
       // start();
    }
  

 

    public void correct()
    {
        GameObject.Find("Incorrect").transform.Find("Plane").gameObject.SetActive(false);
        GameObject.Find("Correct").transform.Find("Plane").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(true);
    }

    void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

}
