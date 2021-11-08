using UnityEngine;

public class Player : BowController
{
    private void Update()
    {
        ManageInput();

        DrawLine(startPoint, endPoint);
    }

    private void ManageInput()
    {
        if (Input.GetMouseButtonDown(0) && CanShoot)
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }

        if (dragging && CanShoot)
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && CanShoot)
        {
            Vector2 dir = (startPoint - endPoint);
            Shoot(dir);
            ResetLine();
        }
    }
}