using Sandbox;
using System;
using System.Linq;

public partial class MakeASnowmanPlayer : Prop
{
	private SnowBall snowBall;
	private TimeSince timeSincePos;
	private float Rate;

	[Net] public int SnowBallCount { get; set; }

	MakeASnowmanGame game;

	public MakeASnowmanPlayer()
	{
		Transmit = TransmitType.Always;
		Components.Add( new MakeASnowmanCamera() );
		game = Game.Current as MakeASnowmanGame;
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

		SnowBallCount = game.SnowBalls.Where( x => x.PhysicsBody.BodyType == PhysicsBodyType.Static ).Count();

		if ( Input.Pressed( InputButton.PrimaryAttack ) && !(Game.Current as MakeASnowmanGame).IsGameOver )
		{
			game.SnowBalls.Add( snowBall );

			snowBall.IsDown = true;
			snowBall = null;

			if ( SnowBallCount % 20 == 19 )
			{
				Rate += 0.5f;
				timeSincePos = 30;
			}
		}
	}
}
