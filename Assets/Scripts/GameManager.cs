using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> playerModels = new List<GameObject>();

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public string playerTag = "Player"; // for parent object

    private GameObject _currentPlayerModel;

    private Button _respawnButton;
    private GameObject _deathScreenGameObject;

    public override void Initialize()
    {
        _currentPlayerModel = playerModels[0];
        _currentPlayerModel.SetActive(true);

        player = GameObject.FindGameObjectWithTag(playerTag);

        _deathScreenGameObject = GameObject.FindGameObjectWithTag("DeathScreen");
        _respawnButton = GameObject.FindGameObjectWithTag("RespawnButton").GetComponent<Button>();
        _respawnButton.onClick.AddListener(OnRespawnClicked);
        _deathScreenGameObject.SetActive(false);
    }

    public void ChangePlayerModel(int playerModelId)
    {
        _currentPlayerModel.SetActive(false);
        _currentPlayerModel = playerModels[playerModelId];
        _currentPlayerModel.SetActive(true);
    }

    public void GameOver()
    {
        _deathScreenGameObject.SetActive(true);
        player.SetActive(false);
    }

    public void Respawn()
    {
        _deathScreenGameObject.SetActive(false);
        player.SetActive(true);

        player.GetComponent<HealthComponent>().Fill();
        player.GetComponent<MovementComponent>().TeleportTo(new Vector2(0,0));
        Inventory.Instance.Fill();
    }

    private void OnRespawnClicked()
    {
        Respawn();
    }

    private void OnDestroy()
    {
        if (_respawnButton)
            _respawnButton.onClick.RemoveAllListeners();
    }
}