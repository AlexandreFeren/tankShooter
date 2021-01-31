using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;            
    public Transform m_SpawnPoint;         
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;
    [HideInInspector] public List<Transform> m_WaypointList;
    public int m_Ammo = 5;
    TankShooting ShootScript;

    //public MonoBehaviour 

    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
    private AIStateController m_AIStateController;

    public void SetupAI(List<Transform> wayPointList)
    {
        m_AIStateController = m_Instance.GetComponent<AIStateController>();
        //Debug.Log(m_AIStateController);
        //Debug.Log(m_Instance);
        m_AIStateController.SetupAI(true, wayPointList);

        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }
    public void SetupPlayer()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();

        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public void DisableControl()
    {
        //m_Movement.enabled = false;
        //m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        //m_Movement.enabled = true;
        //m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        ShootScript = m_Instance.GetComponent<TankShooting>();
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;
        
        ShootScript.m_Ammo = 5;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
        ShootScript.SetAmmoUI();

    }
}
