using UnityEngine;

public static class TrajectoryCalculator
{
    /// <summary>
    /// Calculates a trajectory of a projectile taking into account air drag.
    /// </summary>
    /// <param name="initialPosition">The initial position of the projectile.</param>
    /// <param name="initialVelocity">The initial velocity of the projectile.</param>
    /// <param name="airDrag">The air drag coefficient.</param>
    /// <param name="steps">The number of steps to calculate the trajectory.</param>
    /// <param name="timeStep">The time step between each step.</param>
    /// <returns>An array of 3D vectors representing the position of the projectile at each step.</returns>
    public static Vector3[] CalculateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, float airDrag, int steps, float timeStep)
    {
        Vector3[] trajectoryPoints = new Vector3[steps];
        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;

        // Calculate the trajectory
        for (int i = 0; i < steps; i++)
        {
            // Store the current position
            trajectoryPoints[i] = position;

            // Calculate the new position
            position += velocity * timeStep;

            // Calculate the new velocity
            velocity += Physics.gravity * timeStep;

            // Apply air drag
            // Linearly interpolate the x and z velocities to zero
            velocity.x = Mathf.Lerp(velocity.x, 0, airDrag * timeStep);
            velocity.z = Mathf.Lerp(velocity.z, 0, airDrag * timeStep);
        }

        // Return the calculated trajectory
        return trajectoryPoints;
    }
}
