using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidField : MonoBehaviour 
{	
	public Asteroid.PolyCount	polyCount = Asteroid.PolyCount.HIGH;
	public Asteroid.PolyCount	polyCountCollider = Asteroid.PolyCount.LOW;	

	public Transform[]			prefabAsteroids;
	
	public Material[]			materialVeryCommon;		// 50%
	public Material[]			materialCommon;			// 30%
	public Material[]			materialRare;				// 15%
	public Material[]			materialVeryRare;			// 5%

	public float				range = 20000.0f;
	public int					maxAsteroids;	
	public bool					respawnDestroyedAsteroids = true;
	public bool					respawnIfOutOfRange = true;
	public float				distanceSpawn = 0.95f;		
	public float				minAsteroidScale = 0.1f;
	public float				maxAsteroidScale = 1.0f;	
	public float				scaleMultiplier = 1.0f;
		
	public float				minAsteroidRotationSpeed = 0.0f;
	public float				maxAsteroidRotationSpeed = 1.0f;
	public float				rotationSpeedMultiplier = 1.0f;
	public float				minAsteroidDriftSpeed = 0.0f;
	public float				maxAsteroidDriftSpeed = 1.0f;
	public float				driftSpeedMultiplier = 1.0f;
	
	public bool					isRigidbody = false;
	public float				mass = 1.0f;
	public float				minAsteroidAngularVelocity = 0.0f;
	public float				maxAsteroidAngularVelocity = 1.0f;
	public float				angularVelocityMultiplier = 1.0f;
	public float				minAsteroidVelocity = 0.0f;
	public float				maxAsteroidVelocity = 1.0f;	
	public float				velocityMultiplier = 1.0f;	
	public bool					findurigidebody;
	
	public bool					fadeAsteroids = true;
	public float				distanceFade = 0.95f;
	
	private float				_distanceToSpawn;	
	private float				_distanceToFade;
	private Transform			_cacheTransform;	
	private List<Transform>		_asteroids = new List<Transform>();
	private List<Material>		_asteroidsMaterials = new List<Material>();
	private List<bool>			_asteroidsFading = new List<bool>();
	private Hashtable			_materialsTransparent = new Hashtable();
	private SortedList<int, string>	_materialList = new SortedList<int, string>(4);
	
	
	void OnEnable ()
	{
		_cacheTransform = transform;			
		_distanceToSpawn = range * distanceSpawn;
		_distanceToFade = range * distanceSpawn * distanceFade;
		
		if (fadeAsteroids && _materialsTransparent.Count == 0)
		{	
			CreateTransparentMaterial(materialVeryCommon, _materialsTransparent);
			CreateTransparentMaterial(materialCommon, _materialsTransparent);
			CreateTransparentMaterial(materialRare, _materialsTransparent);
			CreateTransparentMaterial(materialVeryRare, _materialsTransparent);
		}
		if (_materialList.Count == 0) 
		{
			if (materialVeryRare.Length > 0)
				_materialList.Add(5, "VeryRare");
			if (materialRare.Length > 0)
				_materialList.Add(5 + 15, "Rare");
			if (materialCommon.Length > 0)
				_materialList.Add(5 + 15 + 30, "Common");				
			if (materialVeryCommon.Length != 0)
				_materialList.Add(5 + 15 + 30 + 50, "VeryCommon");			
			else
				Debug.LogError("Asteroid Field must have at least one Material in the 'Material Very Common' Array."); 		
		}
		for (int i = 0; i < _asteroids.Count; i++) 
			_asteroids[i].gameObject.SetActive(true);
		SpawnAsteroids(false);
	}
	
	void OnDisable () {
		for (int i = 0; i < _asteroids.Count; i++) 
		{
			if (_asteroids[i] != null) 
				_asteroids[i].gameObject.SetActive(false);
		}
	}		
	
	void OnDrawGizmosSelected () 
	{
	    Gizmos.color = Color.yellow;
	    Gizmos.DrawWireSphere (transform.position, range);
	}	
	
	void Update () 
	{
		for (int i = 0; i < _asteroids.Count; i++) 
		{
			Transform _asteroid = _asteroids[i];			
			if (_asteroid != null) 
			{				
				float _distance = Vector3.Distance(_asteroid.position, _cacheTransform.position);
				
				if (_distance > range && respawnIfOutOfRange) 
				{
					_asteroid.position = Random.onUnitSphere * _distanceToSpawn + _cacheTransform.position;
					float _newScale = Random.Range(minAsteroidScale,maxAsteroidScale) * scaleMultiplier;
					_asteroid.localScale = new Vector3(_newScale, _newScale, _newScale);
					Vector3 _newRotation = new Vector3(Random.Range(0,360), Random.Range(0,360), Random.Range(0,360));					
					_asteroid.eulerAngles = _newRotation;
				}				
				if (fadeAsteroids) 
				{
					if (_distance > _distanceToFade) 
					{
						if (!_asteroidsFading[i]) 
						{
							_asteroid.renderer.sharedMaterial = (Material) _materialsTransparent[_asteroidsMaterials[i]];
							_asteroidsFading[i] = true;
						}		
						Color _col = _asteroid.renderer.material.color;
						float _alpha = Mathf.Clamp01(1.0f - ((_distance - _distanceToFade) / (_distanceToSpawn - _distanceToFade)));
						_asteroid.renderer.material.color = new Color(_col.r, _col.g, _col.b, _alpha );
					}
					else 
					{
						if (_asteroidsFading[i]) 
						{					
							_asteroid.renderer.material = (Material) _asteroidsMaterials[i];
							_asteroidsFading[i] = false;
						}
					}
				}
			} 
			else 
			{
				_asteroids.RemoveAt(i);
				_asteroidsMaterials.RemoveAt(i);
				_asteroidsFading.RemoveAt(i);
			}
			if (respawnDestroyedAsteroids && _asteroids.Count < maxAsteroids) 
				SpawnAsteroids(true);
		}			
	}
	
	void SpawnAsteroids(bool atSpawnDistance) 
	{
		while (_asteroids.Count < maxAsteroids) 
		{
			Transform _newAsteroidPrefab = prefabAsteroids[Random.Range(0, prefabAsteroids.Length)];
			
			Vector3 _newPosition = Vector3.zero;			
			if (atSpawnDistance)
				_newPosition = _cacheTransform.position + Random.onUnitSphere * _distanceToSpawn;
			else
				_newPosition = _cacheTransform.position + Random.insideUnitSphere * _distanceToSpawn;
			Transform _newAsteroid = (Transform) Network.Instantiate(_newAsteroidPrefab, _newPosition, _cacheTransform.rotation, 0);
			//Transform _newAsteroid = (Transform) Instantiate(_newAsteroidPrefab, _newPosition, _cacheTransform.rotation);			
			switch (WeightedRandom(_materialList)) 
			{
				case "VeryCommon":
					_newAsteroid.renderer.sharedMaterial = materialVeryCommon[Random.Range(0, materialVeryCommon.Length)];
				break;
				case "Common":
					_newAsteroid.renderer.sharedMaterial = materialCommon[Random.Range(0, materialCommon.Length)];
				break;
				case "Rare":
					_newAsteroid.renderer.sharedMaterial= materialRare[Random.Range(0, materialRare.Length)];
				break;
				case "VeryRare":
					_newAsteroid.renderer.sharedMaterial = materialVeryRare[Random.Range(0, materialVeryRare.Length)];
				break;
			}
			_asteroids.Add(_newAsteroid);
			_asteroidsMaterials.Add(_newAsteroid.renderer.sharedMaterial);
			_asteroidsFading.Add(false);
			if (_newAsteroid.GetComponent<Asteroid>() != null) 
			{
				_newAsteroid.GetComponent<Asteroid>().SetPolyCount(polyCount);
				if (_newAsteroid.collider != null) 
					_newAsteroid.GetComponent<Asteroid>().SetPolyCount(polyCountCollider, true);
			}
			float _newScale = Random.Range(minAsteroidScale,maxAsteroidScale) * scaleMultiplier;
			_newAsteroid.localScale = new Vector3(_newScale, _newScale, _newScale);
			_newAsteroid.eulerAngles = new Vector3(Random.Range(0,360), Random.Range(0,360), Random.Range(0,360));		
			if (isRigidbody)
			{
				if (_newAsteroid.rigidbody != null) 
				{
					_newAsteroid.rigidbody.mass = mass * _newScale;
					_newAsteroid.rigidbody.velocity = _newAsteroid.transform.forward * Random.Range(minAsteroidVelocity, maxAsteroidVelocity) * velocityMultiplier;
					_newAsteroid.rigidbody.angularVelocity = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f)) * Random.Range(minAsteroidAngularVelocity, maxAsteroidAngularVelocity) * angularVelocityMultiplier;
				}
				else
					Debug.LogWarning("AsteroidField is set to spawn rigidbody asterodids but one or more asteroid prefabs do not have rigidbody component attached.");
			} 
			else 
			{
				
				if (_newAsteroid.rigidbody != null) 
				{
					Destroy(_newAsteroid.rigidbody);
				}
				if (_newAsteroid.GetComponent<Asteroid>() != null) 
				{
					_newAsteroid.GetComponent<Asteroid>().rotationSpeed = Random.Range(minAsteroidRotationSpeed, maxAsteroidRotationSpeed);
					_newAsteroid.GetComponent<Asteroid>().rotationalAxis = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
					_newAsteroid.GetComponent<Asteroid>().driftSpeed = Random.Range(minAsteroidDriftSpeed, maxAsteroidDriftSpeed);
					_newAsteroid.GetComponent<Asteroid>().driftAxis = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
				}
				
			}
		}	
		
	}
	
	void CreateTransparentMaterial(Material[] _sourceMaterials, Hashtable _ht) 
	{
		foreach (Material _mat in _sourceMaterials) 
		{
			_ht.Add(_mat, new Material(Shader.Find("Transparent/Diffuse")));
			((Material) _ht[_mat]).SetTexture("_MainTex", _mat.GetTexture("_MainTex"));		
			((Material) _ht[_mat]).color = _mat.color;
		}
	}
	
	static T WeightedRandom<T>(SortedList<int, T> _list)
	{
		int _max = _list.Keys[_list.Keys.Count-1];
		int _random = Random.Range(0, _max);
		foreach (int _key in _list.Keys) 
		{
			if (_random <= _key) 
				return _list[_key];
		}
   		return default(T);
	}
}
