#pragma once
class GlobalVariables
{
private:
	float deltaTime;

public:
	GlobalVariables() : deltaTime(0) {}
	static GlobalVariables* GetInstance();

	float getDeltaTime() const { return deltaTime; }
	void setDeltaTime(float newDeltaTime) { deltaTime = newDeltaTime; }
};

