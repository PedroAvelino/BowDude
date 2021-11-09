using UnityEngine;
using MyBox;

public class Enemy : BowController
{
    [SerializeField] [ReadOnly]
    private Vector2 _bestArrowPosition = Vector2.zero;
    
    [SerializeField] 
    private float _bestForceUsed = 5f;

    [SerializeField] [ReadOnly]
    private bool _shouldApplyMoreForce = true;
    [SerializeField] private Player _player;

    [SerializeField] [ReadOnly]
    private bool _hasHitPlayer;
    public bool HasHitPlayer => _hasHitPlayer;
    public float BestForceUsed => _bestForceUsed;
    public Vector2 BestArrowPosition => _bestArrowPosition;
    public bool ShouldApplyMoreForce => _shouldApplyMoreForce;

    public float AdditonalForce = .2f;
    private float _additonalForceFactor = .2f;
    private void OnEnable()
    {
        Arrow.OnArrowStoppedAction += ManageArrow;
        Arrow.OnArrowHitCharacterAction += CheckIfHitPlayer;
    }

    private void ManageArrow( Arrow arrow )
    {
        if( arrow == null || arrow.Owner != this || _player == null ) return;

        if( _bestArrowPosition == Vector2.zero)  //If it's equal to zero then we know this is prolly the first arrow
        {
            _bestArrowPosition = arrow.transform.position;
            _bestForceUsed = arrow.ForceApplied;
            return;
        }

        Vector2 playerPos = _player.transform.position;
        Vector2 arrowPos = arrow.transform.position;

        float distance = ( playerPos - arrowPos ).magnitude; //Get the distance
        float previousDistance = ( playerPos - _bestArrowPosition ).magnitude;

        if( distance < previousDistance )
        {
            _bestArrowPosition = arrow.transform.position;           
            _bestForceUsed = arrow.ForceApplied;
        }

        //Check if we should reset our additional force factor
        bool isArrowBeforePlayer = ( arrow.transform.position.x > _player.transform.position.x );
        bool shouldReset = (_shouldApplyMoreForce != isArrowBeforePlayer);

        if( shouldReset )
        {
            AdditonalForce = _additonalForceFactor;
        }
        else
        {
            AdditonalForce += _additonalForceFactor;
        }

        //Finally check if the next shot should apply more force
        _shouldApplyMoreForce = isArrowBeforePlayer;

    }

    public override void Shoot( Vector2 dir )
    {
        if( arrow == null || !CanShoot ) return;

        //Create an offset for the arrow to spawn
        Vector2 center = transform.position;

        GameObject copy = Instantiate( arrow , (Vector2)transform.position + new Vector2( -1f, 2f), Quaternion.identity );
        Arrow newArrow = copy.GetComponent<Arrow>();
        if( newArrow == null ) return;

        newArrow.Owner = this;
        newArrow.ApplyForce( dir );

        CanShoot = false; //Set the flag
    }
    private void CheckIfHitPlayer(Arrow arrow)
    {
        if( arrow.Owner != this ) return;

        _hasHitPlayer = true;
    }

    public override void DrawLine( Vector2 startPosition, Vector2 currentDragPoint)
    {
        if( line == null ) return;

        line.enabled = true;
        line.SetPosition(0, startPosition); //Start point
        line.SetPosition(1, currentDragPoint); //End point
    }



    private void OnDisable()
    {
        Arrow.OnArrowStoppedAction -= ManageArrow;
        Arrow.OnArrowHitCharacterAction -= CheckIfHitPlayer;
    }

}