using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrapSequencer : MonoBehaviour
{
    [System.Serializable]
    public struct SequenceStep
    {
        [Tooltip("Time to wait BEFORE this action runs (relative to previous step).")]
        public float delay;
        public UnityEvent action;
    }

    [Header("Sequence Config")]
    [SerializeField] private List<SequenceStep> steps = new List<SequenceStep>();

    private bool isRunning = false;

    public void StartSequence()
    {
        if (isRunning) return;
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        isRunning = true;

        foreach (var step in steps)
        {
            if (step.delay > 0)
            {
                yield return new WaitForSeconds(step.delay);
            }
            step.action.Invoke();
        }

        isRunning = false;
    }
    
    // Helper to stop it if needed (e.g., player dies mid-sequence)
    public void StopSequence()
    {
        StopAllCoroutines();
        isRunning = false;
    }
}