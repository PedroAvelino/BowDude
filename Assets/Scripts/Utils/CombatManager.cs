using UnityEngine;
using MyBox;

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
}