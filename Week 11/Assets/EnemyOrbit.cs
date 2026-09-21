using System.Collections.Generic;
using UnityEngine;

public class EnemyOrbit : MonoBehaviour
{
    //Target ship
    public Transform player;
    //Orbit settings
    public float orbitRadius = 4f;
    public float radialGain = 1.5f;
    public int orbitDir = 1;
    //Speed settins
    public float minSpeed = 2f;
    public float maxSpeed = 6f;
    public float farDistance = 10f;
    //Turn settings
    public float turnSpeed = 270f;
    public float facingOffset = 0f;
    //Avoid settings
    public float avoidRadius = 1.6f;
    public float avoidStrength = 4f;
    //List of every enemy
    static readonly List<EnemyOrbit> all = new List<EnemyOrbit>();

    void Start()
    {
        //Random direction
        orbitDir = Random.value < 0.5f ? 1 : -1;
    }

    void Update()
    {
        //Get direction of player
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.z = 0f;
        //Distance and direction
        float sqrDist = toPlayer.sqrMagnitude;
        Vector3 inward = toPlayer.normalized;
        //Change speed based on distance
        float distance = Mathf.Clamp01(sqrDist / (farDistance * farDistance));
        float speed = Mathf.Lerp(minSpeed, maxSpeed, distance);
        //Move around player
        Vector3 tangent = new Vector3(-inward.y, inward.x, 0f) * orbitDir;
        float radiusError = Mathf.Clamp(toPlayer.magnitude - orbitRadius, -1f, 1f);
        Vector3 desired = tangent + inward * (radiusError * radialGain);
        if (desired.sqrMagnitude > 1f) desired = desired.normalized;
        //make enemies avoid player
        Vector3 avoid = Vector3.zero;
        float totalAvoid = avoidRadius * avoidRadius;
        for (int i = 0; i < all.Count; i++)
        {
            EnemyOrbit other = all[i];
            if (other == this) continue;
            //Distance between enemies
            Vector3 away = transform.position - other.transform.position;
            away.z = 0f;
            float s = away.sqrMagnitude;
            //Push enemies away
            if (s < totalAvoid && s > 0.0001f)
                avoid += away.normalized * (1f - s / totalAvoid);
        }
        //Orbit and avoid player
        Vector3 velocity = desired * speed + avoid * avoidStrength;
        transform.position += velocity * Time.deltaTime;
        //Look at player
        FaceDirection(inward);
    }

    void FaceDirection(Vector3 dir)
    {
        //Nothing to turn to
        if (dir.sqrMagnitude < 0.0001f) return;
        // Adjust direction
        float off = -facingOffset * Mathf.Deg2Rad;
        Vector3 target = new Vector3(
            dir.x * Mathf.Cos(off) - dir.y * Mathf.Sin(off),
            dir.x * Mathf.Sin(off) + dir.y * Mathf.Cos(off), 0f);

        //Current Direction
        Vector3 forward = transform.up;
        //Check accuracy of player to enemy rotation
        float dot = Vector3.Dot(forward, target);
        //Which way to turn
        float cross = Vector3.Cross(forward, target).z;
        //How far to turn
        float angle = Mathf.Atan2(cross, dot) * Mathf.Rad2Deg;
        //Make sure turn speed doesn't go further than max speed
        float maxStep = turnSpeed * Time.deltaTime;
        float step = Mathf.Clamp(angle, -maxStep, maxStep);
        //Turn by how much
        transform.Rotate(0f, 0f, step);
    }
}