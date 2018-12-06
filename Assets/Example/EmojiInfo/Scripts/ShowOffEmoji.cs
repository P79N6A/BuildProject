using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class ShowOffEmoji : MonoBehaviour {

    public TextAsset textAsset;
    public Text title;
    public Text scrollContent;
    public Text receipeText;
    public RawImage rawImageToClone;
    public Button sureBtn;
    private Dictionary<string, Rect> emojiRects = new Dictionary<string, Rect>();
    public static ShowOffEmoji _instance;
    private static char emSpace = '\u2001';

    public List<GameObject> imgList = new List<GameObject>();
    void Awake()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
        #endif
        
    }

    // Use this for initialization
    void Start ()
    {
        this.ParseEmojiInfo(this.textAsset.text);
        this.gameObject.SetActive(false);
        //StartCoroutine(this.SetUITextThatHasEmoji(this.bicycleAndUSFlagText, ""));
        //StartCoroutine(this.SetUITextThatHasEmoji(this.footballText, "⚽ ➕ ❤ = I love football"));
        //StartCoroutine(this.SetUITextThatHasEmoji(this.receipeText, "lwn🤓_openIdX"));
        sureBtn.onClick.AddListener(delegate() {
            Debug.Log("sure clicked");
            ScrollRect sr = scrollContent.gameObject.GetComponentInParent<ScrollRect>();
            sr.normalizedPosition = new Vector2(0, 1);
            sr.StopMovement();
            foreach(GameObject emoji in imgList){
                Destroy(emoji);
            }
            this.gameObject.SetActive(false);
            MsdkDemo.isShow = true; });
    }
    public void SetEmoji(string emojiStr) {
        Debug.Log("SetEmoji:" + emojiStr);
        MsdkDemo.isShow = false;
        if (scrollContent == null)
        {
            Debug.Log("scrollContent is null");
        }
        this.StartCoroutine(SetUITextThatHasEmoji(scrollContent, emojiStr));
    }
     
    private static string GetConvertedString(string inputString)
    {
        string[] converted = inputString.Split('-');
        for (int j = 0; j < converted.Length; j++)
        {
            converted[j] = char.ConvertFromUtf32(Convert.ToInt32(converted[j], 16));
        }
        return string.Join(string.Empty, converted);
    }

    private void ParseEmojiInfo(string inputString)
    {
        using (StringReader reader = new StringReader(inputString))
        {
            string line = reader.ReadLine();
            while (line != null && line.Length > 1)
            {
                // We add each emoji to emojiRects
                string[] split = line.Split(' ');
                float x = float.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);
                float y = float.Parse(split[2], System.Globalization.CultureInfo.InvariantCulture);
                float width = float.Parse(split[3], System.Globalization.CultureInfo.InvariantCulture);
                float height = float.Parse(split[4], System.Globalization.CultureInfo.InvariantCulture);
                this.emojiRects[GetConvertedString(split[0])] = new Rect(x, y, width, height);

                line = reader.ReadLine();
            }
        }
    }

    private struct PosStringTuple
    {
        public int pos;
        public string emoji;

        public PosStringTuple(int p, string s)
        {
            this.pos = p;
            this.emoji = s;
        }
    }

    public void SetReceipeTextFromJavascript(string input)
    {
        foreach (Transform child in receipeText.transform)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(this.SetUITextThatHasEmoji(this.receipeText, input));
    }

    public IEnumerator SetUITextThatHasEmoji(Text textToEdit, string inputString)
    {
        List<PosStringTuple> emojiReplacements = new List<PosStringTuple>();
        StringBuilder sb = new StringBuilder();

        int i = 0;
        while (i < inputString.Length)
        {
            string singleChar = inputString.Substring(i, 1);
            string doubleChar = "";
            string fourChar = "";
            
            if (i < (inputString.Length - 1))
            {
                doubleChar = inputString.Substring(i, 2);
            }

            if (i < (inputString.Length - 3))
            {
                fourChar = inputString.Substring(i, 4);
            }

            if (this.emojiRects.ContainsKey(fourChar))
            {
                // Check 64 bit emojis first
                sb.Append(emSpace);
                emojiReplacements.Add(new PosStringTuple(sb.Length - 1, fourChar));
                i += 4;
            }
            else if (this.emojiRects.ContainsKey(doubleChar))
            {
                // Then check 32 bit emojis
                sb.Append(emSpace);
                emojiReplacements.Add(new PosStringTuple(sb.Length - 1, doubleChar));
                i += 2;
            }
            else if (this.emojiRects.ContainsKey(singleChar))
            {
                // Finally check 16 bit emojis
                sb.Append(emSpace);
                emojiReplacements.Add(new PosStringTuple(sb.Length - 1, singleChar));
                i++;
            }
            else
            {
                sb.Append(inputString[i]);
                i++;
            }
        }

        // Set text
        textToEdit.text = sb.ToString();
        yield return null;

        // And spawn RawImages as emojis
        TextGenerator textGen = textToEdit.cachedTextGenerator;
        // One rawimage per emoji

        for (int j = 0; j < emojiReplacements.Count; j++)
        {
            int emojiIndex = emojiReplacements[j].pos;
            GameObject newRawImage = GameObject.Instantiate(this.rawImageToClone.gameObject) as GameObject;
            newRawImage.transform.SetParent(textToEdit.transform);
            Vector3 imagePos = new Vector3(textGen.verts[emojiIndex * 4].position.x, textGen.verts[emojiIndex * 4].position.y, 0);
            newRawImage.transform.localPosition = imagePos;

            RawImage ri = newRawImage.GetComponent<RawImage>();
            ri.uvRect = emojiRects[emojiReplacements[j].emoji];
            imgList.Add(newRawImage);
        }
    }
    
}
