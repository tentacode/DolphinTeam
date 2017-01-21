using UnityEngine;

public enum RectTransformHidingMethod
{
	Auto,
	InactiveGameObject,
	DisabledCanvasComponent,
}

[System.Serializable]
public struct RectTransformConfig
{
	public Vector3 AnchoredPosition;
	public Vector2 SizeDelta;
	public Vector2 AnchorMin;
	public Vector2 AnchorMax;
	public Vector2 Pivot;
}

public static class RectTransformExtensions
{
	public static bool IsShown(this RectTransform rectTfm, RectTransformHidingMethod hidingMethod = RectTransformHidingMethod.InactiveGameObject)
	{
		Canvas canvas = null;
		switch (hidingMethod)
		{
			case RectTransformHidingMethod.Auto:
			case RectTransformHidingMethod.DisabledCanvasComponent:
				UnityEngine.Profiling.Profiler.BeginSample("RectTransformExtensions.IsShown GetComponent<Canvas>");
				canvas = rectTfm.GetComponent<Canvas>();
				UnityEngine.Profiling.Profiler.EndSample();
				break;
		}

		if (hidingMethod == RectTransformHidingMethod.Auto && canvas != null)
		{
			if (canvas != null)
			{
				hidingMethod = RectTransformHidingMethod.DisabledCanvasComponent;
			}
			else
			{
				hidingMethod = RectTransformHidingMethod.InactiveGameObject;
			}
		}

		switch (hidingMethod)
		{
			case RectTransformHidingMethod.DisabledCanvasComponent:
				return canvas.enabled;

			case RectTransformHidingMethod.InactiveGameObject:
			default:
				return rectTfm.gameObject.activeSelf;
		}
	}

	public static bool IsShownInHierarchy(this RectTransform rectTfm, RectTransformHidingMethod hidingMethod = RectTransformHidingMethod.InactiveGameObject)
	{
		switch (hidingMethod)
		{
			case RectTransformHidingMethod.DisabledCanvasComponent:
				return rectTfm.IsShown();

			case RectTransformHidingMethod.InactiveGameObject:
			default:
				return rectTfm.gameObject.activeInHierarchy;
		}
	}

	public static void Hide(this RectTransform rectTfm, bool raiseWarningIfAlreadyShown = false, RectTransformHidingMethod hidingMethod = RectTransformHidingMethod.InactiveGameObject)
	{
		if (!rectTfm.IsShown(hidingMethod))
		{
			if (raiseWarningIfAlreadyShown)
			{
				Debug.LogWarningFormat("{0} > Hide: already hidden!", rectTfm.name);
			}
			return;
		}

		Canvas canvas = null;
		switch (hidingMethod)
		{
			case RectTransformHidingMethod.Auto:
			case RectTransformHidingMethod.DisabledCanvasComponent:
				canvas = rectTfm.GetComponent<Canvas>();
				break;
		}

		if (hidingMethod == RectTransformHidingMethod.Auto && canvas != null)
		{
			if (canvas != null)
			{
				hidingMethod = RectTransformHidingMethod.DisabledCanvasComponent;
			}
			else
			{
				hidingMethod = RectTransformHidingMethod.InactiveGameObject;
			}
		}

		switch (hidingMethod)
		{
			case RectTransformHidingMethod.DisabledCanvasComponent:
				canvas.enabled = false;
				break;

			case RectTransformHidingMethod.InactiveGameObject:
			default:
				rectTfm.gameObject.SetActive(false);
				break;
		}

		rectTfm.gameObject.BroadcastMessage("OnUIHide", SendMessageOptions.DontRequireReceiver);
	}

	public static void Show(this RectTransform rectTfm, bool raiseWarningIfAlreadyShown = false, RectTransformHidingMethod hidingMethod = RectTransformHidingMethod.InactiveGameObject)
	{
		if (rectTfm.IsShown(hidingMethod))
		{
			if (raiseWarningIfAlreadyShown)
			{
				Debug.LogWarningFormat("{0} > Show: already shown!", rectTfm.name);
			}
			return;
		}

		Canvas canvas = null;
		switch (hidingMethod)
		{
			case RectTransformHidingMethod.Auto:
			case RectTransformHidingMethod.DisabledCanvasComponent:
				canvas = rectTfm.GetComponent<Canvas>();
				break;
		}

		if (hidingMethod == RectTransformHidingMethod.Auto && canvas != null)
		{
			if (canvas != null)
			{
				hidingMethod = RectTransformHidingMethod.DisabledCanvasComponent;
			}
			else
			{
				hidingMethod = RectTransformHidingMethod.InactiveGameObject;
			}
		}

		switch (hidingMethod)
		{
			case RectTransformHidingMethod.DisabledCanvasComponent:
				canvas.enabled = true;
				break;

			case RectTransformHidingMethod.InactiveGameObject:
			default:
				rectTfm.gameObject.SetActive(true);
				break;
		}

		rectTfm.gameObject.BroadcastMessage("OnUIShow", SendMessageOptions.DontRequireReceiver);
	}

	public static void SetVisibility(this RectTransform rectTfm, bool visibility, bool raiseWarningIfAlreadyShown = false, RectTransformHidingMethod hidingMethod = RectTransformHidingMethod.InactiveGameObject)
	{
		if (visibility)
		{
			rectTfm.Show(raiseWarningIfAlreadyShown, hidingMethod);
		}
		else
		{
			rectTfm.Hide(raiseWarningIfAlreadyShown, hidingMethod);
		}
	}

	public static bool ToggleVisibility(this RectTransform rectTfm, bool raiseWarningIfAlreadyShown = false, RectTransformHidingMethod hidingMethod = RectTransformHidingMethod.InactiveGameObject)
	{
		bool visibility = !rectTfm.IsShown(hidingMethod);

		rectTfm.SetVisibility(visibility, raiseWarningIfAlreadyShown, hidingMethod);

		return visibility;
	}

	public static Canvas GetParentCanvas(this RectTransform rectTfm)
	{
		UnityEngine.Profiling.Profiler.BeginSample("RectTransformExtensions.GetParentCanvasRectTfm 1");
		Canvas parentCanvas = rectTfm.GetComponentInParent<Canvas>();
		UnityEngine.Profiling.Profiler.EndSample();

		return parentCanvas;
	}

	public static void SetParentAndReset(this RectTransform rectTfm, Transform parentRectTfm)
	{
		rectTfm.SetParent(parentRectTfm);
		rectTfm.localPosition = Vector3.zero;
		rectTfm.localScale = Vector3.one;
	}

	public static void SetConfig(this RectTransform rectTfm, RectTransformConfig config)
	{
		rectTfm.anchoredPosition = config.AnchoredPosition;
		rectTfm.sizeDelta = config.SizeDelta;
		rectTfm.anchorMin = config.AnchorMin;
		rectTfm.anchorMax = config.AnchorMax;
		rectTfm.pivot = config.Pivot;
	}

	public static RectTransformConfig GetConfig(this RectTransform rectTfm)
	{
		RectTransformConfig config = new RectTransformConfig()
		{
			AnchoredPosition = rectTfm.anchoredPosition,
			SizeDelta = rectTfm.sizeDelta,
			AnchorMin = rectTfm.anchorMin,
			AnchorMax = rectTfm.anchorMax,
			Pivot = rectTfm.pivot,
		};

		return config;
	}
}
