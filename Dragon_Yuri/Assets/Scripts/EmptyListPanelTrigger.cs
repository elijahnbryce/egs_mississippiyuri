using System.Collections.Generic;
using UnityEngine;

public class EmptyListPanelTrigger : MonoBehaviour
{
    [Header("Objects to Track")]
    [SerializeField] private List<GameObject> trackedObjects = new List<GameObject>();

    [Header("UI Panel")]
    [SerializeField] private GameObject panelToSpawn;

    private bool panelShown = false;

    private void Start()
    {
        if (panelToSpawn != null)
            panelToSpawn.SetActive(false);
    }

    private void Update()
    {
        if (panelShown) return;

        CleanupNullEntries();

        if (trackedObjects.Count == 0)
        {
            ShowPanel();
        }
    }


    private void CleanupNullEntries()
    {
        trackedObjects.RemoveAll(obj => obj == null);
    }

    private void ShowPanel()
    {
        panelShown = true;

        if (panelToSpawn != null)
        {
            panelToSpawn.SetActive(true);
            Debug.Log("[EmptyListPanelTrigger] Panel activated - list is empty.");
        }
    }


    public void RegisterObject(GameObject obj)
    {
        if (!trackedObjects.Contains(obj))
            trackedObjects.Add(obj);
    }

    public void UnregisterObject(GameObject obj)
    {
        trackedObjects.Remove(obj);
    }
}