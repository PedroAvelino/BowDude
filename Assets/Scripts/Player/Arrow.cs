using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _forceMulitplier = 1f;
    [SerializeField] private SpriteRenderer _sprite;
    private Rigidbody2D _rb;
    public BowController Owner;

    public float ForceApplied;
    private bool _updatingRotation = true;

    public static Action<Arrow> OnArrowCreatedAction;
    public static Action<Arrow> OnArrowStoppedAction;
    public static Action<Arrow> OnArrowHitCharacterAction;
    
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if( _updatingRotation )
        {
            transform.up = _rb.velocity;
        }
    }

    public void ApplyForce( Vector2 dir )
    {
        transform.up = dir;
        _rb.velocity = dir * _forceMulitplier;
        ForceApplied = dir.magnitude;

        OnArrowCreatedAction?.Invoke( this );
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        _updatingRotation = false;
        _rb.velocity = Vector2.zero;
        _rb.isKinematic = true;
        OnArrowStoppedAction?.Invoke( this );

        if( other.gameObject.GetComponentInParent<BowController>() )
        {
            OnArrowHitCharacterAction?.Invoke( this );
        }

        var charCol = other.gameObject.GetComponent<CharacterCollider>();
        if( charCol == null ) return;
        
        charCol.OnDamageTaken?.Invoke( charCol.BodyStatus.damageTaken );
    }
}