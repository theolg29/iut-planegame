using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{

	public enum TurretType
	{
		Single = 1,
		Dual = 2,
		Catapult = 3,
	}

	public GameObject currentTarget;
	public Transform turreyHead;

	public float attackDist = 10.0f;
	public float attackDamage;
	public float shootCoolDown;
	private float timer;
	public float loockSpeed;

	//public Quaternion randomRot;
	public Vector3 randomRot;
	public Animator animator;

	[Header("[Turret Type]")]
	public TurretType turretType = TurretType.Single;

	public Transform muzzleMain;
	public Transform muzzleSub;
	public GameObject muzzleEff;
	public GameObject bullet;
	private bool shootLeft = true;

	private Transform lockOnPos;

	//public TurretShoot_Base shotScript;

	void Start()
	{
		InvokeRepeating("CheckForTarget", 0, 0.5f);
		//shotScript = GetComponent<TurretShoot_Base>();

		if (transform.GetChild(0).GetComponent<Animator>())
		{
			animator = transform.GetChild(0).GetComponent<Animator>();
		}

		// Todo init rotate
	}

	void Update()
	{
		if (currentTarget != null)
		{
			// Todo 
			// call follow target here
			this.FollowTarget();

			float currentTargetDist = Vector3.Distance(transform.position, currentTarget.transform.position);
			if (currentTargetDist > attackDist)
			{
				currentTarget = null;
			}
		}
		else
		{
			// Todo 
			// idle rotate 
			IdleRotate();
		}

		timer += Time.deltaTime;
		if (timer >= shootCoolDown)
		{
			if (currentTarget != null)
			{
				timer = 0;

				if (animator != null)
				{
					animator.SetTrigger("Fire");
					// ShootTrigger();
				}
				else
				{
					// ShootTrigger();
				}
			}
		}
	}



	private void CheckForTarget()
	{
		Collider[] colls = Physics.OverlapSphere(transform.position, attackDist);
		float distAway = Mathf.Infinity;

		for (int i = 0; i < colls.Length; i++)
		{
			// Todo  "GreenPlane" || "RainbowPlane" as well
			if (colls[i].tag == "RedPlane")
			{
				float dist = Vector3.Distance(transform.position, colls[i].transform.position);
				if (dist < distAway)
				{
					currentTarget = colls[i].gameObject;
					distAway = dist;
				}
			}
		}
	}

	private void FollowTarget() //todo : smooth rotate
	{
		Vector3 targetDir = currentTarget.transform.position - turreyHead.position;
		targetDir.y = 0;
		//turreyHead.forward = targetDir;
		if (turretType == TurretType.Single)
		{
			turreyHead.forward = targetDir;
		}
		else
		{
			turreyHead.transform.rotation = Quaternion.RotateTowards(turreyHead.rotation, Quaternion.LookRotation(targetDir), loockSpeed * Time.deltaTime);
		}
	}

	private void ShootTrigger()
	{
		//shotScript.Shoot(currentTarget);
		Shoot(currentTarget);
		//Debug.Log("We shoot some stuff!");
	}

	public void InitRotate()
	{
		randomRot = new Vector3(0, Random.Range(0, 359), 0);
	}

	public void IdleRotate()
	{
		bool refreshRandom = false;

		if (turreyHead.rotation != Quaternion.Euler(randomRot))
		{
			turreyHead.rotation = Quaternion.RotateTowards(turreyHead.transform.rotation, Quaternion.Euler(randomRot), loockSpeed * Time.deltaTime * 0.2f);
		}
		else
		{
			refreshRandom = true;

			if (refreshRandom)
			{
				int randomAngle = Random.Range(0, 359);
				randomRot = new Vector3(0, randomAngle, 0);
				refreshRandom = false;
			}
		}
	}


	Vector3 CalculateVelocity(Vector3 target, Vector3 origen, float time)
	{
		Vector3 distance = target - origen;
		Vector3 distanceXZ = distance;
		distanceXZ.y = 0;

		float Sy = distance.y;
		float Sxz = distanceXZ.magnitude;

		float Vxz = Sxz / time;
		float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

		Vector3 result = distanceXZ.normalized;
		result *= Vxz;
		result.y = Vy;

		return result;
	}

	public void Shoot(GameObject go)
	{
		if (turretType == TurretType.Catapult)
		{
			lockOnPos = go.transform;

			Instantiate(muzzleEff, muzzleMain.transform.position, muzzleMain.rotation);
			GameObject missleGo = Instantiate(bullet, muzzleMain.transform.position, muzzleMain.rotation);
			Projectile projectile = missleGo.GetComponent<Projectile>();
			projectile.target = lockOnPos;
		}
		else if (turretType == TurretType.Dual)
		{
			if (shootLeft)
			{
				Instantiate(muzzleEff, muzzleMain.transform.position, muzzleMain.rotation);
				GameObject missleGo = Instantiate(bullet, muzzleMain.transform.position, muzzleMain.rotation);
				Projectile projectile = missleGo.GetComponent<Projectile>();
				projectile.target = transform.GetComponent<TurretAI>().currentTarget.transform;
			}
			else
			{
				Instantiate(muzzleEff, muzzleSub.transform.position, muzzleSub.rotation);
				GameObject missleGo = Instantiate(bullet, muzzleSub.transform.position, muzzleSub.rotation);
				Projectile projectile = missleGo.GetComponent<Projectile>();
				projectile.target = transform.GetComponent<TurretAI>().currentTarget.transform;
			}

			shootLeft = !shootLeft;
		}
		else
		{
			Instantiate(muzzleEff, muzzleMain.transform.position, muzzleMain.rotation);
			GameObject missleGo = Instantiate(bullet, muzzleMain.transform.position, muzzleMain.rotation);
			Projectile projectile = missleGo.GetComponent<Projectile>();
			projectile.target = currentTarget.transform;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackDist);
	}

}
