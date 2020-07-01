using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LCD_ScreenController : MonoBehaviour {

 public TMP_Text LCDText;
 public float scrollSpeed = 1.0f;
 private string originalText;

 private TMP_Text cloneText;
 private RectTransform textRectTransform;
 private string sourceText;
 private string tempText;



 void Start()
 {
   Debug.Log("In this shit");
   LCDText = GameObject.Find("LCD_Text_TMP").GetComponent<TMP_Text>();
   originalText = LCDText.text;
   textRectTransform = LCDText.GetComponent<RectTransform>();

   //LCDText.text = "Fuuuu";
   // cloneText = Instantiate(LCDText) as TMP_Text;
   //
   // RectTransform cloneRectTransform = cloneText.GetComponent<RectTransform>();
   // cloneRectTransform.SetParent(textRectTransform);
   // cloneRectTransform.anchorMin = new Vector2(1, 0.5f);
   // cloneRectTransform.localPosition = new Vector3(LCDText.preferredWidth, 0, cloneRectTransform.position.z);
   // cloneRectTransform.localScale = new Vector3(1, 1, 1);
   // cloneText.text = LCDText.text;


    //StartCoroutine("Scroll");
    InvokeRepeating("Scroll", 2.0f, 0.3f);
   Debug.Log("In this Done");




 }

 void Scroll()
 {

   float width = LCDText.preferredWidth;
 //  Debug.Log("FFFFFFFF");
 //  Debug.Log(string.Format ("Scrolling {0}", width));
   Vector3 startPosition = textRectTransform.localPosition;

   float scrollPosition = 0;
   //string text = LCDText.text.Substring(1);
   string text = LCDText.text;



   LCDText.text = text.Substring(1,text.Length-1) + text.Substring(0, 1);;



   // while (true)
   // {
   //  //textRectTransform.localPosition = new Vector3(-scrollPosition % width, startPosition.y, startPosition.z);
   //  String text =  LCDText.text;
   //  scrollPosition += scrollSpeed * 20 * Time.deltaTime;
   //  if (Time.deltaTime )
   //  //Debug.Log(string.Format ("{0}scrollPosition: {}", scrollPosition));
   //  yield return null;
   // }
 }
}
