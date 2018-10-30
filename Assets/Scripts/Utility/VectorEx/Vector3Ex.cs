using UnityEngine;

public static class Vector3Ex{
	public static bool IsAnyNan(this Vector3 vec)
	{
        if (float.IsNaN(vec.x) || float.IsNaN(vec.y) || float.IsNaN(vec.z))
		{
			return true;
		}
		return false;
    }
}
