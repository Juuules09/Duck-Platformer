using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bumper))]
public class BumperEditor : Editor
{
    /// <summary>
    /// Scene GUI callback that draws a line along the trajectory of the bumper
    /// </summary>
    private void OnSceneGUI()
    {
        // Get the Bumper component
        Bumper bumper = (Bumper)target;

        // Initial position and velocity of the bumper
        Vector3 initialPosition = bumper.transform.position;
        Vector3 initialVelocity = bumper.transform.up * bumper.Force;

        // Calculate the trajectory points using the TrajectoryCalculator
        Vector3[] trajectoryPoints = TrajectoryCalculator.CalculateTrajectory(
            initialPosition,
            initialVelocity,
            0.25f,
            steps: 100,
            timeStep: 0.1f
        );

        // Draw the trajectory path as a series of connected lines
        Handles.color = Color.green;
        for (int i = 0; i < trajectoryPoints.Length - 1; i++)
        {
            Handles.DrawLine(trajectoryPoints[i], trajectoryPoints[i + 1]);
        }
    }
}
