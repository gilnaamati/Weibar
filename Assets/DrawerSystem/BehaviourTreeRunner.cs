using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{

    BehaviourTree tree;

    // Start is called before the first frame update
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();
        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();

        log1.message = " Heelllooo111";

        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();

        log2.message = " Heelllooo222";

        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();

        log3.message = " Heelllooo33";

        var pause1 = ScriptableObject.CreateInstance<WaitNode>();
        pause1.duration = 1f;

        var pause2= ScriptableObject.CreateInstance<WaitNode>();
        pause2.duration = 1f;

        var pause3 = ScriptableObject.CreateInstance<WaitNode>();
        pause3.duration = 1f;

        var seq = ScriptableObject.CreateInstance<SequencerNode>();
        seq.children.Add(log1);
        seq.children.Add(pause1);
        seq.children.Add(log2);
        seq.children.Add(pause2);
        seq.children.Add(log3);
        seq.children.Add(pause3);

        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.child = seq;
        tree.rootNode = loop;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
