using Godot;
using Godot.Collections;

public partial class AnimationTreeData : Resource
{
	[Export] public StringName NodeName;

	protected readonly StringName _parametersPath = "parameters";
	public virtual StringName Path => NodeName;
	public virtual StringName FullPath => $"{_parametersPath}/{Path}";
}

public partial class OneShotData : AnimationTreeData
{
	[Export] public AnimationNodeOneShot.OneShotRequest Request { get; set; }
	public override StringName Path => $"{NodeName}/request";
}

public partial class BlendSpace1DData : AnimationTreeData
{
	public override StringName Path => $"{NodeName}/blend_position";
}

public partial class StateMachineData : AnimationTreeData
{
	[Export] public Array<AnimationTreeData> States;
	public StringName PlaybackPath => $"{_parametersPath}/{NodeName}/playback";
	public string GetStatePath(int index) => $"{FullPath}/{States[index].Path}";
}
