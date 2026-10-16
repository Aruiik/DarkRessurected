using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public PlayerManager player;    
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;


    [Header("Camera Settings")]
    private float cameraSmoothSpeed= 1f;                    // IM WIĘKSZA WARTOŚC TYM DŁUŻEJ KAMERZE ZAJMIE DOJŚCIE DO GRACZA
    [SerializeField] float leftAndRightRotationSpeed = 220f;         
    [SerializeField] float upAndDownRotationSpeed = 220f;
    [SerializeField] float minimumPivot = -30f;             // MINIMALNY KĄT OBRACANIA KAMERY W GÓRĘ
    [SerializeField] float maximumPivot = 60f;              // MAKSYMALNY KĄT OBRACANIA KAMERY W DÓŁ
    [SerializeField] float cameraCollisionRadius = 0.2f;    // PROMIEŃ KOLIZJI KAMERY
    [SerializeField] LayerMask collideWithLayers;           // WARSTWY Z KTÓRYMI KAMERA BĘDZIE WCHODZIĆ W KOLIZJĘ


    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;                          // DOMYŚLNA POZYCJA KAMERY
    private float targetCameraZPosition;                    // POZYCJA KAMERY W ZALEŻNOŚCI OD KOLIZJI

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
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraZPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraZPosition;
    }

    private void HandleRotations()
    {
        // ROTACJA KAMERY W LEWO I PRAWO NA PODSTAWIE HORYZONTALNEGO INPUTU GRACZA
        leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        // ROTACJA KAMERY W GÓRĘ I DÓŁ NA PODSTAWIE WERTYKALNEGO INPUTU GRACZA
        upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        // OGRANICZENIE KĄTA OBRACANIA KAMERY W GÓRĘ I DÓŁ
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // OBLICZENIE ROTACJI KAMERY W LEWO I PRAWO

        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // OBLICZENIE ROTACJI KAMERY W GÓRĘ I DÓŁ
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;

        // OBLICZENIE KIERUNKU PROMIENIA OD PIVOTU KAMERY DO OBIEKTU KAMERY
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        // SPRAWDZENIE CZY KAMERA WCHODZI W KOLIZJĘ Z OBIEKTAMI NA WARSTWACH OKREŚLONYCH W collideWithLayers

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            // OBLICZENIE ODLEGŁOŚCI OD PUNKTU UDERZENIA PROMIENIA DO PIVOTU KAMERY
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;

    }


}