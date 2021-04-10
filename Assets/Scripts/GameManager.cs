using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> playerModels = new List<GameObject>();

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public string playerTag = "Player"; // for parent object

    private GameObject _currentPlayerModel;

    void Awake()
    {
        _currentPlayerModel = playerModels[0];
        _currentPlayerModel.SetActive(true);

        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    public void ChangePlayerModel(int playerModelId)
    {
        Debug.Log(playerModelId);
        _currentPlayerModel.SetActive(false);
        _currentPlayerModel = playerModels[playerModelId];
        _currentPlayerModel.SetActive(true);
    }
}