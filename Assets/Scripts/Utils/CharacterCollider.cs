using System;
using UnityEngine;
public class CharacterCollider : MonoBehaviour
{
    [SerializeField] BodyPartStatus _bodyPart;
    public BodyPartStatus BodyStatus => _bodyPart;

    public Action<int> OnDamageTaken;
}

[System.Serializable]
public struct BodyPartStatus
{
    public BodyPart part;
    public int damageTaken;

    public void SetDamage( int dmg )
    {
        damageTaken = dmg;
    }
}

public enum BodyPart
{
    Head,
    Body,
    Jordans
}