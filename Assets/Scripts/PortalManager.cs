using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : Singleton<PortalManager>
{
    public List<Portal> portalList;
    public Dictionary<int, string> portalDict;
    private void Start()
    {
        portalDict = new Dictionary<int, string>();
        portalDict.Add(0, "Scene1");
        portalDict.Add(1, "Scene1");
        portalDict.Add(2, "Scene2");
        

    }
    public void RegistPortal(Portal portal)
    {
        portalList.Add(portal);
    }

    
}
