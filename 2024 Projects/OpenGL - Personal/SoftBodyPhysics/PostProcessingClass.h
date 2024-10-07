#pragma once

#include "ShaderProgram.h"

#include <vector>
#include <set>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtx/constants.hpp>
#include <glm/geometric.hpp>
#include <glm/gtx/quaternion.hpp>
#include <glm/gtx/string_cast.hpp>

#include <fstream>
#include <iostream>
#include <sstream>
#include <GL/freeglut.h>

using namespace std;
using namespace glm;

class PostProcessingClass
{
public:
	uint vert_num, tri_num;

	vec3* vertices;
	uvec3* triangles;
	vec3* fnormals;
	vec3* vnormals;

	GLuint vao, vbo, nbo, ibo, fbo;
	GLuint textureColorBuffer;
	ShaderProgram shaderProg;
	ShaderClass fShader;
	ShaderClass vShader;

	mat4 model;
	mat4 MVP;

	float normal_offset = 0.0f;

public:
	PostProcessingClass();
	~PostProcessingClass();

	void Create(const char* v_shader_file, const char* f_shader_file);
	void RenderToFBO(void (*func)());
	void Draw(void (*func)(), mat4 view, mat4 proj);

private:
	void computeNormals();
	void prepareVBOandShaders(const char* v_shader_file, const char* f_shader_file);
	void prepareFBOandTextureBuffer();

	void printError(string whereInCode);
};

