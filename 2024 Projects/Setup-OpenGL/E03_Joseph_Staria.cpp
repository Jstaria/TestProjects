// Course: 		    IGME 309
// Student Name: 	Joseph Staria
// Friday Exercise: 03

#ifdef __APPLE__
#include <GLUT/glut.h> // include glut for Mac
#else
#include <GL/freeglut.h> //include glut for Windows
#endif


// the window's width and height
int width, height;

void init(void)
{
    // initialize the size of the window
    width = 600;
    height = 600;
}

void DrawTree(int x, int y, int tri_height, int tri_width, float heightOffset) {
    // this is immedidate mode of OpenGL usded prior to OpenGL 3.0
    // draw hull

    glColor3f(0.75f, 0.5f, 0.0f);
    glBegin(GL_POLYGON);

    glVertex2f(x - ((float)tri_width / 2), 0);
    glVertex2f(x - ((float)tri_width / 2), 1);
    glVertex2f(x + ((float)tri_width / 2), 1);
    glVertex2f(x + ((float)tri_width / 2), 0);

    glEnd();

    glColor3f(0.5f, 0.75f, 0.3f);
    glBegin(GL_TRIANGLES);

    int num = 3;

    for (int i = 0; i < num; i++)
    {
        glVertex2f((GLfloat)(x - (tri_width) * 2 + (i / (float)num * 1.5f)), (GLfloat)(y + i * tri_height - heightOffset * i) + 1);

        glVertex2f((GLfloat)x, (GLfloat)(y + tri_height + i * tri_height - heightOffset * i / 5) + 1);
        
        glVertex2f((GLfloat)(x + (tri_width) * 2 - (i / (float)num * 1.5f)), (GLfloat)(y + i * tri_height - heightOffset * i) + 1);
    }

    glEnd();

    glColor3f(1, 0, 0);
    glPointSize(30.0f);
    glBegin(GL_POINTS);

    for (int i = 0; i < num; i++)
    {
        glVertex2f((GLfloat)(x - (tri_width) * 2 + (i / (float)num * 1.5f)), (GLfloat)(y + i * tri_height - heightOffset * i) + 1);

        glVertex2f((GLfloat)(x + (tri_width) * 2 - (i / (float)num * 1.5f)), (GLfloat)(y + i * tri_height - heightOffset * i) + 1);
    }

    glEnd();
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

    // specify the color for drawing
    glColor3f(1.0, 0.0, 0.0);

    // this is immedidate mode of OpenGL usded prior to OpenGL 3.0
    DrawTree(5, 0, 2, 1, .75f);

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

    //register function to handle window resizes
    glutReshapeFunc(reshape);

    //register function that draws in the window
    glutDisplayFunc(display);

    //start the glut main loop
    glutMainLoop();

    return 0;
}