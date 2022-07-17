using UnityEngine;

[System.Serializable]
public class SpriteCategory
{
    public Sprite[] Sprites;
}

[CreateAssetMenu(fileName = "DataTable_Sprite", menuName = "DataTable/DataTable_Sprite")]
public class DataTable_Sprite : ScriptableObject
{
    public SpriteCategory[] SpritesCategory;
}
