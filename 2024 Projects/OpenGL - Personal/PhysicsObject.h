#pragma once

#include "GlobalVariables.h"
#include <vector>

using namespace std;

class PhysicsObject
{
private:
	float mass, frictionCoeff, radius;

	vector<float> velocity = { 0, 0 };
	vector<float> acceleration = { 0, 0 };

	vector<float> position = { 8, 9 };
	vector<float> direction = { 0, 0 };

	bool useFriction, useGravity;
	bool useForces = true;

	void ScreenBounds();

public:
	PhysicsObject() : mass(0), frictionCoeff(0), useFriction(false), useGravity(false) {}
	PhysicsObject(float m, float f, bool g, bool fr, float r) : 
		mass(m), frictionCoeff(f), useFriction(fr), useGravity(g), radius(r) {}

	void ApplyForce(vector<float> force);
	void ApplyGravity(vector<float> force);
	void ApplyFriction(float coeff);
	void FlipVelocity(bool x, bool y);
	void ApplySpringForce(float springConstant, vector<float> center);

	vector<float> GetDirection() { return direction; }
	void SetPosition(vector<float> pos) { position = pos; }
	void SetUseForces(bool use) { useForces = use; }

	vector<float> Update();
};

