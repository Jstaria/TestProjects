#pragma once
#include "PolyObject.h"
#include <vector>
#include <iostream>
#include "RightClickMenu.h"

class PolyCreation
{
private:
	vector<PolyObject> objects;

	RightClickMenu menu;
	RightClickMenu subMenu;

	bool prevRight;
	bool prevLeft;

	vec3 prevColor;

public:
	PolyCreation();
	~PolyCreation();

	void addVertex(vec2 p_vert); 
	void setColor(vec3 p_color);
	unsigned int getVertNum(); 
	void createPolygon();
	void update(bool mouseRight, bool mouseLeft, vec2 mousePos);
	void draw(vec2& mousePos);
	
};

