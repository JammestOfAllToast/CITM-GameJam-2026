using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] AC;
    
    [Header("References")]
    public TimeController TC;
    public PlayerStats stats;
    public Rigidbody rb;
    
    [Header("Settings")]
    public float footstepInterval = 0.5f;
    public float paranoiaThreshold = 90f;
    public float movementThreshold = 0.1f;
    
    private AudioSource AS;
    private float footstepTimer = 0f;
    private bool isGrounded = false;
    public int count = 1;
    
    private AudioClip currentlyPlayingBreath = null;
    private bool hasPlayedAssfixiation = false;
    
    void Start()
    {
        AS = GetComponent<AudioSource>();
        
        if (stats == null)
        {
            stats = GetComponent<PlayerStats>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (TC == null)
        {
            TC = GameObject.FindWithTag("TimeWizard").GetComponent<TimeController>();
        }
    }
    
    void Update()
    {
        if (!GameManager.paused)
        {
            if(TC != null && (int)TC.CurrentTime / Random.Range(15, 30) == count)
            {
                count++;
                AS.pitch = Random.Range(0.5f, 1.5f);
                AS.volume = Random.Range(0.25f, 0.85f);
                AS.PlayOneShot(AC[Random.Range(0, 3)]);
            }
            
            CheckGrounded();
            
            HandleFootsteps();
        }
        HandleBreathing();
    }
    
    void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            if (hit.collider.CompareTag("floor"))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }
    
    void HandleFootsteps()
    {
        bool isMoving = false;
        
        if (rb != null)
        {
            isMoving = rb.linearVelocity.magnitude > movementThreshold;
        }
        
        if (isGrounded && isMoving)
        {
            footstepTimer += Time.deltaTime;
            
            if (footstepTimer >= footstepInterval)
            {
                AS.pitch = Random.Range(0.8f, 1.2f);
                AS.PlayOneShot(AC[4], 0.5f);
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
    
    void HandleBreathing()
    {
        if (stats == null) return;
        
        AudioClip shouldBePlaying = null;

        if (stats.Oxygen <= 0)
        {
            if (!hasPlayedAssfixiation)
            {
                if (currentlyPlayingBreath != null && AS.isPlaying && AS.clip == currentlyPlayingBreath)
                {
                    AS.Stop();
                }
                
                AS.pitch = Random.Range(1f, 1f);
                AS.PlayOneShot(AC[5]);
                hasPlayedAssfixiation = true;
                currentlyPlayingBreath = null;
            }
            return;
        }
        else
        {
            hasPlayedAssfixiation = false;
        }
        
        if (!GameManager.paused)
        {
            if (stats.Paranoia >= paranoiaThreshold && stats.Oxygen > 0)
            {
                shouldBePlaying = AC[7];
            }
            else if (stats.Oxygen > 0)
            {
                shouldBePlaying = AC[6];
            }
            
            if (shouldBePlaying != currentlyPlayingBreath)
            {
                if (currentlyPlayingBreath != null && AS.isPlaying && AS.clip == currentlyPlayingBreath)
                {
                    AS.Stop();
                }
                
                if (shouldBePlaying != null)
                {
                    AS.clip = shouldBePlaying;
                    AS.loop = true;
                    AS.pitch = 1f;
                    AS.Play();
                    currentlyPlayingBreath = shouldBePlaying;
                }
                else
                {
                    currentlyPlayingBreath = null;
                }
            }
        }
        else
        {
            if (currentlyPlayingBreath != null && AS.isPlaying && AS.clip == currentlyPlayingBreath)
            {
                AS.Stop();
                currentlyPlayingBreath = null;
            }
        }
    }
}