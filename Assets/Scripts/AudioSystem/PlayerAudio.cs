using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Rigidbody2D rb;

    [Header("Jump Settings")]
    [SerializeField] private AudioClip jumpClip;

    [Header("Footstep Settings")]
    [SerializeField] private AudioClip[] footstepClips; // Array for variety
    [SerializeField] private float stepRate = 0.3f; // Time between steps (lower = faster running sound)
    [SerializeField] private float minSpeedForSteps = 0.1f;
    [Range(0.1f, 0.8f)]
    [SerializeField] private float pitchVariation = 0.1f; // Makes it sound less robotic

    private AudioSource source;
    private float stepTimer;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleFootsteps();
    }

    public void PlayJump()
    {
        // Randomize pitch slightly for jump too
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(jumpClip);
    }

    private void HandleFootsteps()
    {
        // Check 1: Are we moving horizontally?
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > minSpeedForSteps;

        // Check 2: Are we on the ground? (Requires exposed bool from Controller)
        bool isGrounded = controller.IsGrounded; 

        if (isMoving && isGrounded)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayOneFootstep();
                stepTimer = stepRate; // Reset timer
            }
        }
        else
        {
            // Reset timer so the moment we land/start moving, a step plays immediately
            stepTimer = 0f;
        }
    }

    private void PlayOneFootstep()
    {
        if (footstepClips.Length == 0) return;

        // Pick a random clip
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        // Randomize pitch to make it sound natural (Human ear hates repetition)
        source.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        
        source.PlayOneShot(clip);
    }
}