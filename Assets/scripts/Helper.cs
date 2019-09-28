using UnityEngine;
using System.Collections;

public enum MouseAxis
{
    x,
    y
}

public static class Helper
{
    public static float GetMouseAxis(MouseAxis axis)
    {
        float axisValue = 0f;
        if (axis == MouseAxis.x)
        {
            axisValue = Input.GetAxis("Mouse X");
        }
        else if (axis == MouseAxis.y)
        {
            axisValue = Input.GetAxis("Mouse Y");
        }

        return axisValue;
    }
}
