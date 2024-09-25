#pragma once

#include <vector>
#include <GL/freeglut.h>
#include "PhysicsObject.h"

using namespace std;

class Shape
{
private:
	float radius;
	int sides;
	vector<float> position;
	vector<float> color;

	PhysicsObject obj;

public: 
	Shape() {}
	Shape(vector<float>& pos, vector<float>& c, float r, int s, float m, float f, bool g, bool fr) : 
		position(pos), color(c), radius(r), sides(s)
	{
		obj = PhysicsObject(pos, m, f, g, fr, r);
	}

	void Draw();
	void Update();
	vector<float> GetPhysicsObjPos();
	void ApplyForce(vector<float> force);
	vector<float> GetPosition() { return position; }
	PhysicsObject GetPhysicsObj() { return obj; }
};

