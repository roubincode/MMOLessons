using System;
using UnityEngine;
using Mirror;
public abstract class Movement : NetworkBehaviour
{
    public abstract Vector3 GetVelocity();

    public abstract bool IsMoving();

    public abstract void SetSpeed(float speed);

    public abstract void LookAtY(Vector3 position);

    // reset all movement. just stop and stand.
    public abstract void Reset();

    public abstract void Warp(Vector3 destination);

    public abstract bool CanNavigate();

    // navigate along a path to a destination
    public abstract void Navigate(Vector3 destination, float stoppingDistance);

    public abstract bool IsValidSpawnPoint(Vector3 position);

    public abstract Vector3 NearestValidDestination(Vector3 destination);

    public abstract bool DoCombatLookAt();
}
