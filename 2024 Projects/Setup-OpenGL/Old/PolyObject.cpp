#include "PolyObject.h"

PolyObject::PolyObject()
{
	color = vec3(1,0,0);
}

PolyObject::~PolyObject()
{
}

void PolyObject::addVertex(vec2 p_vert)
{
	vertices.push_back(p_vert);
}

void PolyObject::setColor(vec3 p_color)
{
	color = p_color;
}

unsigned int PolyObject::getVertNum()
{
	return vertices.size();
}

bool PolyObject::getCreated()
{
	return isCreated;
}

void PolyObject::setCreated(bool value)
{
	isCreated = value;
}

void PolyObject::draw()
{
	glColor3f(color.r, color.g, color.b);
	glLineWidth(5);

	if (vertices.size() > 2)
		glBegin(GL_POLYGON);

	else if (vertices.size() == 2) {
		glLineWidth(5);
		glBegin(GL_LINES);
	}

	else {
		glPointSize(5);
		glBegin(GL_POINTS);
	}

	for (int i = 0; i < vertices.size(); i++)
	{
		glVertex2f(vertices[i].x, vertices[i].y);
	}

	glEnd();
}

void PolyObject::draw(vec2& p_mousePos)
{
	glColor3f(color.r, color.g, color.b);

	if (vertices.size() >= 2)
		glBegin(GL_POLYGON);

	else if (vertices.size() == 1) {
		glLineWidth(5);
		glBegin(GL_LINES);
	}
		
	else {
		glPointSize(5);
		glBegin(GL_POINTS);
	}
		

	for (int i = 0; i < vertices.size(); i++)
	{
		glVertex2f(vertices[i].x, vertices[i].y);
	}

	glVertex2f(p_mousePos.x, p_mousePos.y);

	glEnd();
}
