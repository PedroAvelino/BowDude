using UnityEngine;
using MyBox;
using System;

public class CombatManager : MonoBehaviour
{

    [ReadOnly] [SerializeField]
    private BaseState _currentState;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Enemy _enemy;
    public Player Player => _player;
    public Enemy Enemy => _enemy;
    public CameraManager Camera { get; private set; }
    
    private void OnEnable()
    {
        CharacterHealth.OnDeathAction += CheckWhoWon;
    }

    private void Start()
    {
        SetState( new PlayerState( this ) );
    }

    public void SetState( BaseState newState )
    {
        if( newState == null ) return;

        if( _currentState != null )
        {
            _currentState.OnExitState(); //Exit the current state
        }

        _currentState = newState;   //Attribute new state

        _currentState.OnStateEnter(); //initiate new state
    }

    private void CheckWhoWon( BowController character )
    {
        
    }

    private void OnDisable()
    {
        
    }
}