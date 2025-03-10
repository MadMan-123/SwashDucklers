using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaunchManager : MonoBehaviour
{
    public static LaunchManager instance;
    [SerializeField] private float heightModifier = 0.75f;
    [SerializeField] private float durationModifer = 0.99f;
    [SerializeField] static float ragdollTime = 4.5f;
   public static AudioSource splash;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public static Vector3 CalculateVelocity(Vector3 target, Vector3 start, float launchDuration,float maxHeight)
    {
        var acceleration = Physics.gravity.y;
        var delta = target - start;
        var distance = new Vector3(delta.x,0,delta.z).magnitude;
        var horizontal = new Vector3(delta.x, 0, delta.z).normalized * (distance / launchDuration); 
        var yVel = (1.75f * maxHeight / launchDuration) - (0.5f * acceleration * launchDuration);
         
        return (horizontal + Vector3.up * yVel);
    }

    public static Vector3 LaunchObject(GameObject obj, Vector3 target,float extraForce, float launchDuration)
    {   
        Vector3 vel;
        if (obj.TryGetComponent(out Rigidbody rb))
        {
            var height = target.y - obj.transform.position.y;
            vel = (CalculateVelocity(target,obj.transform.position, launchDuration, height));
            rb.AddForce(vel * extraForce, ForceMode.VelocityChange);
            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<Health>().SetHealth(0);
                obj.GetComponent<PlayerControler>().Ragdoll(ragdollTime,false);
                splash.Play();
            }
            return vel;
        }

        return Vector3.zero;
    }
    
    public static List<Vector3> DrawTrajectory(Vector3 startPos, Vector3 velocity, float duration)
    {
        List<Vector3> points = new List<Vector3>();
        int trajectoryPoints = 25;
        float timeStep = duration / trajectoryPoints;
        Vector3 previousPoint = startPos;

        for (int i = 1; i <= trajectoryPoints; i++)
        {
            float time = timeStep * i;
            Vector3 point = startPos + velocity * time;
            point.y += Physics.gravity.y * time * time * 0.5f;
            points.Add(point);
            Debug.DrawRay(previousPoint, point - previousPoint, Color.red, duration);
            previousPoint = point;
            
        }

        return points;
    } 
    public static List<Vector3> GetTrajectoryPoints(Vector3 startPos, Vector3 velocity, float duration, int pointCount = 25)
    {
        List<Vector3> points = new List<Vector3>();
        float timeStep = duration / pointCount;
        
        for (int i = 0; i <= pointCount; i++)
        {
            float time = timeStep * i;
            var point = startPos + velocity * time;
            point.y += Physics.gravity.y * time * time * 0.5f;
            points.Add(point);
        }
        
        return points;
    }

    public static IEnumerator MoveAlongTrajectory(GameObject obj, List<Vector3> points, float duration, bool clamp = true)
    {
        float elapsedTime = 0;
        int lastPointIndex = points.Count - 1;
        while (elapsedTime < duration)
        {
            float normalizedTime = elapsedTime / duration;
            float curvePoint = normalizedTime * lastPointIndex;
            int indexA = Mathf.FloorToInt(curvePoint);
            int indexB = Mathf.Min(indexA + 1, lastPointIndex);
            float t = curvePoint - indexA;
            
            // Lerp between points
            obj.transform.position = Vector3.Lerp(points[indexA], points[indexB], t);
            
            //ensure we dont clip through anything
            //if there is a floor below us we should stop
            if (clamp)
            {
                if (NavMesh.SamplePosition(obj.transform.position + Vector3.down, out var hit2, 0.75f,NavMesh.AllAreas))
                {
                    obj.transform.position = hit2.position;
                    //tell the object to stop moving
                    elapsedTime = duration;
                    //obj.GetComponent<PlayerControler>().ToggleCamera(true);
                }
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        

        //ensure last position is met

        if(NavMesh.SamplePosition(points[^1], out var hit, 2f,NavMesh.AllAreas))
        {
            obj.transform.position = hit.position;
        }

        
        //reset velocity
        if (obj.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
        }
        obj.GetComponent<PlayerControler>().ToggleCamera(true);
    }

    public Vector3 LaunchObjectWithVar(GameObject obj, Vector3 target, float duration = 1f)
    {
        var height = (target.y - obj.transform.position.y) * heightModifier;
 
        Vector3 velocity = CalculateVelocity(target, obj.transform.position, duration, height);
        List<Vector3> trajectoryPoints = GetTrajectoryPoints(obj.transform.position, velocity, duration * durationModifer);
        
        // Start the coroutine
        StartCoroutine(MoveAlongTrajectory(obj, trajectoryPoints, duration));

        return velocity;
    } 
}
