using Godot;
using Godot.Collections;
public partial class EnemyAnimationController : Node
{
	[Signal] public delegate void OnDeathAnimationCompleteEventHandler();

	[Export] private AnimationTree _animationTree;
	[Export] private StringName StateMachineNodeName = "StateMachine";
	[Export] private StringName MovementBlend1DName = "Movement";
	[Export] private StringName DeathAnimationName = "Death";

	private StateMachineData _stateMachineData => 
		new StateMachineData { 
			NodeName = StateMachineNodeName, 
			States = new Array<AnimationTreeData> {
				new BlendSpace1DData{ NodeName = MovementBlend1DName },
				new AnimationTreeData { NodeName = DeathAnimationName } 
			} 
		};

	private AnimationNodeStateMachinePlayback _playback =>
		(AnimationNodeStateMachinePlayback)_animationTree.Get(_stateMachineData.PlaybackPath);

	public void SetMoveSpeed(float speed)
	{
		_animationTree.Set(_stateMachineData.GetStatePath(0), speed);
	}

	public void DeathAnimationFinished()
	{
		EmitSignal(SignalName.OnDeathAnimationComplete);
	}

	public void Die()
	{
		_playback.Travel(_stateMachineData.States[1].NodeName);
	}
}
