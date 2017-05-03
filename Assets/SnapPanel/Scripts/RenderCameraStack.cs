using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JulianSchoenbaechler.SnapPanel
{
	/// <summary>
	/// A stack-like constructs for storing <see cref="RenderCamera"/> objects.
	/// </summary>
	public class RenderCameraStack
	{
		protected List<RenderCamera> _mainStack;
		protected float _depth;

		public RenderCameraStack(float depth)
		{
			_mainStack = new List<RenderCamera>();
			_depth = depth;
		}

		public RenderCameraStack(float depth, int preAlloc)
		{
			_mainStack = new List<RenderCamera>(preAlloc);
			_depth = depth;
		}

		public RenderCamera GetRenderCamera(Vector3 position, Transform focus, Transform parent)
		{
			for(int i = 0; i < _mainStack.Count; i++)
			{
				if(_mainStack[i].activeSelf == false)
				{
					_mainStack[i].focus = focus;
					_mainStack[i].SetPosition(position);
					_mainStack[i].transform.SetParent(parent);
					_mainStack[i].gameObject.SetActive(true);
					return _mainStack[i];
				}
			}

			RenderCamera newRenderCam = new RenderCamera("SnapPanel Camera " + _mainStack.Count.ToString("D"), _depth);
			newRenderCam.focus = focus;
			newRenderCam.SetPosition(position);
			newRenderCam.transform.SetParent(parent);
			newRenderCam.gameObject.SetActive(true);

			_mainStack.Add(newRenderCam);

			return newRenderCam;
		}

		public void ReleaseRenderCamera(RenderCamera renderCam)
		{
			renderCam.gameObject.SetActive(false);
		}

		public void PreAllocRenderCameras(int count)
		{
			for(int i = 0; i < count; i++)
			{
				_mainStack.Add(new RenderCamera("SnapPanel Camera " + _mainStack.Count.ToString("D"), _depth));
			}
		}
	}
}
