using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleHit : ShowCase
{

	[SerializeField] Camera cam;

	PhotonView  PV;
	Rigidbody RB;
	

	public float KnockBackX = 4f;
	public float KnockBackY = 3f;
	private void Awake()
    {
		PV = GetComponent<PhotonView>();
		RB = GetComponent<Rigidbody>();
	}

    public override void Use()
	{
		Debug.Log(itemInfo.itemName + " used");
		Shoot();
		
	}

	[PunRPC]

	void Shoot()
	{
		{
			if (PV.IsMine)
			{
				Vector3 mouseScreenPosition = Input.mousePosition;
				Ray ray = cam.ScreenPointToRay(mouseScreenPosition);
				ray.origin = cam.transform.position;
				if (Physics.Raycast(ray, out RaycastHit hit, 4.0f))
				{
					hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((ItemShowcase)itemInfo).damage);
					PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
                    hit.rigidbody.AddForce(transform.forward.normalized * KnockBackX, ForceMode.Impulse);
					hit.rigidbody.AddForce(transform.up.normalized * KnockBackY, ForceMode.Impulse);
					Debug.DrawRay(ray.origin, Vector3.forward, Color.red);
				}
			}
		}
	}



	[PunRPC]
	void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
	{
		
		Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
		if (colliders.Length != 0)
		{
			GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal
			* 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
		    Destroy(bulletImpactObj);

			Debug.Log("Knock");
				
		}
	}
}
