using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanels : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        leftPanel.SetActive(!leftPanel.activeSelf);
        rightPanel.SetActive(!rightPanel.activeSelf);
    }


}
