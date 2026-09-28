using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public GameObject laserPrefab;

    //Settings for just this script
    private float shakeAmplitude = 2f;
    private float shakeDuration = 0.3f;
    private float zoomOutAmount = 1.5f;
    private float zoomSpeed = 2f;
    private float speed = 6f;
    private float horizontalScreenLimit = 10f;
    private float verticalScreenLimit = 6f;
    private bool canShoot = true;
    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private float normalZoom;
    private float shakeTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cinemachineCamera = GameManager.Instance.cinemachineCamera;
        noise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        normalZoom = cinemachineCamera.m_Lens.FieldOfView;
        cinemachineCamera.Follow = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
        CameraEffects();
    }

    //Change old movement logic to new system
    void Movement()
    {
        Keyboard keyboard = Keyboard.current;
        float horizontal = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
        float vertical = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);

        transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * speed);
        Vector3 positionBeforeWrap = transform.position;
        if (transform.position.x > horizontalScreenLimit || transform.position.x <= -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1f, transform.position.y, 0);
        }
        if (transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }

        // Tell the camera if we wrapped so it follows the teleport instantly
        Vector3 changeDelta = transform.position - positionBeforeWrap;
        if (changeDelta != Vector3.zero)
        {
            cinemachineCamera.OnTargetObjectWarped(transform, changeDelta);
        }
    }

    void Shooting()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && canShoot)
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            canShoot = false;
            StartCoroutine("Cooldown");
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }

    void CameraEffects()
    {
        // Zoom out (wider field of view) while a big meteor is alive, ease back in once it's gone
        float targetZoom = GameManager.Instance.BigMeteorActive ? normalZoom * zoomOutAmount : normalZoom;
        cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.m_Lens.FieldOfView, targetZoom, Time.deltaTime * zoomSpeed);

        // Shake fades out over shakeDuration
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            noise.m_AmplitudeGain = shakeAmplitude * Mathf.Max(shakeTimer, 0f) / shakeDuration;
        }
        else
        {
            noise.m_AmplitudeGain = 0f;
        }
    }

    public void Shake()
    {
        shakeTimer = shakeDuration;
    }

    void OnDestroy()
    {
        //Don't leave the camera shaking if we die mid-shake
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
        }
    }
}