using Sandbox;
using System;

public class SnowBall : Prop
{
	public bool IsDown { get; set; }
	public MakeASnowmanPlayer Player { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		var rand = Rand.Float( 1 );
		var model = "models/christmas/snowball.vmdl";

		if ( rand < 0.1 )
		{
			model = "models/christmas/snowman.vmdl";
			Scale = Rand.Float( 2, 20 );
		}
		else if ( rand < 0.2 )
		{
			model = "models/christmas/gift.vmdl";
			Scale = Rand.Float( 5, 25 );
			RenderColor = Color.Random;
		}
		else if ( rand > 0.2 )
			Scale = Rand.Float( 20, 80 );

		SetModel( model );
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
	}

	[Event.Tick.Server]
	private void OnTick()
	{
		if ( !IsDown && Player.IsValid() )
		{
			Velocity = Vector3.Zero;
			Position = Player.Position + Vector3.Down * MakeASnowmanGame.OffsetDown;
		}
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{
		if ( PhysicsBody.BodyType == PhysicsBodyType.Static )
			return;

		if ( !IsDown )
		{
			if ( eventData.Entity == Player || ( eventData.Entity is SnowBall sball && sball.PhysicsBody.BodyType != PhysicsBodyType.Static ) ) return;

			Player.Position = Player.Position + Vector3.Up * 600;

			return;
		}

		if ( ( eventData.Entity is SnowBall ball && ball.IsDown ) || eventData.Entity is WorldEntity )
		{
			if ( MakeASnowmanGame.lastSnowBall.IsValid() && eventData.Entity is WorldEntity )
			{
				(Game.Current as MakeASnowmanGame).GameOver();
			}

			if ( eventData.Entity == Player || ( MakeASnowmanGame.lastSnowBall.IsValid() && eventData.Entity != MakeASnowmanGame.lastSnowBall ) ) return;

			PhysicsBody.BodyType = PhysicsBodyType.Static;
			MakeASnowmanGame.lastSnowBall = this;
		}
	}
}
