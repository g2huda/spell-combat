using Godot;

public partial class PlayerAnimationController : Node
{
	[Signal] public delegate void OnSpellReadyEventHandler();
	[Signal] public delegate void OnSpellCastCompleteEventHandler();

	[Export] private AnimationTree _animationTree;
	[Export] private OneShotData _attackOneShotData = 
		new OneShotData { Name = "Attack", Request = AnimationNodeOneShot.OneShotRequest.Fire };
	[Export] private BlendSpace1D _moveBlendData = 
		new BlendSpace1D { Name = "Move" };

	private bool _isAttacking = false;
	public void TriggerAttack()
	{
		if (_isAttacking) return; // Prevent overlapping attacks
		_animationTree.Set(_attackOneShotData.SettingPath, (int)_attackOneShotData.Request);
		_isAttacking = true;
	}

	public void TriggerSpellReady()
	{
		EmitSignal(SignalName.OnSpellReady);
	}

	public void TriggerSpellCastComplete()
	{
		EmitSignal(SignalName.OnSpellCastComplete);
		_isAttacking = false;
	}

	public void SetMoveSpeed(float speed)
	{
		_animationTree.Set(_moveBlendData.SettingPath, speed);
	}
}
