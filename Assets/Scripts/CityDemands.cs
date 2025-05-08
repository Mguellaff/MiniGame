using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CityDemands : MonoBehaviour
{
    private int shirtDemand = 1;
    private float demandTimer = 0f;
    private bool isGenerating = true;

    [SerializeField] private float productionTime;
    [SerializeField] private float timeLimit;
    [SerializeField] private Image timeImage;
    [SerializeField] private GameObject endCanvas;

    void Start()
    {
        StartCoroutine(GenerateDemand());
        timeImage.fillAmount = 0f;
    }

    void Update()
    {
        if (shirtDemand > 0)
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
            shirtDemand++;
            productionTime--;
            Debug.Log("Increased shirt demand: " + shirtDemand);
        }
    }
}
