using UnityEngine;
using UnityEngine.UI;


public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;
    GameManager manager;
    public int m_Ammo = 5;
    public Slider m_Slider;
    public Image m_FillImage;


    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired = true;            //set to true so I can avoid a firing bug I may have created (elif !m_Fired)
    private float nextFireTime;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if (m_Ammo > 0)
        {
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire(m_CurrentLaunchForce, 1);

            }
            else if (Input.GetButtonDown(m_FireButton))
            {
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

            }
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;

                m_ShootingAudio.clip = m_ChargingClip;
                if (!m_ShootingAudio.isPlaying)
                {
                    m_ShootingAudio.Play();     //maybe put elsewhere

                }

            }
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                Fire(m_CurrentLaunchForce, 1);
            }
            else if (m_CurrentLaunchForce - m_MinLaunchForce > .01 && !m_Fired)
            {
                Fire(m_CurrentLaunchForce, 1);
            }
            else if (!m_Fired)
            {
                Fire(m_CurrentLaunchForce, 1);
            }
        }
    }

    public void SetAmmoUI()
    {
        // Adjust the value and colour of the slider.

        //Debug.Log(m_Slider);
        m_Slider.value = m_Ammo + .2f;
    }
    /*
    public void Fire()
    {
        //force, rate
        // Instantiate and launch the shell.
        m_Fired = true;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;

    }
    */
    public void Fire(float launchForce, float fireRate)
    {
        if (Time.time > nextFireTime && m_Ammo > 0)
        {
            m_Ammo -= 1;
            nextFireTime = Time.time + fireRate;
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            // Reset the launch force.  This is a precaution in case of missing button events.
            m_CurrentLaunchForce = m_MinLaunchForce;
            SetAmmoUI();

        }

    }
}
