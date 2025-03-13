using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "LineOfSightCheckCondition", story: "Check [Target] With Line Of Sight [Detector]", category: "Conditions", id: "e69469c8c43a5a136d0c197758ccb884")]
public partial class LineOfSightCheckCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<LineOfSightDetector> Detector;


    public override bool IsTrue()
    {
        return Detector.Value != null && Target.Value != null && Detector.Value.PerformDetection(Target.Value) != null;
    }

    public override void OnStart() { }

    public override void OnEnd() { }
}

