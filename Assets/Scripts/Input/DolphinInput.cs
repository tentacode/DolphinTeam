using UnityEngine;

public class DolphinInput : MonoBehaviour
{
    static Vector2 downMousePosition;
    public static float deadZone = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            downMousePosition = Input.mousePosition;
        }
    }

    public static bool IsJumping()
    {
        return (Input.GetKeyDown(KeyCode.Space) || IsTap());
    }

    public static bool IsGoingLeft()
    {
        return (Input.GetKeyDown(KeyCode.LeftArrow) || IsSwipeLeft());
    }

    public static bool IsGoingRight()
    {
        return (Input.GetKeyDown(KeyCode.RightArrow) || IsSwipeRight());
    }

    static bool IsTap()
    {
        if (IsFingerUp() && Mathf.Abs(GetDeltaX()) <= deadZone)
        {
            return true;
        }   

        return false;
    }

    static bool IsSwipeLeft()
    {
        if (IsFingerUp() && GetDeltaX() < -deadZone)
        {
            return true;
        }   

        return false;
    }


    static bool IsSwipeRight()
    {
        if (IsFingerUp() && GetDeltaX() > deadZone)
        {
            return true;
        }   

        return false;
    }

    static bool IsFingerUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    static float GetDeltaX()    
    {       
        var downPositon = Camera.main.ScreenToWorldPoint(downMousePosition);
        var upPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return upPosition.x - downPositon.x;
    }
}
