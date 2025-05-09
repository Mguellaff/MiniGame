using UnityEditor;
using UnityEngine;

public class FBXMeshExtractor : MonoBehaviour
{
    [MenuItem("Tools/Extract Meshes from FBX")]
    public static void ExtractMeshes()
    {
        // Obtenir l'objet s�lectionn� dans la fen�tre Project
        Object selectedObject = Selection.activeObject;

        if (selectedObject == null)
        {
            Debug.LogError("Veuillez s�lectionner un fichier FBX dans la fen�tre Project.");
            return;
        }

        // V�rifier si l'objet s�lectionn� est un fichier FBX
        string assetPath = AssetDatabase.GetAssetPath(selectedObject);
        if (!assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.LogError("L'objet s�lectionn� n'est pas un fichier FBX.");
            return;
        }

        // Charger tous les assets contenus dans le fichier FBX
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        // Cr�er un dossier pour stocker les meshes extraits
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

        // Sauvegarder et rafra�chir les assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Extraction des meshes termin�e !");
    }
}
