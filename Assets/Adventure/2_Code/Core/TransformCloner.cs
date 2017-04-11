using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JulianSchoenbaechler.Core
{
	/// <summary>
	/// Used to clone basic information of Transform components.
	/// </summary>
	public class TransformClone
	{
		public string tag { get; set; }
		public Vector3 position { get; set; }
		public Quaternion rotation { get; set; }
		public Vector3 scale { get; set; }

		public bool CompareTag(string tag)
		{
			return tag.Equals(this.tag);
		}
	}


	/// <summary>
	/// Transform clone extension class.
	/// </summary>
	public static class TransformCloner
	{
		/// <summary>
		/// Clones a Transform component into a <see cref="TransformClone"/> object.
		/// </summary>
		/// <returns>The clone.</returns>
		/// <param name="transform">The transform to clone.</param>
		public static TransformClone CloneTransform(this Transform transform)
		{
			TransformClone clone = new TransformClone();

			clone.tag = transform.tag;

			clone.position = new Vector3(
				transform.position.x,
				transform.position.y,
				transform.position.z
			);

			clone.rotation = new Quaternion(
				transform.rotation.x,
				transform.rotation.y,
				transform.rotation.z,
				transform.rotation.w
			);

			clone.scale = new Vector3(
				transform.localScale.x,
				transform.localScale.y,
				transform.localScale.z
			);

			return clone;
		}

		/// <summary>
		/// Clones multiple Transform components into <see cref="TransformClone"/> objects.
		/// </summary>
		/// <returns>The clones.</returns>
		/// <param name="transform">The transforms to clone.</param>
		public static TransformClone[] CloneTransform(this Transform[] transforms)
		{
			TransformClone[] clones = new TransformClone[transforms.Length];

			for(int i = 0; i < transforms.Length; i++)
			{
				clones[i] = transforms[i].CloneTransform();
			}

			return clones;
		}

		/// <summary>
		/// Applies the transform from a clone.
		/// </summary>
		/// <param name="clone">The clone.</param>
		/// <param name="transform">The transform the clone should be applied to.</param>
		public static void ApplyTransform(this TransformClone clone, Transform transform)
		{
			transform.position = clone.position;
			transform.rotation = clone.rotation;
			transform.localScale = clone.scale;
		}
	}
}
