#pragma once

enum AnimationState {
	Inactive,
	Active,
	Activating,
	Deactivating
};

class IInteractable {
public:
	virtual void Draw(sf::RenderWindow& window) = 0;
	virtual void Update() = 0;
private:
	virtual void GetInteraction() = 0;
	virtual bool CheckCollision() = 0;
};