#pragma once
#include <GL/freeglut.h>
#include "PhysicsObject.h"
#include "Shape.h"

enum SoftBodyShape
{
	Square,
	Circle
};

class SoftBody
{
private:
	vector<vector<Shape>> softBody;
	vector<vector<vector<float>>> softBodyPos;

	float restLength;
	float springConstant;
	int size;

	void CreateSoftBody(SoftBodyShape shape, int size, vector<float> position, float restlength);
	void ApplySpringForce(vector<int> arrayPos, vector<float> center, float springConstant, float restLength);

public:
	void ApplyForce(vector<float> force);
	SoftBody(int size, SoftBodyShape shape, vector<float> position, 
		float restLength, float springConstant);
	SoftBody() {}
	void Update();
	void Draw();

	Shape& GetFirstShape();
};

