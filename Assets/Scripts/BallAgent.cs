using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TensorFlow;
public class BallAgent : Agent
{

    public GameObject ball;
    private Rigidbody ballRb;
    public override void InitializeAgent()
    {
        ballRb = ball.GetComponent<Rigidbody>();
    }
    public override void CollectObservations()
    {
        AddVectorObs(ball.transform.rotation.x);
        AddVectorObs(ball.transform.rotation.z);
        AddVectorObs((ball.transform.position - gameObject.transform.position));
        AddVectorObs(ballRb.velocity);
    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            var actionX = Mathf.Clamp(vectorAction[0], -1f, 1f);
            var actionZ = Mathf.Clamp(vectorAction[1], -1f, 1f);
            if ((gameObject.transform.rotation.z > -10f && actionZ > 0f) || (gameObject.transform.rotation.z < 10f && actionZ < 0f))
                gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);
            if ((gameObject.transform.rotation.x > -10f && actionX > 0f) || (gameObject.transform.rotation.x < 10f && actionX < 0f))
                gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);
        }
       
        if ((gameObject.transform.position.y - ball.transform.position.y) > 2f)
        {
            Done();
            SetReward(-1f);
        }
        else
        {
            SetReward(0.1f);
        }
    }
    public override void AgentReset()
    {
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), Random.Range(-10f, 10f));
        gameObject.transform.Rotate(new Vector3(1f, 0f, 0f), Random.Range(-10f, 10f));
        ballRb.velocity = new Vector3(0f, 0f, 0f);
        ball.transform.position = new Vector3(Random.Range(-2f, 2f), 3f, Random.Range(-2f, 2f));

    }
}
