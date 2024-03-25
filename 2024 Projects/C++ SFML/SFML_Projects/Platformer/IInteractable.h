#pragma once

class IInteractable {
private:
	virtual void GetInteraction() = 0;
	virtual bool CheckCollision() = 0;
};