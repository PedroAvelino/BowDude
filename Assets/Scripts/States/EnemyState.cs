using System.Threading.Tasks;
using UnityEngine;

public class EnemyState : BaseState
{
    private Player _player;
    private Enemy _enemy;

    //Shot properties
    private float _choosenMagnitudeMagnitude = 0f;
    private Vector2 _dirToDrag = new Vector2( 1f, -.5f ); //diagonal down

    public EnemyState(CombatManager manager) : base(manager)
    {
    }

    public override void OnStateEnter()
    {
        _player = Manager.Player;
        _enemy = Manager.Enemy;

        Arrow.OnArrowStoppedAction += ManageState;//Subscribe to the event
        Manager.Enemy.CNCam.enabled = true;
        Manager.Enemy.CanShoot = true;

        CalculateEnemyShot();
    }

    private void CalculateEnemyShot()
    {
        float bestForce = _enemy.BestForceUsed;
        Vector2 distanceFromPlayer = ( _enemy.BestArrowPosition - (Vector2)_player.transform.position );

        if( _enemy.HasHitPlayer )
        {
            _choosenMagnitudeMagnitude = _enemy.BestForceUsed;
        }
        else if( _enemy.ShouldApplyMoreForce )
        {
            _choosenMagnitudeMagnitude =  bestForce + _enemy.AdditonalForce;
        }
        else
        {
            _choosenMagnitudeMagnitude = bestForce - _enemy.AdditonalForce;
        }

        PrepareToShoot();

    }

    private async void PrepareToShoot()
    {
        await Task.Delay( 600 ); //Small delay so that the camera can catch up to the enemy

        Vector2 startPosition = ( (Vector2)_enemy.transform.position + (new Vector2( -1f, 2f ) ));
        Vector2 currentDragPoint = startPosition;
        Vector2 shootDirection = Vector2.one;
        
        float launchMagnitude = 0f;

        while ( launchMagnitude < _choosenMagnitudeMagnitude ) //
        {

            launchMagnitude = ( currentDragPoint - startPosition ).magnitude;
            shootDirection = ( startPosition - currentDragPoint );

            _enemy.DrawLine( startPosition, currentDragPoint );

            currentDragPoint += _dirToDrag;

            await Task.Delay( 30 );
        }
        
        await Task.Delay( 600 ); //Cool delay before shooting

        _enemy.Shoot(  shootDirection );
        _enemy.ResetLine();
    }

    private void ManageState( Arrow arrow )
    {
        if( arrow == null || arrow.Owner != Manager.Enemy ) return;

        //Leave this state
        Manager.SetState( new PlayerState( Manager) );
    }

    public override void OnExitState()
    {
        Arrow.OnArrowStoppedAction -= ManageState;//Unsubscribe from the event
        Manager.Enemy.CNCam.enabled = false;
    }
}