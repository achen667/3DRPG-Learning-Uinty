using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class Portal : MonoBehaviour
{
    public enum PortalCode { A,B,C}
    public enum PortalType { SAME,DIFFERENT}
    public PortalCode portalCode;
    public PortalCode destinationCode;
    public PortalType portalType;
    private GameObject player;
    private NavMeshAgent playerAgent;
    private bool canTp = false;
    //public bool CanTp { get { return CanTp; } }

    private void Start()
    {
        PortalManager.Instance.RegistPortal(this);
        if(portalType == PortalType.DIFFERENT)
             DontDestroyOnLoad(this);
      
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&canTp )
        {
            TeleportPlyaer();
        }
    }
    
    public Transform FindPortal(PortalCode code)
    {
        foreach(var targetPortal in PortalManager.Instance.portalList)
        {
            if (targetPortal.portalCode == code)
            {
                //string portalSceneName = PortalManager.Instance.portalDict[(int)code];
                //if (portalSceneName == SceneManager.GetActiveScene().name)   //same scene
                    return targetPortal.transform;
            }
        }
        return null;
    }
    IEnumerator DifSceneTeleport(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            canTp = true;

        }       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTp = false;
        }
    }

    public void TeleportPlyaer()
    {
        Transform destination;
        destination = FindPortal(destinationCode);
        if(destination == null)   // not in this scene
        {
            string portalSceneName = PortalManager.Instance.portalDict[(int)destinationCode];             Debug.Log(portalSceneName);Debug.Log((int)destinationCode);
            //StartCoroutine( DifSceneTeleport(portalSceneName));
            SceneManager.LoadScene(portalSceneName);
            destination = FindPortal(destinationCode);
            if (destination == null) Debug.Log("NOT FOUND PORTAL");
            playerAgent = player.gameObject.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.position = destination.position;
            playerAgent.enabled = true;
            Destroy(gameObject);
            return;
        }
        playerAgent = player.gameObject.GetComponent<NavMeshAgent>();
        playerAgent.enabled = false;
        player.transform.position = destination.position;
        playerAgent.enabled = true;
    }
}
