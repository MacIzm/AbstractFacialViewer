using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.Windows.Speech;
#if NETFX_CORE
using Windows.Storage;

#endif
using System.Text;
#if !UNITY_EDITOR

using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Windows.Data.Json;
using System.Runtime.Serialization.Json;
//using System.Web.Script.Serialization;
#endif
using System.Runtime.Serialization;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;






public class HUD : MonoBehaviour,
                             IInputHandler
{ 
    KeywordRecognizer keywordRecognizer;
    //public Text InfoPanel;
    //public Text AnalysisPanel;
    //public Text ThreatAssessmentPanel;
    //public Text DiagnosticPanel;
    private int irequest = 0;
    public GameObject startBtn;
    public GameObject canvas;
    public string passon;
    public GameObject responsePanel;
    public GameObject parent;
    public static int CorrectCounter = 0;
    public static int IncorrectCounter = 0;
    public static Camera playerCamera;
    //int correctPT;
    //int cPT;
    GameObject plane;

    List<GameObject> listBTN = new List<GameObject>();
    List<GameObject> listBTN2 = new List<GameObject>();

    private Boolean destroyed;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    string BigFolder = "";
    string tempFilePathAndName;
    string pictureFolderPath;
    public static HUD instance { get; private set; }


    /// <summary>
    /// Keep a copy of self.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    public void Start()
    {

        playerCamera = Camera.main;
        BigFolder = Application.persistentDataPath;
        plane = GameObject.Find("Canvas").transform.Find("Plane").gameObject;
        //canvas = GameObject.Find("Canvas");
        /*AnalysisPanel.text = "ANALYSIS:\n**************\ntest\ntest\ntest";
        ThreatAssessmentPanel.text = "SCAN MODE XXXXX\nINITIALIZE";
        InfoPanel.text = "CONNECTING";*/
        /*keywordCollection = new Dictionary<string, KeywordAction>();
        keywordCollection.Add("take", Screenshot2);

        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();*/

        //startBtn.GetComponent<Button>().onClick.AddListener(Screenshot);
        //Debug.Log(destroyed);
        responsePanel = GameObject.Find("ResponsePanel");
#if NETFX_CORE
        getPicturesFolderAsync();
#endif

    }
    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        // no action required.
    }

    public  void resetScene()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Basic");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Basic");
    }
    void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    public void OnInputDown(InputEventData eventData)
    { if (parent.name == "Canvas")
        {
            Screenshot();
        }
        else

            Show();
            }
    public void OnInputUp(InputEventData eventData)
    { }

    public void Show()
    {
        GameObject responePanel = GameObject.Find("ResponsePanel");
        responePanel.transform.Find("happiness").gameObject.SetActive(false);
        responePanel.transform.Find("neutral").gameObject.SetActive(false);
        responePanel.transform.Find("sadness").gameObject.SetActive(false);
        responePanel.transform.Find("anger").gameObject.SetActive(false);
        
        var passon =GameObject.Find("Canvas").GetComponent<HUD>().passon;

        Debug.Log("pass on ---> " + passon);
        if (name == passon)
        {
            GameObject correct = GameObject.Find("Correct");
            Vector3 loc = GameObject.Find("Cursor").transform.localPosition;
            correct.transform.SetPositionAndRotation(loc, playerCamera.transform.localRotation);
            correct.transform.Find("Plane").gameObject.SetActive(true);
            CorrectCounter = CorrectCounter + 1;
           plane.transform.Find("CorrectValue").GetComponent<TextMesh>().text = CorrectCounter.ToString();
        }
        else
        {
            GameObject incorrect = GameObject.Find("Incorrect");
            Vector3 loc = GameObject.Find("Cursor").transform.localPosition;
            incorrect.transform.SetPositionAndRotation(loc, playerCamera.transform.localRotation);
            incorrect.transform.Find("Plane").gameObject.SetActive(true);
            IncorrectCounter = IncorrectCounter + 1;
            plane.transform.Find("IncorrectValue").GetComponent<TextMesh>().text = IncorrectCounter.ToString();
        }

        Invoke("showScorePanel", 3f);

    }





    public void correct()
    {
        GameObject.Find("Canvas").transform.Find("Plane").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(true);
    }

#if NETFX_CORE

private async void getPicturesFolderAsync()
{
    Windows.Storage.StorageLibrary picturesStorage = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
    pictureFolderPath = picturesStorage.SaveFolder.Path;
        
}

#endif


    public void Screenshot()//(PhraseRecognizedEventArgs args)
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        AnalyzeScene();
        canvas.transform.Find("Image").gameObject.SetActive(false);
        canvas.transform.Find("Text").gameObject.SetActive(false);
        Invoke("showDelayed", 3f);

        

    }
    public void Screenshot2(PhraseRecognizedEventArgs args)
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        AnalyzeScene();
        

    }

    void showDelayed()
    {
        btnToShow();
    }

    public void showScorePanel()
    {
        GameObject.Find("Incorrect").transform.Find("Plane").gameObject.SetActive(false);
        GameObject.Find("Correct").transform.Find("Plane").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Plane").gameObject.SetActive(true);
        Invoke("correct", 3f);
    }

    void btnToShow()
    {
        GameObject light = GameObject.Find("Directional Light");
        Vector3 loc = GameObject.Find("Cursor").transform.localPosition;
        light.transform.SetPositionAndRotation(light.transform.position, playerCamera.transform.localRotation);
        light.transform.SetPositionAndRotation(light.transform.position, playerCamera.transform.localRotation);
        responsePanel.transform.SetPositionAndRotation(new Vector3(loc.x,loc.y,loc.z), playerCamera.transform.localRotation);
        responsePanel.transform.Find("happiness").gameObject.SetActive(true);
        responsePanel.transform.Find("neutral").gameObject.SetActive(true);
        responsePanel.transform.Find("sadness").gameObject.SetActive(true);
        responsePanel.transform.Find("anger").gameObject.SetActive(true);


/*        if (IncorrectCounter % 3 == 0)
        {
            var rand = new System.Random().Next(listBTN.Count);
            listBTN2.Add(listBTN.ElementAt(rand));
            listBTN.ElementAt(rand).SetActive(false);

            listBTN.RemoveAt(rand);
        }
        else if (CorrectCounter % 5 == 0)
        {
            listBTN2.ElementAt(0).SetActive(true);
            listBTN.Add(listBTN2.ElementAt(0));
            listBTN2.RemoveAt(0);
        }
        */
    }

    public void hideScorePanel()
    {
        canvas.transform.Find("happiness").gameObject.SetActive(false);
    }

    void AnalyzeScene()
    {
        destroyed = false;
       // InfoPanel.text = "CALCULATION PENDING";
        Debug.Log("In analyze scene");

    }
    
    PhotoCapture _photoCaptureObject = null;
    Texture2D targetTexture = null;
    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        Debug.Log("in on capture created");
       
        _photoCaptureObject = captureObject;
        Debug.Log("1");
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        Debug.Log("2");
        CameraParameters cameraParameters = new CameraParameters();
        cameraParameters.hologramOpacity = 0.0f;
        cameraParameters.cameraResolutionWidth = cameraResolution.width;
        cameraParameters.cameraResolutionHeight = cameraResolution.height;
        cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
        Debug.Log("3");
        _photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
            //_photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            OnPhotoModeStarted(result);
        });
        Debug.Log("5");
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("in on photo mode started");
        if (result.success)
        {

            string file = string.Format(@"Terminate.jpg");
            string filePath = Path.Combine(BigFolder, file);
            Debug.Log("File Path1 " + filePath);
            tempFilePathAndName = filePath;
            Debug.Log("Saving photo to " + filePath);
            try
            {
                _photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("System.ArgumentException:\n" + e.Message);
            }
        }
        else
        {
            //DiagnosticPanel.text = "DIAGNOSTIC\n**************\nUnable to start photo mode.";
            //InfoPanel.text = "ABORT";
        }
    }

    byte[] immy;

    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("in on captured to disk");
        if (result.success)
        {
            byte[] image;

#if NETFX_CORE
            //string file = string.Format(@"Image_{0:yyyy-MM-dd_hh-mm-ss-tt}.jpg", DateTime.Now);
            //filePath = System.IO.Path.Combine(Application.persistentDataPath, file);
            //File.Move(tempFilePathAndName, System.IO.Path.Combine(pictureFolderPath, "Camera Roll", System.IO.Path.GetFileName(tempFilePathAndName)));
            
                //Debug.Log("path: " + tempFilePathAndName);
                image = File.ReadAllBytes(tempFilePathAndName);
                immy = File.ReadAllBytes(tempFilePathAndName);
                //image = GetImageAsByteArray(tempFilePathAndName);
                //Debug.Log("Image Byte - "+image[0]);
                //Debug.Log("Image Byte - " + BitConverter.ToString(immy));
                GetTagsAndFaces(image);
                 Debug.Log("Must be be netfx_core ");
            
            
#else
            Debug.Log("We in unity.");
            image = File.ReadAllBytes(tempFilePathAndName);
            GetTagsAndFaces(image);
#endif


        }
        else
        {
           // DiagnosticPanel.text = "DIAGNOSTIC\n**************\n\nFailed to save Photo to disk.";
           // InfoPanel.text = "ABORT";
        }
        _photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {

        _photoCaptureObject.Dispose();
        _photoCaptureObject = null;
        Debug.Log("Photo Destroyed");
        destroyed = true;


    }

    string _subscriptionKey = "b3e86dbeb3be4c9ca1b7d41726f38cfd";
    string _faceAPIKey = "d53ada67cee9431fae6cb7fcd3878f7f";
    string _computerVisionEndpoint = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Tags,Faces";
    string _faceAPIEndpoint = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=false&returnFaceLandmarks=false&returnFaceAttributes=emotion";//age,gender,headPose,smile,facialHair,glasses,hair,makeup,occlusion,accessories,blur,exposure,noise

    public void GetTagsAndFaces(byte[] image)
    {
        Debug.Log("In Get Tags and Faces");
        //Debug.Log("Image Byte[] - " + BitConverter.ToString(image));

#if NETFX_CORE
   RunCV(image);
        
#else
        try
        {
            StartCoroutine(RunComputerVision(image));
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
            //Screenshot();
        }
        
#endif
    }

#if !UNITY_EDITOR
    private async void RunCV(byte[] image)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _faceAPIKey);
        HttpResponseMessage response;

        using (ByteArrayContent content = new ByteArrayContent(image))
        {
            
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // Execute the REST API call.
            response = await client.PostAsync(_faceAPIEndpoint, content);
    Debug.Log("GOT Response!");
            // Get the JSON response.
            
            var contentString = await response.Content.ReadAsStringAsync();
            var jj = JsonValue.Parse(contentString);
            Debug.Log("Content String - \n" + contentString);

            // Display the JSON response.
            Debug.Log("\nResponse:\n");
            Debug.Log(JsonPrettyPrint(contentString));
            Debug.Log("JJ " + jj.GetArray().GetObjectAt(0).GetNamedObject("faceAttributes").GetNamedObject("emotion"));
            var emotion = jj.GetArray().GetObjectAt(0).GetNamedObject("faceAttributes").GetNamedObject("emotion");
            double answerValue = 0.0;
           string answerText = "";

            foreach (var key in emotion)
            {
                Debug.Log(key.Key + ":"+key.Value.GetNumber());
                Debug.Log("Dummy value: " + answerValue);
                if(key.Value.GetNumber() > answerValue)
                {
                    answerValue = key.Value.GetNumber();
                    answerText = key.Key;
                }
            }
            try {
    Debug.Log("THIS OBJECT 1 --->" + this.name);
    Debug.Log("THIS OBJECT 2 --->" + HUD.instance.name);
    } catch(Exception ex)
    {
    Debug.Log(ex);
    }
            this.passon = answerText;

            Debug.Log("The maxium value is " + answerText + ":" + answerValue);

            //List<string> tags = new List<string>();
            /*var jsonTags = json.GetNamedArray("tags");

            var count = 0;

            for (count = 0; count < jsonTags.Count; count++)
            {
                Debug.Log("Tag - " + jsonTags.GetObjectAt((uint)count).GetNamedString("name"));

                tags.Add(jsonTags.GetObjectAt((uint)count).GetNamedString("name"));
            }
            */

            //AnalysisPanel.text = "ANALYSIS:\n***************\n" + string.Join("\n", tags.ToArray());

            /* List<string> faces = new List<string>();
             var jsonFaces = json.GetNamedArray("faces");
             for (count = 0; count < jsonFaces.Count; count++)
             {
                 faces.Add(string.Format("{0} scanned: age {1}.", jsonFaces.GetObjectAt((uint)count).GetNamedString("gender"), jsonFaces.GetObjectAt((uint)count).GetNamedString("age")));
             }
             if (faces.Count > 0)
             {
                // InfoPanel.text = "MATCH";
             }
             else
             {
                // InfoPanel.text = "ACTIVE SPATIAL MAPPING";
             }

         */
            // ThreatAssessmentPanel.text = "SCAN MODE 43984\nTHREAT ASSESSMENT\n" + string.Join("\n", faces.ToArray());

        }
    }
#endif
    public IEnumerator RunComputerVision(byte[] image)//
    {

        Debug.Log("In Run Computer Vision");
        //Debug.Log("Image Byte - " + BitConverter.ToString(image));
        var headers = new Dictionary<string, string>() {
        { "Ocp-Apim-Subscription-Key", _faceAPIKey },
        { "Content-Type", "application/octet-stream" }//_subscriptionKey
    };

        WWW www = new WWW(_faceAPIEndpoint, image, headers);//_computerVisionEndpoint
        yield return www;

        Debug.Log("My www request " + JsonPrettyPrint(www.text));
        List<string> tags = new List<string>();

        var jsonResults = www.text;
        
        Debug.Log("Json results " + JsonPrettyPrint(www.text));
        //Debug.Log("Type - "+jsonResults.GetType());
        this.passon = "anger";
       /*var js = new JavaScriptSerializer();

        var json = js.Deserialize<string>(www.text);
        Debug.Log("JSON - " + json);
        var myObject = JsonUtility.FromJson<Emotion>(json);
        Debug.Log("My json object " + myObject);

        tags.Add("Anger: " + myObject.anger.ToString("0.0000"));
        tags.Add("Contempt: " + myObject.contempt.ToString("0.0000"));
        tags.Add("Disgust: " + myObject.disgust.ToString("0.0000"));
        tags.Add("Fear: " + myObject.fear.ToString("0.0000"));
        tags.Add("Happiness: " + myObject.happiness.ToString("0.0000"));
        tags.Add("Neutral: " + myObject.neutral.ToString("0.0000"));
        tags.Add("Sadness: " + myObject.sadness.ToString("0.0000"));
        tags.Add("Surprise: " + myObject.surprise.ToString("0.0000"));
        Debug.Log("Tags -" + tags.ToArray().ToString());
         for (int i=0; i< 8;i++)
         {
             tags.Add(myObject.);
             Debug.Log("Tag - " + tag);
         }*/
        //AnalysisPanel.text = "ANALYSIS:\n***************\n" + string.Join("\n", tags.ToArray());

        /* List<string> faces = new List<string>();
         foreach (var face in myObject.faces)
         {
             faces.Add(string.Format("{0} scanned: age {1}.", face.gender, face.age));
         }
         if (faces.Count > 0)
         {
             InfoPanel.text = "MATCH";
         }
         else
         {
             InfoPanel.text = "ACTIVE SPATIAL MAPPING";
         }
         ThreatAssessmentPanel.text = "SCAN MODE 43984\nTHREAT ASSESSMENT\n" + string.Join("\n", faces.ToArray());
    */
    }

    static string JsonPrettyPrint(string json)
    {
        if (string.IsNullOrEmpty(json))
            return string.Empty;

        json = json.Replace(Environment.NewLine, "").Replace("\t", "");

        StringBuilder sb = new StringBuilder();
        bool quote = false;
        bool ignore = false;
        int offset = 0;
        int indentLength = 3;

        foreach (char ch in json)
        {
            switch (ch)
            {
                case '"':
                    if (!ignore) quote = !quote;
                    break;
                case '\'':
                    if (quote) ignore = !ignore;
                    break;
            }

            if (quote)
                sb.Append(ch);
            else
            {
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        sb.Append(Environment.NewLine);
                        sb.Append(new string(' ', ++offset * indentLength));
                        break;
                    case '}':
                    case ']':
                        sb.Append(Environment.NewLine);
                        sb.Append(new string(' ', --offset * indentLength));
                        sb.Append(ch);
                        break;
                    case ',':
                        sb.Append(ch);
                        sb.Append(Environment.NewLine);
                        sb.Append(new string(' ', offset * indentLength));
                        break;
                    case ':':
                        sb.Append(ch);
                        sb.Append(' ');
                        break;
                    default:
                        if (ch != ' ') sb.Append(ch);
                        break;
                }
            }
        }

        return sb.ToString().Trim();
    }


    public class AnalysisResult
    {
        public Tag[] tags;
        public Face[] faces;

    }

    [Serializable]
    public class Tag
    {
        [DataMember]
        public double confidence;

        [DataMember]
        public string name;
    }

    [Serializable]
    public class Emotion
    {

        public float anger;
        public float contempt;
        public float disgust;
        public float fear;
        public float happiness;
        public float neutral;
        public float sadness;
        public float surprise;
    }

    [Serializable]
    public class Face
    {
        public int age;
        public FaceRectangle facerectangle;
        public string gender;
    }

    [Serializable]
    public class FaceRectangle
    {
        public int height;
        public int left;
        public int top;
        public int width;
    }

    [Serializable]
    public class FaceEmote
    {
        public FaceRectangle rect;
        public FaceAttributes attr;
    }

    [Serializable]
    public class FaceAttributes
    {
        public Emotion emote;
    }


}
