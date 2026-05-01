using Godot;

public partial class AnimationTreeData : Resource
{
	[Export] public StringName Name;
}

public partial class OneShotData : AnimationTreeData
{
	[Export] public AnimationNodeOneShot.OneShotRequest Request { get; set; }
	public string SettingPath => $"parameters/{Name}/request";
}

public partial class BlendSpace1D : AnimationTreeData
{
	public string SettingPath => $"parameters/{Name}/blend_position";
}
