using Flexalon;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuyButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private Producer producer;
    private FlexalonGridLayout gridLayout;
    private Image[] childImages;
    private int maxGridChildren;
    void Awake()
    {
        childImages = new Image[0];
        button = GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.LogError("Le composant Button est introuvable sur cet objet !");
            return;
        }

        button.onClick.AddListener(InstantiatePrefabInGrid);

        gridLayout = FindObjectOfType<FlexalonGridLayout>();
        if (gridLayout == null)
        {
            Debug.LogError("Aucun FlexalonGridLayout trouvé dans la scène !");
        }
    }



    private void InstantiatePrefabInGrid()
    {
        if (producer == null || producer.prefab == null)
        {
            Debug.LogError("Le Producer ou son prefab est null !");
            return;
        }

        if (gridLayout.transform.childCount >= GetMaxGridChildren())
        {
            Debug.Log("Le nombre maximum d'enfants est atteint. Impossible d'en ajouter un autre.");
            return; 
        }

        Debug.Log("Le nombre d'enfants est dans la limite autorisée.");
        GameObject prefabInstance = Instantiate(producer.prefab, Vector3.zero, Quaternion.identity);
        prefabInstance.transform.SetParent(gridLayout.transform, false);
    }



    private void SetButtonInfos()
    {
        if (producer == null)
        {
            Debug.LogError("Le Producer est null !");
            return;
        }

        if (producer.image == null)
        {
            Debug.LogError("L'image du Producer est null !");
            return;
        }

        if (button == null)
        {
            Debug.LogError("Le bouton est null !");
            return;
        }

        childImages = button.GetComponentsInChildren<Image>();
        Debug.Log($"Nombre d'images trouvées : {childImages.Length}");

        if (childImages.Length > 1 && childImages[1] != null)
        {
            childImages[1].sprite = producer.image;
        }
        else
        {
            Debug.LogError("L'image enfant à l'index 1 est introuvable ou inexistante !");
        }

        var textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = producer.price.ToString();
        }
        else
        {
            Debug.LogError("Aucun composant TextMeshProUGUI trouvé dans les enfants du bouton !");
        }
    }



    public void SetProducer(Producer newProducer)
    {
        if (newProducer == null)
        {
            Debug.LogError("Le Producer passé à SetProducer est null !");
            return;
        }

        producer = newProducer;
        SetButtonInfos();
    }

    private int GetMaxGridChildren()
    {
        maxGridChildren = (int)(gridLayout.Columns * gridLayout.Layers); 
        return maxGridChildren;
    }


}
