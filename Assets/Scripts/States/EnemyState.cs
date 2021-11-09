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

        //Create the force magnitude
        if( _enemy.HasHitPlayer )
        {
            _choosenMagnitudeMagnitude = bestForce;
            _choosenMagnitudeMagnitude = Random.Range( bestForce, bestForce + .04f );
        }
        else if( _enemy.ShouldApplyMoreForce )
        {
            _choosenMagnitudeMagnitude = Random.Range( bestForce + _enemy.AdditonalForce , bestForce + (_enemy.AdditonalForce * 2));
        }
        else
        {
            _choosenMagnitudeMagnitude = Random.Range( bestForce - _enemy.AdditonalForce , bestForce + (_enemy.AdditonalForce * -2));
        }

        PrepareToShoot();

    }

    private async void PrepareToShoot()
    {
        await Task.Delay(600); //Small delay so that the camera can catch up to the enemy

        Vector2 startPosition = ((Vector2)_enemy.transform.position + (new Vector2(-1f, 2f)));
        Vector2 currentDragPoint = startPosition;
        Vector2 shootDirection = Vector2.one;

        float launchMagnitude = 0f;

        //Ui Stuff
        if (_enemy.angleText != null)
        {
            _enemy.angleText.gameObject.SetActive(true);
            _enemy.angleText.SetStartPosition(startPosition);
        }

        if (_enemy.magnitudeText != null)
        {
            _enemy.magnitudeText.gameObject.SetActive(true);
        }

        while (launchMagnitude < _choosenMagnitudeMagnitude) //
        {

            launchMagnitude = (currentDragPoint - startPosition).magnitude;
            shootDirection = (startPosition - currentDragPoint);

            _enemy.DrawLine(startPosition, currentDragPoint);

            //

            _enemy.magnitudeText.SetPosition( currentDragPoint );
            _enemy.magnitudeText.Text.text = $"{launchMagnitude.ToString("F2")}º";

            float degrees = Vector2.Angle( Vector2.right, (currentDragPoint - startPosition) );
            _enemy.angleText.Text.text = $"{degrees.ToString("F2")}º";


            currentDragPoint += _dirToDrag;

            await Task.Delay(30);
        }

        await Task.Delay(600); //Cool delay before shooting

        _enemy.Shoot(shootDirection);
        _enemy.ResetLine();

        //Ui Stuff
        DeactivateUI();
    }

    private void DeactivateUI()
    {
        if (_enemy.angleText != null)
        {
            _enemy.angleText.gameObject.SetActive(false);
        }

        if (_enemy.magnitudeText != null)
        {
            _enemy.magnitudeText.gameObject.SetActive(false);
        }
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