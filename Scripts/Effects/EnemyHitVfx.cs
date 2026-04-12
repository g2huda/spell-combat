using Godot;

public partial class EnemyHitVfx : Node3D
{
    [Export] private GpuParticles3D _particles;

    public void Play(Vector3 position, Vector3 normal)
    {
        GlobalPosition = position;

        // Optional: orient the node so the effect faces away from the hit surface.
        if(normal != Vector3.Zero)
        {
            LookAt(position + normal.Normalized(), Vector3.Up);
        }

        _particles.Restart();
        _particles.Emitting = true;

    }
}
