using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JulianSchoenbaechler.SnapPanel
{
	public class RenderCamera
	{
		#region Variables

		protected GameObject _renderCamera;
		protected Camera _cameraComponent;
		protected Transform _focus;

		public string name { get; set; }

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Initializes a new <see cref="RenderCamera"/>.
		/// </summary>
		public RenderCamera()
		{
			_renderCamera = new GameObject("SnapPanel Camera");
			_renderCamera.transform.SetParent(transform.root);

			_cameraComponent = _renderCamera.AddComponent<Camera>();
			_cameraComponent.depth = Camera.main.depth - 1f;

			_renderCamera.SetActive(false);
		}

		/// <summary>
		/// Initializes a new <see cref="RenderCamera"/>.
		/// </summary>
		/// <param name="name">The name of the gameobject in hierarchy.</param>
		public RenderCamera(string name)
		{
			if(name == null)
				throw new ArgumentNullException("name");
			
			_renderCamera = new GameObject(name);
			_renderCamera.transform.SetParent(transform.root);

			_cameraComponent = _renderCamera.AddComponent<Camera>();
			_cameraComponent.depth = Camera.main.depth - 1f;

			_renderCamera.SetActive(false);
		}

		/// <summary>
		/// Initializes a new <see cref="RenderCamera"/>.
		/// </summary>
		/// <param name="name">The name of the gameobject in hierarchy.</param>
		/// <param name="depth">Camera's depth in the camera rendering order.</param>
		public RenderCamera(string name, float depth)
		{
			if(name == null)
				throw new ArgumentNullException("name");
			
			_renderCamera = new GameObject(name);
			_renderCamera.transform.SetParent(transform.root);

			_cameraComponent = _renderCamera.AddComponent<Camera>();
			_cameraComponent.depth = depth;

			_renderCamera.SetActive(false);
		}

		#endregion

		#region Statics

		/// <summary>
		/// Clones the RenderCamera original and returns the clone.
		/// </summary>
		/// <param name="original">Original.</param>
		public static RenderCamera Instantiate(RenderCamera original)
		{
			GameObject.Instantiate(original.gameObject);
			return original;
		}

		/// <summary>
		/// Clones the RenderCamera original and returns the clone.
		/// </summary>
		/// <param name="original">Original.</param>
		/// <param name="parent">Parent that will be assigned to the new object.</param>
		public static RenderCamera Instantiate(RenderCamera original, Transform parent)
		{
			GameObject.Instantiate(original.gameObject, parent);
			return original;
		}

		/// <summary>
		/// Clones the RenderCamera original and returns the clone.
		/// </summary>
		/// <param name="original">Original.</param>
		/// <param name="parent">Parent that will be assigned to the new object.</param>
		/// <param name="instantiateInWorldSpace">If set to <c>true</c> instantiate in world space.</param>
		public static RenderCamera Instantiate(RenderCamera original, Transform parent, bool instantiateInWorldSpace)
		{
			GameObject.Instantiate(original.gameObject, parent, instantiateInWorldSpace);
			return original;
		}

		/// <summary>
		/// Clones the RenderCamera original and returns the clone.
		/// </summary>
		/// <param name="original">Original.</param>
		/// <param name="position">Position for the new object.</param>
		/// <param name="rotation">Orientation of the new object.</param>
		public static RenderCamera Instantiate(RenderCamera original, Vector3 position, Quaternion rotation)
		{
			GameObject.Instantiate(original.gameObject, position, rotation);
			return original;
		}

		/// <summary>
		/// Clones the RenderCamera original and returns the clone.
		/// </summary>
		/// <param name="original">Original.</param>
		/// <param name="position">Position for the new object.</param>
		/// <param name="rotation">Orientation of the new object.</param>
		/// <param name="parent">Parent that will be assigned to the new object.</param>
		public static RenderCamera Instantiate(RenderCamera original, Vector3 position, Quaternion rotation, Transform parent)
		{
			GameObject.Instantiate(original.gameObject, position, rotation, parent);
			return original;
		}

		/// <summary>
		/// Destroys the specified RenderCamera.
		/// </summary>
		/// <param name="renderCamera">RenderCamera to destroy.</param>
		public static void Destroy(RenderCamera renderCamera)
		{
			GameObject.Destroy(renderCamera.gameObject);
		}

		/// <summary>
		/// Destroys the specified RenderCamera.
		/// </summary>
		/// <param name="renderCamera">RenderCamera to destroy.</param>
		/// <param name="t">The optional amount of time to delay before destroying the object.</param>
		public static void Destroy(RenderCamera renderCamera, float t)
		{
			GameObject.Destroy(renderCamera.gameObject, t);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the position of the camera in world space.
		/// </summary>
		/// <param name="position">Position.</param>
		public void SetPosition(Vector3 position)
		{
			_renderCamera.transform.position = position;
			_renderCamera.transform.LookAt(_focus);
		}

		/// <summary>
		/// Sets the position of the camera.
		/// </summary>
		/// <param name="position">Position.</param>
		public void SetLocalPosition(Vector3 position)
		{
			_renderCamera.transform.localPosition = position;
			_renderCamera.transform.LookAt(_focus);
		}

		/// <summary>
		/// Render the camera manually.
		/// </summary>
		public void Render()
		{
			_cameraComponent.Render();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Is the GameObject active in the scene?
		/// </summary>
		/// <value><c>true</c> if active in hierarchy; otherwise, <c>false</c>.</value>
		public bool activeInHierarchy
		{
			get { return _renderCamera.activeInHierarchy; }
		}

		/// <summary>
		/// The local active state of this GameObject. (Read Only)
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool activeSelf
		{
			get { return _renderCamera.activeSelf; }
		}

		/// <summary>
		/// Editor only API that specifies if a game object is static.
		/// </summary>
		/// <value><c>true</c>  if any of the static flags are set; otherwise, <c>false</c>. (See Also: <seealso cref="UnityEditor.GameObjectUtility.SetStaticEditorFlags">GameObjectUtility.SetStaticEditorFlags</seealso>)</value>
		public bool isStatic
		{
			get { return _renderCamera.isStatic; }
			set { _renderCamera.isStatic = value; }
		}

		/// <summary>
		/// The layer the game object is in. A layer is in the range [0...31].
		/// </summary>
		public int layer
		{
			get { return _renderCamera.layer; }
			set { _renderCamera.layer = value; }
		}

		/// <summary>
		/// Scene that the GameObject is part of.
		/// </summary>
		public UnityEngine.SceneManagement.Scene scene
		{
			get { return _renderCamera.scene; }
		}

		/// <summary>
		/// The tag of this game object.
		/// </summary>
		public string tag
		{
			get { return _renderCamera.tag; }
			set { _renderCamera.tag = value; }
		}

		/// <summary>
		/// The Transform attached to this GameObject.
		/// </summary>
		public Transform transform
		{
			get { return _renderCamera.transform; }
		}

		/// <summary>
		/// Camera's depth in the camera rendering order.
		/// </summary>
		public float depth
		{
			get { return _cameraComponent.depth; }
			set { _cameraComponent.depth = value; }
		}

		/// <summary>
		/// The game object the camera is attached to.
		/// </summary>
		public GameObject gameObject
		{
			get { return _renderCamera; }
		}

		/// <summary>
		/// Destination render texture.
		/// </summary>
		/// <value>The target texture.</value>
		public RenderTexture targetTexture
		{
			get { return _cameraComponent.targetTexture; }
			set { _cameraComponent.targetTexture = value; }
		}

		/// <summary>
		/// Gets or sets the focus point the camera is pointing at.
		/// </summary>
		public Transform focus
		{
			get { return _focus; }

			set
			{
				_focus = value;
				_renderCamera.transform.LookAt(_focus);
			}
		}

		#endregion
	}
}
