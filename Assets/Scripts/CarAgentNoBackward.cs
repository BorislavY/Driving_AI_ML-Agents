using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgentNoBackward : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnTransform;

    private Vector3 spawnPosition;
    private Vector3 spawnForward;
    private PrometeoCarControllerNoBackward carDriver;

    private void Awake() {
        carDriver = GetComponent<PrometeoCarControllerNoBackward>();
        spawnPosition = spawnTransform.position;
        spawnForward = spawnTransform.forward;
    }

private void Start()
    {
        trackCheckpoints.OnPlayerCorrectCheckpoint += TrackCheckpoints_OnPlayerCorrectCheckpoint;
        trackCheckpoints.OnPlayerWrongCheckpoint += TrackCheckpoints_OnPlayerWrongCheckpoint;
        trackCheckpoints.OnPlayerLapCompleted += TrackCheckpoints_OnPlayerLapCompleted;
    }

    private void TrackCheckpoints_OnPlayerWrongCheckpoint(object sender, System.EventArgs e)
    {
        AddReward(-0.1f);
    }

    private void TrackCheckpoints_OnPlayerCorrectCheckpoint(object sender, System.EventArgs e)
    {
        AddReward(0.1f);
    }

    private void TrackCheckpoints_OnPlayerLapCompleted(object sender, System.EventArgs e)
    {
        AddReward(3f);
        EndEpisode();
    }

    public override void OnEpisodeBegin() {
        transform.position = spawnPosition + new Vector3(Random.Range(-10f, +10f), 0, Random.Range(-12f, +12f));
        transform.forward = spawnForward;
        trackCheckpoints.ResetCheckpoint(transform);
        carDriver.StopCompletely();
    }

    public override void CollectObservations(VectorSensor sensor) {
        CheckpointSingle nextCheckpoint = trackCheckpoints.GetNextCheckpoint(transform);

        float directionDot = Vector3.Dot(transform.forward, nextCheckpoint.transform.forward);
        sensor.AddObservation(directionDot);

        Vector3 difference = nextCheckpoint.transform.position - transform.position;
        sensor.AddObservation(difference / 10f); // Somewhat normalize

        // Small penalty on each time step to incentivise faster driving.
        AddReward(-0.0001f);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        int forward = 0;
        int left = 0;
        int right = 0;

        switch (actions.DiscreteActions[0]) {
            case 1: forward = 1; break;
        }
        switch (actions.DiscreteActions[1]) {
            case 1: left = 1; break;
            case 2: right = 1; break;
        }

        carDriver.SetInputs(forward, left, right);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.W)) forwardAction = 1;
        if (Input.GetKey(KeyCode.S)) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.A)) turnAction = 1;
        if (Input.GetKey(KeyCode.D)) turnAction = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall)) {
            AddReward(-0.05f);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall)) {
            AddReward(-0.001f);
        }

    }
}
