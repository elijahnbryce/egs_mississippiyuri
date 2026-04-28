using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public enum ElementType
    {
        Fire,
        Water,
        Neutral
    }

    [Header("Basic Info")]
    public string enemyName;

    [Header("Stats")]
    public float moveSpeed = 2f;
    public int maxHealth = 10;

    [Header("Element")]
    public ElementType type;
    public ElementType weakness;
    public ElementType strength;
}