#include "PhysicsObject.h"

vector<float> velocity = { 0, 0 };
vector<float> acceleration = { 0, 0 };

vector<float> position = { 8, 9 };
vector<float> direction = { 0, 0 };

float maxVel = 10;

void PhysicsObject::ApplyGravity(vector<float> force)
{
	acceleration[0] += force[0];
	acceleration[1] += force[1];
}

void PhysicsObject::ApplyForce(vector<float> force)
{
	acceleration[0] += force[0] / mass;
	acceleration[1] += force[1] / mass;
}

void PhysicsObject::ApplyFriction(float coeff)
{
	vector<float> friction = { -velocity[0], -velocity[1] };

	float fricMag = abs(sqrtf(friction[0] * friction[0] + friction[1] * friction[1]));

	if (fricMag != 0)
		friction = { friction[0] / fricMag, friction[1] / fricMag };

	friction = { friction[0] * coeff, friction[1] * coeff };

	ApplyForce(friction);
}

void PhysicsObject::FlipVelocity(bool x, bool y)
{
	velocity = { x ? -velocity[0] : velocity[0], y ? -velocity[1] : velocity[1] };
}

vector<float> PhysicsObject::Update()
{
	if (useGravity) ApplyGravity(vector<float> {0, -.009800f});
	if (useFriction) ApplyFriction(frictionCoeff);

	float deltaTime = GlobalVariables::GetInstance()->getDeltaTime();

	velocity = { 
		velocity[0] + acceleration[0] * deltaTime, 
		velocity[1] + acceleration[1] * deltaTime };

	float velMag = abs(sqrtf(velocity[0] * velocity[0] + velocity[1] * velocity[1]));

	if (velMag != 0)
		direction = { velocity[0] / velMag, velocity[1] / velMag };

	if (abs(velMag) < .00005f)
		velocity = { 0,0 };

	velocity[0] = min(velocity[0], maxVel);
	velocity[1] = min(velocity[1], maxVel);

	position = { position[0] + velocity[0] * deltaTime, position[1] + velocity[1] * deltaTime };

	acceleration = { 0,0 };

	ScreenBounds();

	return position;
}

void PhysicsObject::ScreenBounds()
{
	float offset = .002;

	if (position[0] + radius + velocity[0] > 16)
	{
		FlipVelocity(true, false);
		position = { 16 - radius - offset, position[1] };
	}
	if (position[0] - radius + velocity[0] < 0) {
		FlipVelocity(true, false);
		position = { 0 + radius + offset, position[1] };
	}

	if (position[1] + radius + velocity[1] > 9) {
		FlipVelocity(false, true);
		position = { position[0], 9 - radius - offset };
	}

	if (position[1] - radius + velocity[1] < 0) {
		FlipVelocity(false, true);
		position = { position[0], 0 + radius + offset };
	}

}


