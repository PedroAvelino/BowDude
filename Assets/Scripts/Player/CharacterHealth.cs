using UnityEngine;
using MyBox;
using System;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private int _currenthealth;
    [SerializeField] private int _maxHealth = 15;

    [SerializeField] private CharacterCollider[] _cols;

    private void OnEnable()
    {
        foreach ( var c in _cols )
        {
            if ( c == null ) return;
            c.OnDamageTaken += ManageHealth;

            //Set the head damage while we are at it
            if ( c.BodyStatus.part == BodyPart.Head )
            {
                c.BodyStatus.SetDamage( _maxHealth );
            }
        }
    }

    private void Start()
    {
        ResetStats();
    }

    private void ResetStats()
    {
        _currenthealth = _maxHealth;
    }

    private void ManageHealth( int dmg )
    {
        _currenthealth -= dmg;
    }
    private void OnDisable()
    {
        foreach ( var c in _cols )
        {
            if ( c == null ) return;
            c.OnDamageTaken -= ManageHealth;
        }
    }

    
}