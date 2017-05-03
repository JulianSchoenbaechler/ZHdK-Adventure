using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure.UI
{
	public class SnapPanelSize : MonoBehaviour
	{
		[SerializeField] protected float _borderSize = 10f;

		protected virtual void Start()
		{
			RectTransform rectTransform = GetComponent<RectTransform>();
			Vector2 size = rectTransform.sizeDelta;
			float maxWidth = (float)Screen.width / 3f;
			float maxHeight = (float)Screen.height / 3f;

			size.x = maxWidth - 2f * _borderSize;
			size.y = maxHeight - _borderSize;

			rectTransform.sizeDelta = size;
		}
	}
}
