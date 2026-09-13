using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GameData/EnemyData")]
public class SO_EnemyData : ScriptableObject
{
    public enum EnemyType
    {
        Walker,
        Runner,
        BigBull
    }

    public EnemyType m_EnemyType;

    public int m_EnemyHealth;
    public float m_EnemySpeed;
    public int m_Damage;
    public int m_KillScore;
}