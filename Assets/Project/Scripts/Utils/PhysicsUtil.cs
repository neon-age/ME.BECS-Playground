using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtil
{
    public static void ApplyDrag(ref Vector3 velocity, float linearDrag, float quadraticDrag, float deltaTime)
    {
        var velMag = velocity.magnitude;

        if (quadraticDrag != 0)
            velocity -= (velMag * velMag) * quadraticDrag * deltaTime * velocity.normalized;

        if (linearDrag != 0)
            velocity *= (1 - deltaTime * linearDrag);
    }

    // https://gamedev.stackexchange.com/questions/182850/rotate-rigidbody-to-face-away-from-camera-with-addtorque
    public static Vector3 GetTorqueTowardsRotation(Vector3 angularVelocity, Quaternion fromRot, Quaternion targetRot, float spring, float damper, float deltaTime)
    {
        // Compute the change in orientation we need to impart.
        var rotationChange = targetRot * Quaternion.Inverse(fromRot);

        // Convert to an angle-axis representation, with angle in range -180...180
        rotationChange.ToAngleAxis(out var angle, out var axis);
        if (angle > 180f)
            angle -= 360f;

        // If we're already facing the right way, just stop.
        // This avoids problems with the infinite axes ToAngleAxis gives us in this case.
        if (Mathf.Approximately(angle, 0))
        {
            angularVelocity = Vector3.zero;
            return default;
        }

        // If you need to, you can enforce a cap here on the maximum rotation you'll
        // allow in a single step, to prevent overly jerky movement from upsetting your sim.
        // angle = Mathf.Clamp(angle, -90f, 90f);

        // Convert to radians.
        angle *= Mathf.Deg2Rad;

        // Compute an angular velocity that will bring us to the target orientation
        // in a single time step.
        var targetAngularVelocity = axis * angle / deltaTime;

        // You can reduce this parameter to smooth the movement over multiple time steps,
        // to help reduce the effect of sudden jerks.
        float catchUp = 1.0f;
        targetAngularVelocity *= catchUp;

        return axis * (angle * spring) - (angularVelocity * damper);
        //return targetAngularVelocity - angularVelocity;
    }
}
