#include "PolyCreation.h"

PolyCreation::PolyCreation()
{
	objects.push_back(PolyObject());
	prevRight = false;
	prevLeft = false;

	menu.menuItems = {
		{"Color", 0, 0, 2.50, .750},
		{"Clear", 0, 0, 2.50, .750},
		{"Quit", 0, 0, 2.50, .750}
	};

	subMenu.menuItems = {
		{"Red", 0, 0, 2.00, .50},
		{"Green", 0, 0, 2.00, .50},
		{"Blue", 0, 0, 2.00, .50},
		{"Black", 0, 0, 2.00, .50 },
		{"Random", 0, 0, 2.00, .50 }
	};
}

PolyCreation::~PolyCreation()
{
}

/// <summary>
/// Adds vertex to most recent polygon
/// </summary>
/// <param name="p_vert"></param>
void PolyCreation::addVertex(vec2 p_vert)
{
	objects[objects.size() - 1].addVertex(p_vert);
}

/// <summary>
/// Set color of most recent polygon
/// </summary>
/// <param name="p_color"></param>
void PolyCreation::setColor(vec3 p_color)
{
	objects[objects.size() - 1].setColor(p_color);
}

/// <summary>
/// Returns most recent polygon vert number
/// </summary>
/// <returns></returns>
unsigned int PolyCreation::getVertNum()
{
	return objects[objects.size() - 1].getVertNum();
}

void PolyCreation::createPolygon()
{
	if (objects[objects.size() - 1].getVertNum() == 0) return;
	objects[objects.size() - 1].setCreated(true);
	objects.push_back(PolyObject());
	objects[objects.size() - 1].setColor(prevColor);
}

void PolyCreation::update(bool mouseRight, bool mouseLeft, vec2 mousePos)
{
	if (mouseLeft && !prevLeft && !menu.isMenuVisible) {
		addVertex(mousePos);
	}
	if (mouseRight && !prevRight) {
		menu.menuPos = mousePos;
		menu.isMenuVisible = !menu.isMenuVisible;
		subMenu.isMenuVisible = false;
	}

#pragma region Menu Interaction
	if (mouseLeft && !prevLeft && menu.isMenuVisible && !subMenu.isMenuVisible) {
		int item = menu.checkCollision(mousePos);
		subMenu.isMenuVisible = false;

		switch (item) {
		case -1:
			break;

		case 0:

			subMenu.isMenuVisible = !subMenu.isMenuVisible;
			subMenu.menuPos = vec2(menu.menuItems[0].x + menu.menuItems[0].width, menu.menuItems[0].y);
			break;

		case 1:
			objects.clear();
			objects.push_back(PolyObject());
			menu.isMenuVisible = false;
			break;

		case 2:
			exit(0);
			break;
		}
	}
#pragma endregion

#pragma region Submenu Interaction
	if (mouseLeft && !prevLeft && subMenu.isMenuVisible) {
		int item = subMenu.checkCollision(mousePos);

		switch (item) {
		case -1:
			break;

		case 0:

			objects[objects.size() - 1].setColor(prevColor = vec3(1, 0, 0));

			break;

		case 1:

			objects[objects.size() - 1].setColor(prevColor = vec3(0, 1, 0));

			break;

		case 2:
			objects[objects.size() - 1].setColor(prevColor = vec3(0, 0, 1));
			break;

		case 3:
			objects[objects.size() - 1].setColor(prevColor = vec3(0, 0, 0));
			break;

		case 4:
			objects[objects.size() - 1].setColor(prevColor = vec3(((double)rand()) / RAND_MAX, ((double)rand()) / RAND_MAX, ((double)rand()) / RAND_MAX));
		}
	}
#pragma endregion

	prevRight = mouseRight;
	prevLeft = mouseLeft;
}

/// <summary>
///  Draws all polygon objects
/// </summary>
/// <param name="mousePos"></param>
void PolyCreation::draw(vec2& mousePos)
{
	for (int i = 0; i < objects.size(); i++)
	{
		//cout << objects[i].getVertNum() << endl;

		if (objects[i].getCreated())
			objects[i].draw();
		else 
			objects[i].draw(mousePos);

		menu.drawRightClickMenu(mousePos);
		subMenu.drawRightClickMenu(mousePos);
	}
}


