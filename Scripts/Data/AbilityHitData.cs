using Godot;

public partial class AbilityHitData : RefCounted
{
    public string AbilityID { get; set; }
    public Node3D Target { get; set; }
    public Vector3 HitPosition { get; set; }
    public Vector3 HitNormal { get; set; }
    public float Power { get; set; }
    public BaseAbility SourceAbility { get; set; }
    public PackedScene HitSfxScene { get; set; }
    public PackedScene HitVfxScene { get; set; }
}
