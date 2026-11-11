using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] PlayerUIHudManager playerUIHudManager;

    private void Awake()
    {
        playerUIHudManager = GetComponent<PlayerUIHudManager>();

    }

    public void StartNewGame()
    {
        if (WorldSaveGameManager.Instance == null)
        {
            Debug.LogError("Cannot start a new game: WorldSaveGameManager is missing from the title scene.", this);
            return;
        }

        WorldSaveGameManager.Instance.StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
        // playerUIHudManager.healthBar.enabled = true;
        // playerUIHudManager.staminaBar.enabled = true;
    }
}
