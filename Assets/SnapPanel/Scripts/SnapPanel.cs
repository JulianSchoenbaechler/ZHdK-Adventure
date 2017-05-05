using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JulianSchoenbaechler.SnapPanel
{
	public class SnapPanel : MonoBehaviour
	{
		#region Variables

		public enum PanelType { Static, Realtime, LastInRealtime }

		// Serialized
		[SerializeField] protected GameObject[] _panels;
		[SerializeField] protected GameObject[] _cameraPositions;
		[SerializeField] protected Transform _focus;
		[SerializeField] protected bool _chooseRandom = false;
		[SerializeField] private float _snapDelay = 0.2f;
		[SerializeField] private float _panelLifetime = 2f;
		[SerializeField] private bool _mutualLifetime = false;

		// Public
		public PanelType panelType;

		// Private
		private Camera _renderCamera;
		private RenderCameraStack _renderCameraStack;
		private WaitForSeconds _coroutineDelay;
		private WaitForSeconds _lifetimeDelay;
		private WaitUntil _sequenceFinishDelay;

		#endregion

		#region Unity Behaviour

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		private void Awake()
		{
			// Disable panels
			for(int i = 0;i < _panels.Length;i++)
			{
				_panels[i].SetActive(false);
			}

			SequenceFinished = true;
		}

		/// <summary>
		/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		/// </summary>
		private void Start()
		{
			_coroutineDelay = new WaitForSeconds(_snapDelay);
			_lifetimeDelay = new WaitForSeconds(_panelLifetime);
			_sequenceFinishDelay = new WaitUntil(() => SequenceFinished);

			// Prealloc new RenderCamera stack
			_renderCameraStack = new RenderCameraStack(Camera.main.depth - 1f, _panels.Length);
			ShufflePositionsIndex();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Start snapping process: Comic panel rendering.
		/// </summary>
		public void StartSnapping()
		{
			if(SequenceFinished)
			{
				SequenceFinished = false;
				StartCoroutine(SnapRoutine());
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Creates a randomized sequence of indices used to shuffle the camera positions array.
		/// </summary>
		/// <returns>A randomized indices sequence for <see cref="_cameraPositions"/>.</returns>
		protected int[] ShufflePositionsIndex()
		{
			int[] i = new int[_panels.Length];

			// Prefill array
			for(int j = 0; j < _panels.Length; j++)
			{
				if(j < _cameraPositions.Length)
					i[j] = j;
				else
					i[j] = Random.Range(0, _cameraPositions.Length);
			}
			
			// Knuth shuffle algorithm
			for(int t = 0; t < i.Length; t++)
			{
				int tmp = i[t];
				int r = Random.Range(t, i.Length);
				i[t] = i[r];
				i[r] = tmp;
			}

			return i;
		}

		/// <summary>
		/// Create static comic panel. Invoke this Method as a coroutine.
		/// </summary>
		/// <param name="panel">Comic panel object.</param>
		/// <param name="size">Panel size.</param>
		/// <param name="camera">RenderCamera to use.</param>
		protected IEnumerator Snapshot(GameObject panel, Vector2 size, RenderCamera camera)
		{
			RenderTexture rendered = RenderTexture.GetTemporary((int)size.x, (int)size.y, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 2);

			// Setup camera
			camera.targetTexture = rendered;


			// Setup render path and render camera view
			RenderTexture currentRT = RenderTexture.active;
			RenderTexture.active = camera.targetTexture;

			camera.Render();

			// Read pixels from render pipeline and save it in a 2D texture
			Texture2D content = new Texture2D((int)size.x, (int)size.y);
			content.ReadPixels(new Rect(Vector2.zero, size), 0, 0);
			content.Apply();

			// Reset render path and release temporary render texture
			RenderTexture.active = currentRT;
			camera.targetTexture = null;
			RenderTexture.ReleaseTemporary(rendered);
			_renderCameraStack.ReleaseRenderCamera(camera);

			// Display GUI panel
			panel.GetComponentInChildren<RawImage>(true).texture = content as Texture;
			panel.SetActive(true);

			// Delay before cleanup
			if(_mutualLifetime)
				yield return _sequenceFinishDelay;
			else
				yield return _lifetimeDelay;
			

			// Cleanup panel after its lifetime
			panel.GetComponentInChildren<RawImage>(false).texture = null;
			panel.SetActive(false);
		}

		/// <summary>
		/// Create realtime comic panel. Invoke this Method as a coroutine.
		/// </summary>
		/// <param name="panel">Comic panel object.</param>
		/// <param name="size">Panel size.</param>
		/// <param name="camera">RenderCamera to use.</param>
		protected IEnumerator Realtime(GameObject panel, Vector2 size, RenderCamera camera)
		{
			RenderTexture rendered = RenderTexture.GetTemporary((int)size.x, (int)size.y, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 2);

			// Setup camera
			camera.targetTexture = rendered;

			// Display GUI panel
			panel.GetComponentInChildren<RawImage>(true).texture = rendered as Texture;
			panel.SetActive(true);

			// Delay before cleanup
			if(_mutualLifetime)
				yield return _sequenceFinishDelay;
			else
				yield return _lifetimeDelay;

			// Cleanup panel after its lifetime
			panel.GetComponentInChildren<RawImage>(false).texture = null;
			camera.targetTexture = null;
			RenderTexture.ReleaseTemporary(rendered);
			panel.SetActive(false);
			_renderCameraStack.ReleaseRenderCamera(camera);
		}

		/// <summary>
		/// Start snapping process. Invoke this Method as a coroutine.
		/// </summary>
		/// <returns>The routine.</returns>
		protected IEnumerator SnapRoutine()
		{
			Vector2 panelSize;
			Vector3 cameraPos;
			RenderCamera camera;
			int[] randomPositionIndices = ShufflePositionsIndex();

			// Iterate through all panels
			for(int i = 0; i < _panels.Length; i++)
			{
				panelSize = _panels[i].GetComponent<RectTransform>().sizeDelta;

				// Randomize camera position?
				if(_chooseRandom)
					cameraPos = _cameraPositions[randomPositionIndices[i]].transform.position;
				else
					cameraPos = _cameraPositions[i].transform.position;

				// Resolve panel type
				switch(panelType)
				{
					// Static panel
					case PanelType.Static:

						camera = _renderCameraStack.GetRenderCamera(cameraPos, _focus,  transform);
						StartCoroutine(Snapshot(_panels[i], panelSize, camera));

						break;

					
					// Realtime panel
					case PanelType.Realtime:

						camera = _renderCameraStack.GetRenderCamera(cameraPos, _focus,  transform);
						StartCoroutine(Realtime(_panels[i], panelSize, camera));

						break;


					// All static, last realtime
					case PanelType.LastInRealtime:

						camera = _renderCameraStack.GetRenderCamera(cameraPos, _focus, transform);

						// Last panel?
						if(i >= (_panels.Length - 1))
						{
							StartCoroutine(Realtime(_panels[i], panelSize, camera));
						}
						else
						{
							StartCoroutine(Snapshot(_panels[i], panelSize, camera));
						}

						break;

					// Unresolved case
					default:
						break;
				}

				yield return _coroutineDelay;
			}

			// Release sequence
			yield return _lifetimeDelay;
			SequenceFinished = true;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Indicating whether this instance is choosing random camera positions.
		/// </summary>
		/// <value><c>true</c> if choose random position; otherwise, <c>false</c>.</value>
		public bool ChooseRandom
		{
			get { return _chooseRandom; }
			set { _chooseRandom = value; }
		}

		/// <summary>
		/// Indicating whether this instances snapping sequence finished.
		/// </summary>
		/// <value><c>true</c> if sequence finished; otherwise, <c>false</c>.</value>
		public bool SequenceFinished { get; private set; }

		#endregion
	}
}
