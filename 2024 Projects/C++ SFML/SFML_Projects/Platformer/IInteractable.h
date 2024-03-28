#pragma once

enum AnimationState {
	Inactive,
	Active,
	Activating,
	Deactivating
};

class IInteractable {
private:
	virtual void GetInteraction() = 0;
	virtual bool CheckCollision() = 0;
};