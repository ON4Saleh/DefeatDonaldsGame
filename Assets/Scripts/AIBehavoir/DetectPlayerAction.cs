using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectPlayer", story: "Update Range New Target [Detect] and Assign [Target]", category: "Action", id: "d083c34d974d2fce8ee0b414e61b0485")]
public partial class DetectPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<RangeDetector> Detect;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        Target.Value = Detect.Value.UpdateDetector();
        return Target.Value == null ? Status.Failure : Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

