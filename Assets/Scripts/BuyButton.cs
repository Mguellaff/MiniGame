using Flexalon;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuyButton : MonoBehaviour
{
    private Button button;
    private Producer producer;
    private FlexalonGridLayout gridLayout;
    private Image[] childImages;
    private int maxGridChildren;
    private TextMeshProUGUI textComponent;
    private UnitProduction unitProduction;
    private GameObject prefabInstance;
    private InventoryManager inventoryManager;
    [SerializeField] private AudioClip spawnSound;
    private AudioSource audioSource;

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
        inventoryManager = InventoryManager.Instance;
        audioSource = FindFirstObjectByType<AudioSource>();
    }

    // Méthode pour instancier un prefab dans la grille
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

        if (inventoryManager.SpendResource("Money", producer.price))
        {
            audioSource.PlayOneShot(spawnSound);
            prefabInstance = Instantiate(producer.prefab, Vector3.zero, Quaternion.identity);
            unitProduction = prefabInstance.GetComponent<UnitProduction>();
            if (unitProduction != null)
            {
                unitProduction.SetProducer(producer);
            }
        }
        else
        {
            Debug.LogError("Le composant UnitProduction est introuvable sur l'instance du prefab !");
        }

        prefabInstance.transform.SetParent(gridLayout.transform, false);
    }

    // Méthode pour configurer les informations du bouton (image et texte)
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

        textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = producer.price.ToString();
        }
        else
        {
            Debug.LogError("Aucun composant TextMeshProUGUI trouvé dans les enfants du bouton !");
        }
    }

    // Méthode pour définir un nouveau Producer et mettre à jour les informations du bouton
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

    // Méthode pour calculer le nombre maximum d'enfants autorisés dans la grille
    private int GetMaxGridChildren()
    {
        maxGridChildren = (int)(gridLayout.Columns * gridLayout.Layers);
        return maxGridChildren;
    }
}
