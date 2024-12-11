using UnityEngine;

public class PlayerFootstepAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] grassStepSounds;
    [SerializeField] private AudioClip[] puddleStepSounds;
    [SerializeField] private AudioClip[] floorStepSounds;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float stepInterval = 0.3f;
    [SerializeField] private float stepVolume = 0.5f;

    [Header("Terrain Detection")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float raycastDistance = 0.1f;

    private float stepTimer;
    private bool isMoving;

    private void Update()
    {
        // Cek apakah player sedang bergerak
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

        // Hitung timer untuk step sound
        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private void PlayFootstepSound()
    {
        // Deteksi jenis permukaan
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayers);

        if (hit.collider != null)
        {
            AudioClip[] selectedSounds = null;

            // Pilih suara berdasarkan layer
            switch (hit.collider.gameObject.layer)
            {
                case var layer when layer == LayerMask.NameToLayer("Base"):
                    selectedSounds = grassStepSounds;
                    break;
                case var layer when layer == LayerMask.NameToLayer("Boss Field"):
                    selectedSounds = puddleStepSounds;
                    break;
                case var layer when layer == LayerMask.NameToLayer("Cosmetic"):
                    selectedSounds = floorStepSounds;
                    break;
                default:
                    selectedSounds = grassStepSounds; // Default
                    break;
            }

            // Putar suara step acak
            if (selectedSounds != null && selectedSounds.Length > 0)
            {
                AudioClip randomStepSound = selectedSounds[Random.Range(0, selectedSounds.Length)];
                audioSource.PlayOneShot(randomStepSound, stepVolume);
            }
        }
    }

    // Opsional: Debug garis raycast
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * raycastDistance);
    }
}