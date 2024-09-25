#include "PhysicsObject.h"

vector<float> velocity = { 0, 0 };
vector<float> acceleration = { 0, 0 };

vector<float> position = { 8, 9 };
vector<float> direction = { 0, 0 };

float maxVel = 3;

void PhysicsObject::ApplyGravity(vector<float> force)
{
	(*acceleration)[0] += force[0];
	(*acceleration)[1] += force[1];
}

void PhysicsObject::ApplyForce(vector<float> force)
{
	(*acceleration)[0] += force[0] / mass;
	(*acceleration)[1] += force[1] / mass;
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
	float dampening = .5f;
	velocity = { x ? -velocity[0] * dampening: velocity[0], y ? -velocity[1] * dampening : velocity[1] };
}

void PhysicsObject::ApplySpringForce(float springConstant, vector<float> center, float restLength)
{
	float distance =  sqrtf(pow(position[0] - center[0], 2) + pow(position[1] - center[1], 2));
	float x = distance - restLength;

	vector<float> force = { (position[0] - center[0]) / distance, (position[1] - center[1]) / distance};

	float b = .75;

	force = { -springConstant * x * force[0] - velocity[0] * b, -springConstant * x * force[1] - velocity[1] * b };

	ApplyForce(force);
}

vector<float> PhysicsObject::Update()
{
	if (useGravity) ApplyGravity(vector<float> {0, -.098f});
	if (useFriction) ApplyFriction(frictionCoeff);

	float deltaTime = GlobalVariables::GetInstance()->getDeltaTime();

	velocity = { 
		velocity[0] + (*acceleration)[0] * deltaTime,
		velocity[1] + (*acceleration)[1] * deltaTime };

	float velMag = abs(sqrtf(velocity[0] * velocity[0] + velocity[1] * velocity[1]));

	if (velMag != 0)
		direction = { velocity[0] / velMag, velocity[1] / velMag };
	else
		direction = { 0,0 };

	velocity[0] = min(velocity[0], maxVel);
	velocity[1] = min(velocity[1], maxVel);
	velocity[0] = max(velocity[0], -maxVel);
	velocity[1] = max(velocity[1], -maxVel);

	// printf("(%f,%f)\n", velocity[0], velocity[1]);

	position = { position[0] + velocity[0] * deltaTime, position[1] + velocity[1] * deltaTime };

	(*acceleration) = { 0,0 };

	ScreenBounds();

	return position;
}

void PhysicsObject::ScreenBounds()
{
	float offset = .002;
	float deltaTime = GlobalVariables::GetInstance()->getDeltaTime();

	if (position[0] + radius + velocity[0] * deltaTime > 16)
	{
		FlipVelocity(true, false);
		position = { 16 - radius - offset, position[1] };
	}
	if (position[0] - radius + velocity[0] * deltaTime < 0) {
		FlipVelocity(true, false);
		position = { 0 + radius + offset, position[1] };
	}

	if (position[1] + radius + velocity[1] * deltaTime > 9) {
		FlipVelocity(false, true);
		position = { position[0], 9 - radius - offset };
	}

	if (position[1] - radius + velocity[1] * deltaTime < 0) {
		FlipVelocity(false, true);
		position = { position[0], 0 + radius + offset };
	}

}


