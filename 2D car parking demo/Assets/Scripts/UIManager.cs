using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject carCrashedPanel;
    [SerializeField] private GameObject parkedPanel;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void EnableCarCrashedPanel()
    {
        carCrashedPanel.SetActive(true);
    }
    public void DisbleCarCrashedPanel()
    {
        carCrashedPanel.SetActive(false);
    }
    public void EnableParkedPanel()
    {
        parkedPanel.SetActive(true);
    }
    public void DisableParkedPanel()
    {
        parkedPanel.SetActive(false);
    }
}
