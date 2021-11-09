using UnityEngine;
using MyBox;
using Cinemachine;

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

    [Foldout("End Game Stuff", true)]
    [SerializeField]
    private CinemachineVirtualCamera _endCam;
    [SerializeField]
    private GameObject _endPanel;
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
        SetState( new EndGameState ( this ));
        _player.CanShoot = false;
        _enemy.CanShoot = false;

        if( _endCam != null )
        {
            _endCam.enabled = true;
        }
        if( _endPanel != null )
        {
            _endPanel.SetActive( true );
        }

        //See who died
        if( character == _enemy )
        {
            _endCam.m_Follow = _player.transform;
        }
        else
        {
            _endCam.m_Follow = _enemy.transform;
        }
    }

    private void OnDisable()
    {
        
    }
}