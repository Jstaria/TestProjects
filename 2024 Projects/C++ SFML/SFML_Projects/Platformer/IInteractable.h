#pragma once

class IInteractable {
private:
	virtual void GetInteraction() const = 0;
	virtual bool CheckCollision() const = 0;
};