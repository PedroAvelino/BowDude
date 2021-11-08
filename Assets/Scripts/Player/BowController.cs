using Cinemachine;
using UnityEngine;

public class BowController : MonoBehaviour
{
    protected bool dragging;
    protected Vector2 startPoint = new Vector2( 1f, -.5f );
    protected Vector2 endPoint;

    protected Camera cam;

    [SerializeField] private CinemachineVirtualCamera _cnCam;
    [SerializeField] protected LineRenderer line;
    [SerializeField] protected GameObject arrow;

    public CinemachineVirtualCamera CNCam => _cnCam;

    public bool CanShoot;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        if( line != null )
        {
            line.positionCount = 2;
        }
    }

    public virtual void DrawLine( Vector2 startPosition, Vector2 currentDragPoint )
    {
        if( dragging == false || line == null ) return;

        line.enabled = true;
        line.SetPosition(0, startPosition); //Start point
        line.SetPosition(1, currentDragPoint); //End point
    }

    public void ResetLine()
    {
        if( line == null ) return;

        line.enabled = false;
        startPoint = new Vector2( 1f, -.5f );
        dragging = false;
    }

    public virtual void Shoot( Vector2 dir )
    {
        if( arrow == null || !CanShoot ) return;

        //Create an offset for the arrow to spawn
        Vector2 center = transform.position;
        Vector2 spawnPosition = (startPoint - center).normalized;

        GameObject copy = Instantiate( arrow , transform.position + (Vector3)spawnPosition, Quaternion.identity );
        Arrow newArrow = copy.GetComponent<Arrow>();
        if( newArrow == null ) return;

        newArrow.Owner = this;
        newArrow.ApplyForce( dir );

        CanShoot = false; //Set the flag
    }
}
