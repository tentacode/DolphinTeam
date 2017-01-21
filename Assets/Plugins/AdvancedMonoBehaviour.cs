using UnityEngine;

public class AdvancedMonoBehaviour : MonoBehaviour
{
	public Transform Tfm { get; private set; }

	protected virtual void Awake()
	{
		this.Tfm = this.GetComponent<Transform>();
	}
}
