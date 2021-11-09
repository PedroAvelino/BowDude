using TMPro;
using UnityEngine;

public class MagnitudeText : MonoBehaviour
{

    public TextMeshProUGUI Text;

    private Camera _cam;

    private void Awake()
    {
        _cam  = Camera.main;
    }
    public void SetPosition( Vector2 mousePos )
    {
        transform.position = _cam.WorldToScreenPoint( mousePos );
    }

}
