using Godot;

public partial class PlayerAnimationController : Node
{
	[Signal] public delegate void OnSpellReadyEventHandler();

	[Export] private AnimationTree _animationTree;
	[Export] private OneShotData _attackOneShotData = 
		new OneShotData { Name = "Attack", Request = AnimationNodeOneShot.OneShotRequest.Fire };
	[Export] private BlendSpace1D _moveBlendData = 
		new BlendSpace1D { Name = "Move" };

	public void TriggerAttack()
	{
		_animationTree.Set(_attackOneShotData.SettingPath, (int)_attackOneShotData.Request);
	}

	public void TriggerSpellReady()
	{
		EmitSignal(SignalName.OnSpellReady);
	}

	public void SetMoveSpeed(float speed)
	{
		_animationTree.Set(_moveBlendData.SettingPath, speed);
	}
}
