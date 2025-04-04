using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200001C RID: 28
public class FirebaseManager : MonoBehaviour
{
	// Token: 0x06000081 RID: 129 RVA: 0x00005024 File Offset: 0x00003224
	private IEnumerator FetchActiveVote()
	{
		string uri = "https://your-backend-url.com/active_vote";
		using (UnityWebRequest request = UnityWebRequest.Get(uri))
		{
			yield return request.SendWebRequest();
			if (request.result == UnityWebRequest.Result.Success)
			{
				JObject jobject = JObject.Parse(request.downloadHandler.text);
				Debug.Log("Active Vote: " + jobject.ToString());
			}
			else
			{
				Debug.LogError("Failed to fetch vote: " + request.downloadHandler.text);
			}
		}
		UnityWebRequest request = null;
		yield break;
		yield break;
	}
}
