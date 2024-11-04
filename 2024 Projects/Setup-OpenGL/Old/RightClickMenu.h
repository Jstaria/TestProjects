
#pragma once

#include <string>
#include <vector>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtx/constants.hpp>

#include <GL/freeglut.h>
#include <random>

using namespace glm;
using namespace std;

// Menu item structure
struct MenuItem {
    std::string label;
    float x, y, width, height;
};

class RightClickMenu {
public:

    RightClickMenu() {}
    
    bool isMenuVisible = false;
    vec2 menuPos;

    std::vector<MenuItem> menuItems;

    void renderText(float x, float y, const std::string& text) {

        glColor3f(0, 0, 0);
        glRasterPos2f(x, y);

        for (char c : text)
        {
            glutBitmapCharacter(GLUT_BITMAP_HELVETICA_18, c);
        }
    }

    // Draw a rectangle (for the menu)
    void drawRect(float x, float y, float width, float height, vec3 color) {
        glColor3f(color.r, color.g, color.b);
        glBegin(GL_QUADS);
        glVertex2f(x, y);
        glVertex2f(x + width, y);
        glVertex2f(x + width, y - height);
        glVertex2f(x, y - height);
        glEnd();
    }

    bool checkCollision(vec2 mousePos, MenuItem item) {
        return
            mousePos.x > item.x && mousePos.x < item.x + item.width &&
            mousePos.y > item.y - item.height && mousePos.y < item.y;
    }

    int checkCollision(vec2 mousePos) {
        for (int i = 0; i < menuItems.size(); i++)
        {
            if (checkCollision(mousePos, menuItems[i]))
                return i;
        }

        return -1;
    }

    void drawRightClickMenu(vec2 mousePos) {
        if (!isMenuVisible) return;

        for (int i = 0; i < menuItems.size(); i++) {
            float itemX = menuPos[0];
            float itemY = menuPos[1] - i * menuItems[i].height;

            menuItems[i].x = itemX;
            menuItems[i].y = itemY;

            vec3 color = checkCollision(mousePos, menuItems[i]) ? vec3(.85, .85, .85) : vec3(.75, .75, .75);

            drawRect(itemX, itemY, menuItems[i].width, menuItems[i].height, color);
            renderText(itemX + .33f * menuItems[i].width, itemY - .65 * menuItems[i].height, menuItems[i].label);
        }
    }
};

