#include "Shape.h"

void Shape::Draw()
{
	glColor3f(color[0], color[1], color[2]);

	// draw the cicle
	glBegin(GL_POLYGON);

	for (int i = 0; i < sides; ++i) {
		float t = (float)i / sides * 2.0f * 3.14f;
		glVertex2f(position[0] + radius * cos(t), position[1] + radius * sin(t));
	}

	glEnd();
}

void Shape::Update(vector<float>(*func)()) {

	position = func();

	//printf("(%f, %f)\n", position[0], position[1]);
}

vector<float> Shape::GetPhysicsObjPos()
{
	return obj.Update();
}

void Shape::ApplyForce(vector<float> force)
{
	obj.ApplyForce(force);
}



