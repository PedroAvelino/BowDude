using UnityEngine;
using TMPro;

public class AngleText : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private Camera _cam;

    private void Awake()
    {
        _cam  = Camera.main;
    }
    public void SetStartPosition( Vector2 mousePos )
    {
        transform.position = _cam.WorldToScreenPoint( mousePos );
    }

}
