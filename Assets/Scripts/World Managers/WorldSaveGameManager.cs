using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;

    [SerializeField] int worldSceneIndex = 1;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Vector3 playerSpawnPosition;
    [SerializeField] float playerSpawnRotationY;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Cannot start the game: Player Prefab is not assigned on WorldSaveGameManager.", this);
            yield break;
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        if (loadOperation == null)
        {
            Debug.LogError($"Cannot load world scene at build index {worldSceneIndex}.", this);
            yield break;
        }

        while (!loadOperation.isDone)
            yield return null;

        GameObject player = Instantiate(
            playerPrefab,
            playerSpawnPosition,
            Quaternion.Euler(0f, playerSpawnRotationY, 0f));
        if (player == null)
        {
            Debug.LogError("Player prefab instantiation failed.", this);
            yield break;
        }

        Debug.Log($"Spawned player '{player.name}' at {player.transform.position}.", player);
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
