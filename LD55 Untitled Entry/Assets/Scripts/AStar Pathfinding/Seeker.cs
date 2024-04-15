using System.Collections;
using UnityEngine;

public abstract class Seeker : MonoBehaviour
{
	[Header("Debug-only"), Space]
	[SerializeField] private bool seekOnStart;
	public Transform debugTarget;

	[Header("Movement Delta Squared"), Space]
	[SerializeField] protected float maxMovementDeltaSqr;

	// Private fields.
	protected Vector3[] _path;
	protected Coroutine _followCoroutine;
	
	protected int _waypointIndex;
	protected bool _finishedFollowingPath;

	private IEnumerator Start()
	{
		if (seekOnStart)
		{
			yield return new WaitForSeconds(.1f);
			PathRequester.Request(transform.position, debugTarget.position, OnPathFound);
		}
	}

	private void OnEnable()
	{
		// Resume the previous coroutine if it hasn't finished yet.
		if (!_finishedFollowingPath && _followCoroutine != null)
			_followCoroutine = StartCoroutine(FollowPath(_waypointIndex));
	}

	protected void OnPathFound(Vector3[] newPath, bool pathFound)
	{
		// Only start following the found path if this gameobject has not been destroyed yet.
		if (pathFound && gameObject != null)
		{
			_path = newPath;
			_waypointIndex = 0;

			if (_followCoroutine != null)
				StopCoroutine(_followCoroutine);
			
			_followCoroutine = StartCoroutine(FollowPath());
		}
	}

	protected abstract IEnumerator FollowPath(int previousIndex = -1);

	private void OnDrawGizmos()
	{
		if (_path != null)
		{	
			for (int i = _waypointIndex; i < _path.Length; i++)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawCube(_path[i], Vector3.one);

				Gizmos.color = Color.black;
				if (i == _waypointIndex)
					Gizmos.DrawLine(transform.position, _path[i]);
				else
					Gizmos.DrawLine(_path[i - 1], _path[i]);
			}
		}
	}
}
