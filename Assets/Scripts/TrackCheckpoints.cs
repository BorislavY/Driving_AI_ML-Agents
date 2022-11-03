using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;
    public event EventHandler OnPlayerLapCompleted;

    [SerializeField] private Transform agentsTransform;

    private List<Transform> carTransformList;
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    private void Awake() {
        carTransformList = new List<Transform>();
        foreach (Transform carTransform in agentsTransform)
        {
            carTransformList.Add(carTransform);
        }

        Transform checkpointsTransform = transform.Find("Checkpoints");
        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }
        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList) {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform) {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            // Correct checkpoint
            // Debug.Log("Correct");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            if ((nextCheckpointSingleIndex + 1) % checkpointSingleList.Count == 0) {
               // Debug.Log("Lap completed!");
               OnPlayerLapCompleted?.Invoke(this, EventArgs.Empty);

            }
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        } else {
            // Wrong checkpoint
            // Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            //CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            //correctCheckpointSingle.Show();
        }
    }

    public void ResetCheckpoint(Transform carTransform) {
        nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] = 0;
    }

    public CheckpointSingle GetNextCheckpoint(Transform carTransform) {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        CheckpointSingle nextCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
        return nextCheckpointSingle;
    }

}
