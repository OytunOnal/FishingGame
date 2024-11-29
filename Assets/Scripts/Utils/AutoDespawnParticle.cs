using System.Collections;
using UnityEngine;

public class AutoDespawnParticle : MonoBehaviour
{
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while (true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);
			if (!ps.IsAlive(true))
			{
				PoolManager.Despawn(gameObject);
			}
		}
	}
}
