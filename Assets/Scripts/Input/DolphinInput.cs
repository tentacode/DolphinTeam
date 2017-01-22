using UnityEngine;
using UnityEngine.EventSystems;

public class DolphinInput : MonoBehaviour
{
    static Vector2 downMousePosition;
    public static float deadZone = 0.5f;

	bool IsPointerOverUIElement()
	{
		if (EventSystem.current == null)
		{
			// Happens on Android since 5.3 :/
			return false;
		}

		if (Application.isMobilePlatform)
		{
			return EventSystem.current.IsPointerOverGameObject(0);
		}
		else
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}

	void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement()) {
            downMousePosition = Input.mousePosition;
        }
    }

    public static bool IsJumping()
    {
        return (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || IsTap());
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
