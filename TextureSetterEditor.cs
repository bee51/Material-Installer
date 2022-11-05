using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using YeappGame.Utilities;

namespace Game.Scripts.Play.Editor
{
    public class TextureSetterEditor : EditorWindow
    {
        [MenuItem("TextureEditors/TextureSetter")]
        private static void ShowWindow()
        {
            var window = GetWindow<TextureSetterEditor>();
            window.titleContent = new GUIContent("Texture Folder Setter");
            window.Show();
        }

        private void OnEnable()
        {
        }


        private void OnGUI()
        {
            var selectedObjects = Selection.objects;
            var selectedFirstObject = Selection.activeObject;
            if (GUILayout.Button("INSTALL MATERIALS"))
            {
                var assetPath = AssetDatabase.GetAssetPath(selectedFirstObject);
              
            
                if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(assetPath) + "/Materials"))
                {
                    AssetDatabase.CreateFolder(Path.GetDirectoryName(assetPath), "Materials");
                    AssetDatabase.CreateFolder(Path.GetDirectoryName(assetPath) + "/Materials", "Textures");

                }


                foreach (var objectItem in selectedObjects)
                {
                    var objectType = objectItem.GetType();
                    if (objectType == typeof(Material))
                    {
                        Material materialObject = (Material) objectItem;
                        var color = materialObject.color;
                        var albedoMap = materialObject.mainTexture;
                        Shader toonyColorsShader = Shader.Find("Toony Colors Pro 2/Hybrid Shader");
                        materialObject.shader = toonyColorsShader;
                        materialObject.color = color;
                        if (albedoMap != null)
                        {
                            materialObject.mainTexture = albedoMap;
                        }

                        var firstPath = AssetDatabase.GetAssetPath(materialObject);
                        var fileName=Path.GetFileName(firstPath);
                        AssetDatabase.MoveAsset(firstPath, Path.GetDirectoryName(assetPath) + "/Materials/" + fileName);
                    }
                    else if (objectType == typeof(Texture2D))
                    {
                        Texture2D textureObject = (Texture2D) objectItem;

                        var textureObjectPath = AssetDatabase.GetAssetPath(textureObject);
                        
                        var fileName=Path.GetFileName(textureObjectPath);

                        AssetDatabase.MoveAsset(textureObjectPath,
                            Path.GetDirectoryName(assetPath) + "/Materials/Textures/" +fileName);
                    }
                }
            }
        }
    }
}