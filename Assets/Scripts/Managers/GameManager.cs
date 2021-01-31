using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 0f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;              
    public GameObject[] m_TankPrefabs;
    public GameObject[] m_RefillPrefabs;
    public TankManager[] m_Tanks;
    public Transform[] m_Consumables;
    public List<Transform> AIWaypoints;
    public static int gameMode = 0; //0: Player, 1: AI
    public int startGameMode = 0;
    //public bool paused = true;

    public int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;       


    private void Start()
    {
        Debug.Log(gameMode);
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SpawnCollectables();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void SpawnCollectables()
    {
        //ammo collectable
        int waypoint = Random.Range(0, AIWaypoints.Count);
        m_RefillPrefabs[0] = Instantiate(m_RefillPrefabs[0], AIWaypoints[waypoint].position, Quaternion.identity);
        AmmoRefill.refillWaypoint = AIWaypoints[waypoint].transform.position;
        //hp collectable
        int waypoint1 = Random.Range(0, AIWaypoints.Count);
        if (waypoint != waypoint1)
        {
            m_RefillPrefabs[1] = Instantiate(m_RefillPrefabs[1], AIWaypoints[waypoint1].position, Quaternion.identity);
            HPRefill.refillWaypoint = AIWaypoints[waypoint1].transform.position;
        }
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            //Debug.Log(m_Tanks.Length);
            if (gameMode == 0 || i == 0)
            {

                m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefabs[0], m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;
                m_Tanks[i].SetupPlayer();
            }
            else if (gameMode == 1)     //AI
            {
                //may want to change this line if adding arbitrarily many tanks
                
                m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefabs[i], m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;
                /*
                if (i < 1)
                {
                    m_Tanks[i].SetupPlayer();
                }
                else
                {
                    m_Tanks[i].SetupAI(AIWaypoints);
                }
                */
                //Debug.Log("Pre-AI Setup");
                //Debug.Log(AIWaypoints.Count);
                m_Tanks[i].SetupAI(AIWaypoints);
                //Debug.Log("Post-AI Setup");
            }
            else
            {
                Debug.Log("Gamemode not implemented: " + gameMode);
            }
            
        }
        SetCameraTargets();
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length + m_RefillPrefabs.Length];

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }
        
        for (int i = m_Tanks.Length; i < targets.Length; i++)
        {
            //Debug.Log("Consumable");
            targets[i] = m_RefillPrefabs[i - m_Tanks.Length].transform;
        }
        
        //Debug.Log(m_RefillPrefabs[0].transform.position);

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null || PauseMenu.modeChanged)
        {
            PauseMenu.modeChanged = false;
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl(); //probably optional
        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "Round " + m_RoundNumber;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = "";
        while (!OneTankLeft())
        {
            /*
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = true;
            }
            */
            yield return null;

        }
    }


    private IEnumerator RoundEnding()
    {

        DisableTankControl();
        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();
        if (m_RoundWinner != null)
        {
            m_RoundWinner.m_Wins++;
        }

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins >= m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }

    public void SwapGameMode()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance.GetComponent<TankHealth>().OnDeath();

        }
        SpawnAllTanks();
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Wins = 0;
        }
        m_RoundNumber = 0;
        StopAllCoroutines();

        StartCoroutine(GameLoop());
        
        //manager.EndMessage();
    }


}