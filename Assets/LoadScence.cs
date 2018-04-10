using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class LoadScence : MonoBehaviour {

    public Camera camera;
    public GameObject input;

  

    

    public void train()
    {
        
        Debug.Log("Loading Train phase");
        //TurnOffAL();
        SceneManager.LoadSceneAsync("Train", LoadSceneMode.Single);
    }

    public void test()
    {
      

        Debug.Log("Loading Test phase");
        //TurnOffAL();
        SceneManager.LoadSceneAsync("Basic",LoadSceneMode.Single);
    }
	
    void TurnOffAL()
    {
        //turn off AudioListener component if one already exists in the scene
        //WARNING! This finds disabled listener components!! (although not inactive GOs with listeners on them)
        AudioListener[] myListeners = FindObjectsOfType(typeof(AudioListener)) as AudioListener[];

        int totalListeners = 0;//find out how many listeners are actually active
        foreach (AudioListener thisListener in myListeners)
        {
            if (thisListener.enabled) { totalListeners++; }
        }

        if (totalListeners > 1)
        {
            //turn off my audioListener component
            AudioListener al = GetComponent<AudioListener>();
            al.enabled = false;
        }
        else
        {
            //turn on my audioListener component
            AudioListener al = GetComponent<AudioListener>();
            al.enabled = true;
            //print ("turn on audio "+name);
        }
    }
}
