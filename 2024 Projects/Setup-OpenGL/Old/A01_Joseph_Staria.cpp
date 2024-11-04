// Course: 		    IGME 309
// Student Name: 	Joseph Staria
// Assignment:      01

#ifdef __APPLE__
#include <GLUT/glut.h> // include glut for Mac
#else
#include <GL/freeglut.h> //include glut for Windows
#endif

#include <math.h>
#include <numbers>


// the window's width and height
int width, height, maxVertices;

void init(void)
{
    // initialize the size of the window
    width = 600;
    height = 600;
    maxVertices = 100;
}

void DrawFilledCircle(float red, float green, float blue, float centerX, float centerY, float radius) {
    
    glColor3f(red, green, blue);
    glBegin(GL_POLYGON);

    float radians = 2 * (atan(1) * 4) / maxVertices;

    for (int i = 0; i < maxVertices; i++)
    {
        glVertex2f(centerX + radius * cos(radians * i), centerY + radius * sin(radians * i));
    }
    glEnd();

    glutPostRedisplay();
}

void DrawWireframeCircle(float red, float green, float blue, float centerX, float centerY, float radius, float lineWidth) {

    glColor3f(red, green, blue);
    glLineWidth(lineWidth);
    glBegin(GL_LINE_LOOP);

    float radians = 2 * (atan(1) * 4) / maxVertices;

    for (int i = 0; i < maxVertices; i++)
    {
        glVertex2f(centerX + radius * cos(radians * i), centerY + radius * sin(radians * i));
    }
    glEnd();

    glutPostRedisplay();
}

// 20 total objects
void DrawPanda() 
{
    DrawFilledCircle(1, .5f, .75f, 5, 0, 4);
    DrawWireframeCircle(.25f, .75f, 1, 5, 0, 4, 20);

    DrawFilledCircle(1, .5f, .75f, 7, 7, 2);
    DrawWireframeCircle(.25f, .75f, 1, 7, 7, 2, 5);

    DrawFilledCircle(1, .5f, .75f, 3, 7, 2);
    DrawWireframeCircle(.25f, .75, 1, 3, 7, 2, 5);

    DrawFilledCircle(1, .5f, .75f, 5, 5, 3);
    DrawWireframeCircle(.25f, .75f, 1, 5, 5, 3, 20);

    DrawFilledCircle(0,0,0, 5, 5, 2);
    DrawFilledCircle(1, .5f, .75f, 5, 5.5, 2);

    DrawFilledCircle(1, 1, 1, 6, 6, 1);
    DrawWireframeCircle(.25f, .75f, 1, 6, 6, 1, 5);

    DrawFilledCircle(1, 1, 1, 4, 6, 1);
    DrawWireframeCircle(.25f, .75, 1, 4, 6, 1, 5);

    DrawFilledCircle(0,0,0, 4, 6, .75f);
    DrawFilledCircle(0,0,0, 6, 6, .75f);

    DrawFilledCircle(1, .5f, .75f, 8, 1, 1.5);
    DrawWireframeCircle(.25f, .75f, 1, 8, 1, 1.5, 5);

    DrawFilledCircle(1, .5f, .75f, 2, 1, 1.5);
    DrawWireframeCircle(.25f, .75, 1, 2, 1, 1.5, 5);
}

void Keyboard(unsigned char key, int x, int y) 
{
    switch (key) {
    case '-':
        maxVertices--;
        //exit(0);
        break;

    case '+':
        maxVertices++;
        //exit(0);
        break;
    }
}

// called when the GL context need to be rendered
void display(void)
{
    // clear the screen to white, which is the background color
    glClearColor(1.0, 1.0, 1.0, 0.0);

    // clear the buffer stored for drawing
    glClear(GL_COLOR_BUFFER_BIT);


    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();

    DrawPanda();

    // specify the color for new drawing
    glColor3f(0.0, 0.0, 1.0);

    glutSwapBuffers();
}

// called when window is first created or when window is resized
void reshape(int w, int h)
{
    // update thescreen dimensions
    width = w;
    height = h;

    //do an orthographic parallel projection, limited by screen/window size
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    gluOrtho2D(0.0, 10.0, 0.0, 10.0);
    //gluOrtho2D(-5.0, 5.0, -5.0, 5.0);

    /* tell OpenGL to use the whole window for drawing */
    glViewport(0, 0, (GLsizei)width, (GLsizei)height);
    //glViewport((GLsizei) width/2, (GLsizei) height/2, (GLsizei) width, (GLsizei) height);

    glutPostRedisplay();
}

int main(int argc, char* argv[])
{
    // before create a glut window,
    // initialize stuff not opengl/glut dependent
    init();

    //initialize GLUT, let it extract command-line GLUT options that you may provide
    //NOTE that the '&' before argc
    glutInit(&argc, argv);

    // specify as double bufferred can make the display faster
    // Color is speicfied to RGBA, four color channels with Red, Green, Blue and Alpha(depth)
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA);

    //set the initial window size */
    glutInitWindowSize((int)width, (int)height);

    // create the window with a title
    glutCreateWindow("First OpenGL Program");

    /* --- register callbacks with GLUT --- */

    glutKeyboardFunc(Keyboard);

    //register function to handle window resizes
    glutReshapeFunc(reshape);

    //register function that draws in the window
    glutDisplayFunc(display);

    

    //start the glut main loop
    glutMainLoop();

    return 0;
}