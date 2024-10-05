using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectsOnPlane : MonoBehaviour
{
	public GameObject spawner;
	public GameObject shootPlane;

	[SerializeField]
	[Tooltip("Instantiates this prefab on a plane at the touch location.")]
	GameObject m_PlacedPrefab;

	/// <summary>
	/// The prefab to instantiate on touch.
	/// </summary>
	public GameObject placedPrefab
	{
		get { return m_PlacedPrefab; }
		set { m_PlacedPrefab = value; }
	}

	/// <summary>
	/// The object instantiated as a result of a successful raycast intersection with a plane.
	/// </summary>
	public GameObject spawnedObject { get; private set; }

	/// <summary>
	/// Invoked whenever an object is placed in on a plane.
	/// </summary>
	public static event Action onPlacedObject;

	ARRaycastManager m_RaycastManager;

	static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

	[SerializeField]
	int m_MaxNumberOfObjectsToPlace = 1;

	int m_NumberOfPlacedObjects = 0;

	[SerializeField]
	bool m_CanReposition = true;

	public bool canReposition
	{
		get => m_CanReposition;
		set => m_CanReposition = value;
	}

	void Awake()
	{
		m_RaycastManager = GetComponent<ARRaycastManager>();
	}

	void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
				{
					Pose hitPose = s_Hits[0].pose;

					if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
					{

						// Todo set missile position
						Spawner.Instance.setMissilePosition(hitPose.position);

						// Todo Spawn planes
						Spawner.Instance.spawnRedPlane();
						Spawner.Instance.spawnGreenPlane();
						Spawner.Instance.spawnRainbowPlane();

						// Create instance of the the prefab to place
						spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

						// Todo
						// add taret shoot script
						// shootPlane.GetComponent<Shoot>().missile = spawnedObject;

						m_NumberOfPlacedObjects++;


					}
					else
					{
						if (m_CanReposition)
						{
							spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
						}
					}

					if (onPlacedObject != null)
					{
						onPlacedObject();
					}
				}
			}
		}
	}
}
