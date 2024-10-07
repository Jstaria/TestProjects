#include "PostProcessingClass.h"

PostProcessingClass::PostProcessingClass()
{
	model = mat4(1.0f);
}

PostProcessingClass::~PostProcessingClass()
{
	delete[] vertices;
	delete[] triangles;
	delete[] fnormals;
	delete[] vnormals;

	glDeleteFramebuffers(1, &fbo);
	glDeleteTextures(1, &textureColorBuffer);
}

void PostProcessingClass::Create(const char* v_shader_file, const char* f_shader_file)
{
	prepareFBOandTextureBuffer();
	
	// Fullscreen, 2 triangle mesh
	vector<vec3> ori_vertices = {
		// Positions        // TexCoords
		vec3(0.0f, 9.0f, 10.0f),  // Top-left
		vec3(0.0f,0.0f, 10.0f),  // Bottom-left
		vec3( 16.0f,0.0f, 10.0f),  // Bottom-right
		vec3( 16.0f, 9.0f, 10.0f)   // Top-right
	};

	vector<uvec3> ori_triangles = {
		{0, 1, 2},
		{0, 2, 3}
	};

	vert_num = ori_vertices.size();
	tri_num = ori_triangles.size();

	vertices = new vec3[vert_num];
	triangles = new uvec3[tri_num];

	for (uint i = 0; i < vert_num; i++) {
		vertices[i] = ori_vertices[i];
	}
	for (uint i = 0; i < tri_num; i++) {
		triangles[i] = ori_triangles[i];
	}


	//I don't even know if I need the normals for an orthographic scene view, as I'm not using light
	//computeNormals();
	prepareVBOandShaders(v_shader_file, f_shader_file);
}

void PostProcessingClass::RenderToFBO(void(*func)())
{
	//prepareFBOandTextureBuffer();

	glBindFramebuffer(GL_FRAMEBUFFER, fbo);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glUseProgram(0);

	func();

	glBindFramebuffer(GL_FRAMEBUFFER, 0);
}

void PostProcessingClass::Draw(void (*func)(), mat4 view, mat4 proj)
{
	// Render frame buffer to fbo
	RenderToFBO(func);

	glBindTexture(GL_TEXTURE_2D, textureColorBuffer);

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	if (vert_num <= 0 || tri_num <= 0)
		return;

	glUseProgram(shaderProg.id);
	
	MVP = model * view * proj;

	shaderProg.setInt("screenTexture", 0);
	shaderProg.setMatric4v("MVP", 1, value_ptr(MVP));

	// Only gives me a fullscreen quad if I double world coord scalings
	glBegin(GL_QUADS);
	glVertex3f(-16.0f, 9.0f, 0.0f); // Top-left
	glVertex3f(-16.0f, -9.0f, 0.0f); // Bottom-left
	glVertex3f(16.0f, -9.0f, 0.0f); // Bottom-right
	glVertex3f(16.0f, 9.0f, 0.0f); // Top-right
	glEnd();

	// Commented out because I don't know if this is doing *anything*
	// Well, it did at one point, *I think*
	//glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ibo);
	//glDrawElements(GL_TRIANGLES, tri_num * 3, GL_UNSIGNED_INT, NULL);
}

void PostProcessingClass::computeNormals()
{
	fnormals = new vec3[tri_num];
	vnormals = new vec3[vert_num];

	vec3 a, b, n;

	for (unsigned int i = 0; i < tri_num; i++) {
		a = vertices[triangles[i][1]] - vertices[triangles[i][0]];
		b = vertices[triangles[i][2]] - vertices[triangles[i][0]];

		n = cross(a, b);

		fnormals[i] = normalize(n);
	}

	for (unsigned int i = 0; i < vert_num; i++) {
		vnormals[i] = vec3(0.0f);
	}

	for (unsigned int i = 0; i < tri_num; i++) {
		for (unsigned int j = 0; j < 3; j++) {
			vnormals[triangles[i][j]] += fnormals[i];
		}
	}

	for (unsigned int i = 0; i < vert_num; i++) {
		vnormals[i] = normalize(vnormals[i]);
	}
}

void PostProcessingClass::prepareVBOandShaders(const char* v_shader_file, const char* f_shader_file)
{
	vShader.Create(v_shader_file, GL_VERTEX_SHADER);
	fShader.Create(f_shader_file, GL_FRAGMENT_SHADER);

	shaderProg.Create();
	shaderProg.link(vShader);
	shaderProg.link(fShader);

	vShader.Destroy();
	fShader.Destroy();

	glGenVertexArrays(1, &vao);
	glBindVertexArray(vao);

	glGenBuffers(1, &vbo);
	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vec3) * vert_num, vertices, GL_STATIC_DRAW);
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	glGenBuffers(1, &nbo);
	glBindBuffer(GL_ARRAY_BUFFER, nbo);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vec3) * vert_num, vnormals, GL_STATIC_DRAW);
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	glGenBuffers(1, &ibo);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ibo);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uvec3) * tri_num, triangles, GL_STATIC_DRAW);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	
	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, NULL);

	glBindBuffer(GL_ARRAY_BUFFER, nbo);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 0, NULL);

	glEnableVertexAttribArray(0);
	//glEnableVertexAttribArray(1);

	// Unbind the VAO (to avoid accidental modification later)
	glBindVertexArray(0);

	// Unbind the buffers (optional, but good practice)
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
}

void PostProcessingClass::prepareFBOandTextureBuffer()
{
	glGenFramebuffers(1, &fbo);
	glBindFramebuffer(GL_FRAMEBUFFER, fbo);

	glGenTextures(1, &textureColorBuffer);
	glBindTexture(GL_TEXTURE_2D, textureColorBuffer);

	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, glutGet(GLUT_WINDOW_WIDTH), glutGet(GLUT_WINDOW_HEIGHT), 0, GL_RGB, GL_UNSIGNED_BYTE, NULL);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, textureColorBuffer, 0);

	unsigned int rbo;
	glGenRenderbuffers(1, &rbo);
	glBindRenderbuffer(GL_RENDERBUFFER, rbo);
	glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH24_STENCIL8, 800, 600);
	glBindRenderbuffer(GL_RENDERBUFFER, 0);

	glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_STENCIL_ATTACHMENT, GL_RENDERBUFFER, rbo);

	if (glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
		cerr << "Framebuffer not complete!" << endl;

	glBindFramebuffer(GL_FRAMEBUFFER, 0);

	//glDeleteRenderbuffers(1, &rbo);
	//glDeleteFramebuffers(1, &fbo);
}

// Just trying to figure out what is wrong here, for the most part I get GL_INVALID_ENUM, dont know why
void PostProcessingClass::printError(string whereInCode)
{
	GLenum err = glGetError();

	switch (err)
	{
	case GL_NO_ERROR:

	case GL_INVALID_ENUM:
		std::cerr << "GL_INVALID_ENUM: An unacceptable value is specified." << std::endl;
		break;
	case GL_INVALID_VALUE:
		std::cerr << "GL_INVALID_VALUE: A numeric argument is out of range." << std::endl;
		break;
	case GL_INVALID_OPERATION:
		std::cerr << "GL_INVALID_OPERATION: The specified operation is not allowed in the current state." << std::endl;
		break;
	case GL_OUT_OF_MEMORY:
		std::cerr << "GL_OUT_OF_MEMORY: There is not enough memory left to execute the command." << std::endl;
		break;
	default:
		std::cerr << "Unknown OpenGL error: " << err << std::endl;
		break;
	}
}
