using Sandbox;
using System;

public partial class MakeASnowmanPlayer : Prop
{
	private SnowBall snowBall;
	private TimeSince timeSincePos;
	private float Rate;

	[Net] public int SnowBallCount { get; set; }

	public MakeASnowmanPlayer()
	{
		Transmit = TransmitType.Always;
		Camera = new MakeASnowmanCamera();
	}

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/christmas/snowman.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Static, false );
	}

	public virtual void ReSpawn()
	{
		Game.Current?.MoveToSpawnpoint( this );

		Position = Position + Vector3.Up * 1000;
		Scale = MakeASnowmanGame.PlayerScale;
		timeSincePos = 30;
		Rate = 1f;
		SnowBallCount = 0;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		var dir = Rotation.Left;
		var addv = 25f;

		Position += dir * MathF.Sin( timeSincePos * Rate ) * addv * Rate;

		if ( !IsServer )
			return;

		if ( !snowBall.IsValid() )
        {
			snowBall = new SnowBall()
			{
				Player = this,
				Position = Position + Vector3.Down * MakeASnowmanGame.OffsetDown
			};
        }

		if ( Input.Pressed( InputButton.Attack1 ) && !(Game.Current as MakeASnowmanGame).IsGameOver )
		{
			MakeASnowmanGame.snowBalls.Add( snowBall );

			snowBall.IsDown = true;
			snowBall = null;
			SnowBallCount++;

			if ( SnowBallCount % 20 == 19 )
			{
				Rate += 0.5f;
				timeSincePos = 30;
			}
		}
	}
}
