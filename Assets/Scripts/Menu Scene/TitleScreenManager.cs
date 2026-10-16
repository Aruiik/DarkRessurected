using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNewGame()
    {
        if (WorldSaveGameManager.Instance == null)
        {
            Debug.LogError("Cannot start a new game: WorldSaveGameManager is missing from the title scene.", this);
            return;
        }

        WorldSaveGameManager.Instance.StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
    }
}
