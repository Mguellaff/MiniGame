using UnityEditor;
using UnityEngine;

public class FBXMeshExtractor : MonoBehaviour
{
    [MenuItem("Tools/Extract Meshes from FBX")]
    public static void ExtractMeshes()
    {
        // Obtenir l'objet sélectionné dans la fenêtre Project
        Object selectedObject = Selection.activeObject;

        if (selectedObject == null)
        {
            Debug.LogError("Veuillez sélectionner un fichier FBX dans la fenêtre Project.");
            return;
        }

        // Vérifier si l'objet sélectionné est un fichier FBX
        string assetPath = AssetDatabase.GetAssetPath(selectedObject);
        if (!assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.LogError("L'objet sélectionné n'est pas un fichier FBX.");
            return;
        }

        // Charger tous les assets contenus dans le fichier FBX
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        // Créer un dossier pour stocker les meshes extraits
        string folderPath = "Assets/ExtractedMeshes";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "ExtractedMeshes");
        }

        // Parcourir les assets et extraire les Meshes
        foreach (Object asset in assets)
        {
            if (asset is Mesh mesh)
            {
                string meshPath = $"{folderPath}/{mesh.name}.asset";
                AssetDatabase.CreateAsset(Object.Instantiate(mesh), meshPath);
                Debug.Log($"Mesh extrait : {mesh.name}");
            }
        }

        // Sauvegarder et rafraîchir les assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Extraction des meshes terminée !");
    }
}
