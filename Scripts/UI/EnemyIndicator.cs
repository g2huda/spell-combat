using Godot;
using System;

public partial class EnemyIndicator : Control
{
	[Export] private float _screenPadding = 40f;
	private Camera3D _camera;
	private Node3D _targetEnemy;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_camera = GetViewport().GetCamera3D();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_targetEnemy == null || _camera == null)
			return;
		UpdateIndicatorPosition();
	}

	public void Init(Node3D enemy)
	{
		_targetEnemy = enemy;
	}

	private void UpdateIndicatorPosition()
	{
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		Vector2 screenCenter = viewportSize / 2f;

		Vector3 enemyPos = _targetEnemy.GlobalPosition;
		Vector2 screenPos = _camera.UnprojectPosition(enemyPos);
		bool isBehindCam = _camera.IsPositionBehind(enemyPos);

		Vector2 direction = isBehindCam ? 
			(screenCenter - screenPos).Normalized() : (screenPos - screenCenter).Normalized();

		Vector2 edgePosition = screenCenter + direction * 10000f;

		edgePosition.X = Mathf.Clamp(edgePosition.X, _screenPadding, viewportSize.X - _screenPadding);
		edgePosition.Y = Mathf.Clamp(edgePosition.Y, _screenPadding, viewportSize.Y - _screenPadding);
		GlobalPosition = edgePosition;
		Rotation = direction.Angle();
	}
}
