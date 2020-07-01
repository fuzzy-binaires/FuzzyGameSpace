using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

// Check this out for Spotify get current track REST doc
//https://developer.spotify.com/console/get-users-currently-playing-track/?market=&additional_types=

// Note that for all this work our application requires a Spotify client id
// https://developer.spotify.com/documentation/general/guides/app-settings/


public class SpotifyGetCurrnetTrack : MonoBehaviour
{
  private GameObject playlistCollider;
  public TMP_Text songURLText;
  private RawImage albumArt;
  public TMP_InputField spotifyTokenInput;
  public bool haveAuthorizationToken = true;
  private string spotifyApplicationClientId = "88364b4054954b34bd98c83917307144";

    void Start()
    {
        playlistCollider = GameObject.Find("playlist_trigger");
        spotifyTokenInput  = GameObject.Find("InputSpotifyToken").GetComponent<TMP_InputField>();
        albumArt = GameObject.Find("albumArt").GetComponent<RawImage>();
        songURLText = GameObject.Find("songURLText").GetComponent<TMP_Text>();
        //playlistCollider.GetComponent<ToggleMusicGui>().MusicPlayerCanvasGroup.SetActive(false);

    }

 //https://developer.spotify.com/console/get-users-currently-playing-track/?market=&additional_types=
  UnityWebRequest BuildSpotifyRequest(string url, string method)
  {

      var request = new UnityWebRequest();

      request.url = url;
      request.method = method;
      request.downloadHandler = new DownloadHandlerBuffer();
      //request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString));
      request.SetRequestHeader("Accept", "application/json");
      request.SetRequestHeader("Content-Type", "application/json");
      request.SetRequestHeader("Authorization", "Bearer " + spotifyTokenInput.text);


      return request;
  }

    void Update(){
      var isGuiVisible = playlistCollider.GetComponent<ToggleMusicGui>().isMusicGuiVisible;

      if (/*isGuiVisible &&*/ Input.GetKey(KeyCode.P)) {

        if (false){//(!haveAuthorizationToken){
          string redirectURI="http://fuzzybinaires.org/";

          Debug.Log("Requesting TOKEN");

          string URL = "https://accounts.spotify.com/authorize?client_id="+
          spotifyApplicationClientId +
          "&response_type=code&redirect_uri="+
          redirectURI+
          "&scope=user-read-currently-playing&state=34fFs29kd09";

          Debug.Log(URL);
          //https://accounts.spotify.com/authorize?client_id=88364b4054954b34bd98c83917307144&response_type=code&redirect_uri=http://fuzzybinaires.org/&scope=user-read-currently-playing&state=34fFs29kd09
          //http://fuzzybinaires.org/?code=AQCv-vm2g1nHSojvTsVIWVQ5nDlFbVMlyUNNiHhu1wU9HS0t25cAmTkXBfXCtoROov7fQi0nUAPpGrXQbZZ4LqML720JZK1mKD3_3CxgT9RDpcLBxQITsHqK6kjoFz0fFVJKfzopcpoSdtWMoRinh8l1dz4jQ8KAmKKc3nnSSwP29QkH19AD5bQNNdX5HQuMI5aN-6jfCs_CRw

          //string URL =  "GET https://accounts.spotify.com/authorize?response_type=code&scope=user-read-currently-playing" +
          //"&client_id=" + spotifyApplicationClientId +
          //"&redirect_uri="+redirectURI;

          //https://answers.unity.com/questions/1155907/capture-redirection-url.html
          Application.OpenURL(URL);
          //StartCoroutine(GetRequest(URL));
          haveAuthorizationToken = true;
        } else {
          Debug.Log("Requesting Spotify stuff");



          StartCoroutine(RequestCoroutine(BuildSpotifyRequest("https://api.spotify.com/v1/me/player/currently-playing", UnityWebRequest.kHttpVerbGET)));

        }

      }

    }
    //-------------------------------------------------------------------------
    IEnumerator DownloadAlbumImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            albumArt.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    }

    //-------------------------------------------------------------------------
    IEnumerator RequestCoroutine(UnityWebRequest webRequest){
      yield return  webRequest.SendWebRequest();

      Debug.Log("XXXX " + webRequest.downloadHandler.text + " YYYY");
      if (webRequest.isDone)
      {

        JSONNode jsonData = JSON.Parse(webRequest.downloadHandler.text);
        if (jsonData != null){
          Debug.Log(">>>> " + jsonData["is_playing"]);

          string albumArtUrl = jsonData["item"]["album"]["images"][0]["url"];
          string spotifyLink = jsonData["item"]["album"]["artists"][0]["external_urls"]["spotify"];
          Debug.Log(">>>> " + albumArt);
          Debug.Log("???? " + spotifyLink);


          songURLText.text = string.Format ("<link = \"{0}\"> Open Song...  </link>", spotifyLink);

          StartCoroutine(DownloadAlbumImage(albumArtUrl));

        } else {
          Debug.Log("Could not parse JSON data");
        }
      }


    }
    //-------------------------------------------------------------------------

}
