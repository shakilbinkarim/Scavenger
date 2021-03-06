﻿using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardBehavior : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		LoadingCircle.Show();

		GetLeaderboardRequest req = new GetLeaderboardRequest
		{
			MaxResultsCount = 10,
			StatisticName = "Score"
		};

		PlayFabClientAPI.GetLeaderboard(req,
			result => 
			{
				GameObject prefab = Resources.Load<GameObject>("Prefabs/LeaderboardRow");
				GameObject container = GameObject.Find("ItemList");

				foreach(var li in result.Leaderboard)
				{
					GameObject item = Instantiate(prefab);

					item.transform.Find("DisplayName").gameObject.GetComponent<Text>().text = li.DisplayName;
					item.transform.Find("Score").gameObject.GetComponent<Text>().text = li.StatValue.ToString();

					item.transform.SetParent(container.transform, false);
					item.SetActive(true);
				}
				LoadingCircle.Dismiss();
			},
			error =>
			{
				LoadingCircle.Dismiss();
				Debug.Log("Error getting leaderboard: " + error.GenerateErrorReport());
				DialogBox.Show(error.GenerateErrorReport());
			}
		);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Title");
	}
}
