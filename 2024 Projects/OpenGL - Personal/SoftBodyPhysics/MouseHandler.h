#pragma once
#include <GL/freeglut.h>

class MouseHandler
{
private:
	int posX, posY;
	bool leftButtonDown;
	bool rightButtonDown;

public:
	MouseHandler() : posX(0), posY(0), leftButtonDown(false), rightButtonDown(false) {}

	static void MouseButtonCallback(int button, int state, int x, int y);
	static void MouseMotionCallback(int x, int y);
	static void PassiveMouseMotionCallback(int x, int y);

	void HandleMouseButton(int button, int state, int x, int y);
	void HandleMouseMotion(int x, int y);
	void HandlePassiveMouseMotion(int x, int y);

	int getX() const { return posX; }
	int getY() const { return posY; }

	bool isLeftButtonDown() const { return leftButtonDown; }
	bool isRightButtonDown() const { return rightButtonDown; }

	static MouseHandler* GetInstance();
};

