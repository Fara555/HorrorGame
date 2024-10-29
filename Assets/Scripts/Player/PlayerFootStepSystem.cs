using Hearing;
using UnityEngine;
using static Hearing.Sound;

public class PlayerFootStepSystem : MonoBehaviour
{

    [Space(10f), Header("Movement Variables")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Sounds")]
    [SerializeField] private Sound.SoundType soundType = Sound.SoundType.Chasing;
    [SerializeField] private LayerMask soundListeners;

    [Space(5f), Header("Walk Sounds")]
    [SerializeField] private AudioClip snowWalkingSound;
    [SerializeField] private AudioClip rockWalkingSound;


    [Space(5f), Header("Run Sounds")]
    [SerializeField] private AudioClip snowRunningSound;
    [SerializeField] private AudioClip rockRunningSound;

    [Space(5f), Header("Jump Sounds")]
    [SerializeField] private AudioClip snowJumpSound;
    [SerializeField] private AudioClip rockJumpSound;
    [SerializeField] private float jumpSoundRange;

    [Space(5f), Header("Land Sound")]
    [SerializeField] private AudioClip snowLandSound;
    [SerializeField] private AudioClip rockLandSound;


    [SerializeField] private AudioSource jumpSoundSource;
    [SerializeField] private AudioSource walkSoundSource;

    private PlayerMovement playerMovement;
    private RaycastHit hit;
    private Rigidbody rb;
    private bool readyToPlayFootstepSound = true;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isSprinting;

    private void OnDisable()
    {
        if (jumpSoundSource != null) jumpSoundSource.enabled = false;
        if (walkSoundSource != null) walkSoundSource.enabled = false;
    }

    private void OnEnable()
    {
        if (jumpSoundSource != null) jumpSoundSource.enabled = true;
        if (walkSoundSource != null) walkSoundSource.enabled = true;
    }

    private void Start()
    {
       rb = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(sprintKey)) isSprinting = true;
        else isSprinting = false;

        ChangeFootStepSound();
    }

    public void CheckIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f * 0.5f + 0.2f, whatIsGround); //Set maxDistance accordingly to a player height (2f is default)
    }

    private void ChangeFootStepSound()
    {
        if (!isGrounded || rb.velocity.magnitude <= 0.5f)
        {
            if (walkSoundSource.isPlaying)
            {
                walkSoundSource.Pause();
            }
            return;
        }

        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 2f * 0.5f + 0.2f, whatIsGround))
        {
            return;
        }

        if (isSprinting)
        {
            HandleFootsteps("Snow", snowRunningSound, 18f);
            HandleFootsteps("Rocks", rockRunningSound, 18f);
        }
        else
        {
            HandleFootsteps("Snow", snowWalkingSound, 10f);
            HandleFootsteps("Rocks", rockWalkingSound, 18f);
        }
    }

    private void HandleFootsteps(string tag, AudioClip clip, float volume)
    {
        if (hit.collider.CompareTag(tag))
        {
            CreateFootStepSound(volume);

            if (walkSoundSource.clip != clip || !walkSoundSource.isPlaying)
            {
                PlayFootstepSound(clip);
            }
        }
    }

    private void CheckFootStepSound()
    {
        if (isGrounded && rb.velocity.magnitude > 0.5f)
        {
            if (isSprinting)
            {
                CreateFootStepSound(18f);

                if (walkSoundSource.clip != snowRunningSound || !walkSoundSource.isPlaying)
                {
                    PlayFootstepSound(snowRunningSound);
                }
            }
            else
            {
                CreateFootStepSound(10f);

                if (walkSoundSource.clip != snowWalkingSound || !walkSoundSource.isPlaying)
                {
                    PlayFootstepSound(snowWalkingSound);
                }
            }
        }
        else if (walkSoundSource.isPlaying)
        {
            walkSoundSource.Pause();
        }
    }

    public void PlayFootstepSound(AudioClip clip)
    {
        if (!readyToPlayFootstepSound) return;

        walkSoundSource.clip = clip;
        walkSoundSource.Play();

        readyToPlayFootstepSound = false;
        Invoke(nameof(ResetFootstepSoundReady), 0.1f);
    }

    private void CreateFootStepSound(float soundRange)
    {
        var sound = new Sound(transform.position, soundRange, soundType);

        Sounds.MakeSound(sound, soundListeners);

        footStepSoundRange = soundRange;
    }


    private void ResetFootstepSoundReady()
    {
        readyToPlayFootstepSound = true;
    }

    #region JumpSounds
    public void PlayJumpSound()
    {
        jumpSoundSource.clip = snowJumpSound;
        jumpSoundSource.Play();
    }

    public void PlayLandSound()
    {
        jumpSoundSource.clip = snowLandSound;
        jumpSoundSource.Play();

        var sound = new Sound(transform.position, jumpSoundRange, soundType);
        Sounds.MakeSound(sound, soundListeners);
    }

    #endregion

    #region OnDrawGimzos

    [Space(15f), Header("Debug")]
    [SerializeField] private bool showJumpRadius;
    [SerializeField] private bool showFootStepRadius;
    private float footStepSoundRange;

    private void OnDrawGizmos()
    {
        if (showJumpRadius)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, jumpSoundRange);
        }

        if (showFootStepRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, footStepSoundRange);
        }
    }

    #endregion

}
//if (hit.collider.CompareTag("Snow"))
//{
//    if (isSprinting)
//    {
//        CreateFootStepSound(18f);

//        if (walkSoundSource.clip != snowRunningSound || !walkSoundSource.isPlaying)
//        {
//            PlayFootstepSound(snowRunningSound);
//        }
//    }
//    else
//    {
//        CreateFootStepSound(10f);

//        if (walkSoundSource.clip != snowWalkingSound || !walkSoundSource.isPlaying)
//        {
//            PlayFootstepSound(snowWalkingSound);
//        }
//    }
//}
//if (hit.collider.CompareTag("Rocks"))
//{
//    if (isSprinting)
//    {
//        CreateFootStepSound(18f);

//        if (walkSoundSource.clip != rockRunningSound || !walkSoundSource.isPlaying)
//        {
//            PlayFootstepSound(rockRunningSound);
//        }
//    }
//    else
//    {

//    }
//}