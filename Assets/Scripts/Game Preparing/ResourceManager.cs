using UnityEngine;

public static class ResourceManager
{
    public static string GetText(string fileName)
    {
        TextAsset text = Resources.Load<TextAsset>("Configs/Units/" + fileName);
        return text.text;
    }
}