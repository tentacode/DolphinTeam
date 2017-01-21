using UnityEngine;
using UnityEngine.Assertions;

// This script is used to preserve the "pixel perfect" style in 2D pixel art games.
// We want to scale the pixels to fit in a "Visible Area" corresponding to the scene we want to display,
// for example our "Visible area" could be 256px width and 512px height. The camera will always show at least
// this amount of pixels (but can display more).
// It will try to scale the pixels up while keeping the visible area, well, visible.
public class PixelPerfectCamera : MonoBehaviour
{
    public Vector2 visibleArea;

    // Should be set to the "pixel per unit" import setting of your textures.
    public int pixelsPerUnit = 16;

    private int lastScreenWidth;
    private int lastScreenHeight;

    private int scaling;

    void Start()
    {
        Assert.IsFalse(visibleArea.x == 0 || visibleArea.y == 0, "Visible area vector should be set with values > 0.");

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        ComputeOrthographicSize();
    }

    void Update()
    {
        // Screen size has changed
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight) {
            ComputeOrthographicSize();
            return;
        }
    }

    void ComputeOrthographicSize()
    {
        ComputeScaling();

        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Screen.height * 0.5f / (scaling * pixelsPerUnit);
    }

    void ComputeScaling()
    {
        float screenRatio = Screen.width / Screen.height;
        float visibleAreaRatio = visibleArea.x / visibleArea.y;

        // Screen is wider than visible area. We scale the height.
        if (screenRatio > visibleAreaRatio)
        {
            scaling = Mathf.FloorToInt(Screen.height / visibleArea.y);
        }
        // Screen is upper than visible area.  We scale the width.
        else
        {
            scaling = Mathf.FloorToInt(Screen.width / visibleArea.x);
        }

        if (scaling <= 0)
        {
            scaling = 1;
        }
    }
}