#pragma once
#include <vector>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtx/constants.hpp>

#include <GL/freeglut.h>

using namespace glm;
using namespace std;

class PolyObject
{
private:
	vector <vec2> vertices; // all vertices in C++ vector
	vec3 color; // color of this polygon

	bool isCreated = false; // returns whether the polygon has been created

public:
	PolyObject();
	~PolyObject();
	void addVertex(vec2 p_vert); // add vertex at the end of the vertex list
	void setColor(vec3 p_color); // set the color of this polygon
	unsigned int getVertNum(); // return the number of vertices
	bool getCreated(); // return created state
	void setCreated(bool value);
	void draw(); // draw the polygon if it’s completed
	void draw(vec2& p_mousePos); // draw the polygon if it’s being created
};
