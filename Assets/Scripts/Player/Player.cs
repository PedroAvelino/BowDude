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

            //Deal with texts here
            if( magnitudeText != null )
            {
                magnitudeText.gameObject.SetActive(true);
            }

            if( angleText != null )
            {
                angleText.gameObject.SetActive(true);
                angleText.SetStartPosition( startPoint );
            }
        }

        if (dragging && CanShoot)
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);


            //Deal with texts here
            if( magnitudeText != null )
            {
                magnitudeText.SetPosition( endPoint );
                magnitudeText.Text.text = $"{(startPoint - endPoint).magnitude.ToString("F2")}º";
            }

            if( angleText != null )
            {
                float degrees = Vector2.Angle( Vector2.left, (endPoint - startPoint) );
                angleText.Text.text = $"{degrees.ToString("F2")}";
            }

        }

        if (Input.GetMouseButtonUp(0) && CanShoot)
        {
            Vector2 dir = (startPoint - endPoint);
            Shoot(dir);
            ResetLine();

            //Text
            if( magnitudeText != null )
            {
                magnitudeText.gameObject.SetActive(false);
            }

            if( angleText != null )
            {
                angleText.gameObject.SetActive( false );
            }
        }
    }
}