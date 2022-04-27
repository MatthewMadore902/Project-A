using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
	public GameObject bullet;

	public float shootForce, upwardForce;
	public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;

	public int bulletDamage;
	public int magSize, bulletsPerTap;
	public int bulletsLeft, bulletsShot;
	public bool allowTriggerHold, shooting, readyToShoot, reloading;

	public Camera Camera;
	public Transform attackPoint;

	//Recoil
	public Rigidbody playerRb;
	public float recoilForce;

	//Graphics
	public GameObject muzzleFlash;
	public TextMeshProUGUI ammouCount;

	//for bug fixing
	public bool allowInvoke = true;

	public void Awake()
	{
		//Mag size - make sure its full
		bulletsLeft = magSize;
		readyToShoot = true;

	}
	public void Update()
	{
		UserInput();

		// Ammo counter display
		if (ammouCount != null)
		{
			ammouCount.SetText(bulletsLeft / bulletsPerTap + " / " + magSize);
		}
	}
	public void UserInput()
	{
		if (allowTriggerHold)
		{
			shooting = Input.GetKey(KeyCode.Mouse0);
		}
		else
		{
			shooting = Input.GetKeyDown(KeyCode.Mouse0);
		}

		//reloading manually 
		if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
		{
			Reload();
		}
		//reloading automatically when user tries to shoot with empty mag
		if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
		{
			Reload();
		}

		//shooting
		if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
		{
			bulletsShot = 0;
			Shoot();
		}
	}
	public void Shoot()
	{
		readyToShoot = true;
		Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		Vector3 targetPoint;
		if (Physics.Raycast(ray, out hit))
		{
			targetPoint = hit.point;
		}
		else
		{
			targetPoint = ray.GetPoint(75);
		}

		Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

		float x = Random.Range(-spread, spread);
		float y = Random.Range(-spread, spread);

		Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

		GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

		//rotates bullets to shooting direction
		currentBullet.transform.forward = directionWithSpread.normalized;

		//adds froce to bullets
		currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
		currentBullet.GetComponent<Rigidbody>().AddForce(Camera.transform.up * upwardForce, ForceMode.Impulse);

		//Muzzle flash
		if (muzzleFlash != null)
		{
			Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
		}

		bulletsShot++;
		bulletsLeft--;

		if (allowInvoke)
		{
			Invoke("ResetShot", timeBetweenShooting);
			allowInvoke = false;

			//Recoil
			playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
		}

		if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
		{
			Invoke("Shoot", timeBetweenShooting);
		}
		if (gameObject.CompareTag("Bullet"))
		{

			bulletDamage = 20;
		}
	}

	public void ResetShot()
	{
		readyToShoot = true;
		allowInvoke = true;
	}
	public void Reload()
	{
		reloading = true;
		Invoke("ReloadFinished", reloadTime);
	}
	public void ReloadFinished()
	{
		bulletsLeft = magSize;
		reloading = false;
	}
}
