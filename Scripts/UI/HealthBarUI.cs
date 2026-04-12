using Godot;

public partial class HealthBarUI : Control
{

	[Export] ColorRect Fill;

	private float _intialFillWidth;

	public override void _Ready()
	{
		_intialFillWidth = Fill.Size.X;
		base._Ready();
	}

	public void SetHealthPercent(float percent)
	{
		percent = Mathf.Clamp(percent, 0f, 1f);
		Vector2 size = Fill.Size;
		size.X = _intialFillWidth * percent;
		Fill.Size = size;
	}
}
