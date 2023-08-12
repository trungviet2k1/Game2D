using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite spriteName;
    public string lootName;
    public int dropChance;

    public Loot(string lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
