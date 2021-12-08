using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Browse : MonoBehaviour {

    public GameObject emotions;
    static int inc = 0;

     void Start()
    {
        emotions.transform.GetChild(inc).gameObject.SetActive(true);
    }

    public void back()
    {
        Debug.Log("back");
        Debug.Log("inc:"+inc);

        if (inc == 0)
        {
            emotions.transform.GetChild(inc).gameObject.SetActive(false);
            inc = 4;
            emotions.transform.GetChild(3).gameObject.SetActive(true);
            return;
        }

        inc--;
        emotions.transform.GetChild(inc).gameObject.SetActive(true);
        var temp = inc + 1;
        emotions.transform.GetChild(temp).gameObject.SetActive(false);

    }

    public void next()
    {
        Debug.Log("next");
       
        Debug.Log("inc:" + inc);
        if (inc == 4)
        {
            emotions.transform.GetChild(inc).gameObject.SetActive(false);
            inc = 0;
            emotions.transform.GetChild(0).gameObject.SetActive(true);
            return;
        }
        inc++;
        emotions.transform.GetChild(inc).gameObject.SetActive(true);
        var temp = inc - 1;
        emotions.transform.GetChild(temp).gameObject.SetActive(false);

    }
}
