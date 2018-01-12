using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAgent : Agent {
    public float scale;
    int a_idx;

    /*
      Add vertices to their own layer so they don't interact with each other. If that doesn't work make thier charge very small compared to attractor
      Link action to changing charge of the attractors
      Change their color when their charge changes
      Add child objects for attractor charge scripts
     */
    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        return state;
    }

    public override void AgentStep(float[] act)
    {

    }
   
    public override void AgentReset()
    {
    }
}
