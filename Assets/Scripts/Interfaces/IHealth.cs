using UnityEngine;

public interface IHealth
{
    public int Health { get; set; }

    public void TakeDamage(int damage);
}