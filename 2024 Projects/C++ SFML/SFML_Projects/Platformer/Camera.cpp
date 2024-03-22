#include "Camera.h"

void Camera::CheckCollisions()
{
	currentPosition = GlobalVariables::getPlayerPosition();

	if (current.CheckCollision(currentPosition)) {
		return;
	}
	for (auto& bb : boundingEdges) {
		// if player enters new bounding box, set view position to new camera
		// position that is at the players current location but still is locked within bounds

		if (bb.CheckCollision(currentPosition)) {
			//std::cout << "set current" << std::endl;
			current = bb;
			return;
		}
	}
}

void Camera::StayInBounds()
{
	sf::Vector2f size = ViewManager::Instance()->GetWindowView().getSize();

	float left, right, up, down;

	left = current.getRect().left;
	up = current.getRect().top;
	right = left + (current.getRect().width);
	down = up + (current.getRect().height);

	sf::Vector2f newPosition(
		clamp(currentPosition.x, left + size.x / 2, right - size.x / 2),
		clamp(currentPosition.y, up + size.y / 2, down - size.y / 2));

	if (current.getRect().width < ViewManager::Instance()->GetWindowView().getSize().x) {
		newPosition.x = left + current.getRect().width / 2;
	}
	if (current.getRect().height < ViewManager::Instance()->GetWindowView().getSize().y) {
		newPosition.y = up + current.getRect().height / 2;
	}

	

	ViewManager::Instance()->SetCameraPosition(newPosition);
}

Camera::Camera()
{

}

void Camera::AddBoundingEdge(BoundingBox boundingEdge)
{
	boundingEdges.push_back(boundingEdge);
}

void Camera::RemoveLastEdge()
{
	if (boundingEdges.size() == 0) return;

	boundingEdges.pop_back();
}

void Camera::Update()
{
	if (boundingEdges.size() <= 0) return;

	CheckCollisions();

	StayInBounds();
}

void Camera::Draw(sf::RenderWindow& window)
{
	for (size_t i = 0; i < boundingEdges.size(); i++)
	{
		boundingEdges[i].Draw(window);
	}
}

std::vector<BoundingBox> Camera::GetBoundingEdges()
{
	return boundingEdges;
}
