using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CityDemands : MonoBehaviour
{
    private int shirtDemand = 1;
    private float demandTimer = 0f;
    private bool isGenerating = true;

    [SerializeField] private float productionTime;
    [SerializeField] private float timeLimit;
    [SerializeField] private Image timeImage;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private int demandLimit = 5;
    [SerializeField] private TextMeshProUGUI shirtDemandText;

    public event Action<int> OnShirtDemandChanged;

    private float ProductionTime
    {
        get => productionTime;
        set => productionTime = Mathf.Max(1f, value);
    }

    // Propri�t� pour encapsuler shirtDemand
    private int ShirtDemand
    {
        get => shirtDemand;
        set
        {
            shirtDemand = value;
            OnShirtDemandChanged?.Invoke(shirtDemand); // D�clenche l'�v�nement
        }
    }

    void Start()
    {
        StartCoroutine(GenerateDemand());
        timeImage.fillAmount = 0f;

        // Abonnez-vous � l'�v�nement pour mettre � jour l'UI
        OnShirtDemandChanged += UpdateShirtDemandUI;
        UpdateShirtDemandUI(shirtDemand); // Initialisez l'UI avec la valeur actuelle
    }

    void Update()
    {
        if (shirtDemand > demandLimit)
        {
            demandTimer += Time.deltaTime;
            timeImage.fillAmount = Mathf.Clamp01(demandTimer / timeLimit);

            if (demandTimer >= timeLimit)
            {
                Debug.Log("Demand not met in time. Reloading scene...");
                isGenerating = false;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                endCanvas.SetActive(true);
            }
        }
        else
        {
            if (demandTimer > 0f)
            {
                demandTimer -= Time.deltaTime;
                timeImage.fillAmount = Mathf.Clamp01(demandTimer / timeLimit);
            }
        }
    }

    private IEnumerator GenerateDemand()
    {
        while (isGenerating)
        {
            yield return new WaitForSeconds(productionTime);
            ShirtDemand++; // Utilisez la propri�t� pour d�clencher l'�v�nement
            ProductionTime--;
            Debug.Log("Increased shirt demand: " + shirtDemand);
        }
    }

    // M�thode pour mettre � jour l'UI
    private void UpdateShirtDemandUI(int newDemand)
    {
        if (shirtDemandText != null)
        {
            shirtDemandText.text = $"Shirt Demand: {newDemand}";
        }
    }
}
