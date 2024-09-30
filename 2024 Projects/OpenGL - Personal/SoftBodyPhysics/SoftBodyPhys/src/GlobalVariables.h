#pragma once
class GlobalVariables
{
private:
	float deltaTime;
	unsigned int shaderID;
public:
	GlobalVariables() : deltaTime(0) {}
	static GlobalVariables* GetInstance();

	unsigned int getShaderID() { return shaderID; }
	void incrementShaderID() { shaderID++; }
	float getDeltaTime() const { return deltaTime; }
	void setDeltaTime(float newDeltaTime) { deltaTime = newDeltaTime; }
};

