using System;
using UnityEngine;
public class ShootPosition : MonoBehaviour
{
    [SerializeField]
    private BowController _character;
    Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        MoveShootPosition();
    }

    private void MoveShootPosition()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pivot = _character.transform.position;

        Vector2 dir = (mousePos - pivot);
    }
}