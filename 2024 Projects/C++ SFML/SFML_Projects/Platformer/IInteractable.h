#pragma once

class IInteractable {
private:
	virtual void GetInteraction() const = 0;
	virtual void CheckCollision() const = 0;
};