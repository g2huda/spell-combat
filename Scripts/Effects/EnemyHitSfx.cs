using Godot;

public partial class EnemyHitSfx : Node3D
{
	[Export] private AudioStreamPlayer3D _audioPlayer;

	public void Play(Vector3 position)
	{
		GlobalPosition = position;

		// Restart the sound cleanly
		_audioPlayer.Stop();
		_audioPlayer.Play();
	}
}
