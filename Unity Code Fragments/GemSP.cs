using UnityEngine;
using System.Collections;

public class GemSP : MonoBehaviour
{
	//public GameObject	gemPrefab;
	public float		maxGems;	// Stops generating gems when there are these many gems nearby
	public float		interval;	// Frequency at which gems are generated
	public int			radius;		// How far the gems at generated around the spawnpoint

	public float		timer;		// Timer to count towards the interval
	public float		currGems;	// Current amount 

	bool startspawn;
	PhotonView pv;

	void Start()
	{
		timer = 0f;
		currGems = 0;
		GetComponent<Renderer>().enabled = false;
		startspawn = false;
		pv = GetComponent<PhotonView> ().photonView;
	}

	public void StartLevel()
	{
		startspawn = true;
	}

	void Update()
	{
		if(startspawn)
		{
			// Counter to generate gems
			if(currGems < maxGems)
			{
				timer += Time.deltaTime;
				if(timer >= interval)
				{
					GenerateGem();
					timer = 0f;
				}
			}
			else
			{
				timer = 0f;
			}
		}
	}

	void GenerateGem()
	{
		GameObject go = PhotonNetwork.Instantiate ("Gem", 
		                                           new Vector3 (transform.position.x + Random.Range(-radius, radius) * 0.1f,
		                                                        transform.position.y,
		             											transform.position.z + Random.Range(-radius, radius) * 0.1f),
		                                           Quaternion.identity, 0);
		go.transform.parent = transform;
		currGems++;
	}
}
