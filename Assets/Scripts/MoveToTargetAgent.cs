using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveToTargetAgent : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer bgSpriteRenderer;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)target.localPosition);
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");

    }

    // refactor pending
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float movementSpeed = 5f;
        
        transform.localPosition += new Vector3(moveX, moveY, 0) * 
                                   movementSpeed * Time.deltaTime;

    }

    // refactor pending
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Target target))
        {
            AddReward(10f);
            bgSpriteRenderer.color = Color.green;
            EndEpisode();

        } else if (collision.TryGetComponent(out Wall wall))
        {
            AddReward(-2f);
            bgSpriteRenderer.color = Color.red;
            EndEpisode();

        }
        
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-3.5f, -1.5f), 
            Random.Range(-3.5f, 3.5f));
        target.localPosition = new Vector3(Random.Range(1.5f, 3.5f), 
            Random.Range(-3.5f, 3.5f));
        
    }

    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    
}
