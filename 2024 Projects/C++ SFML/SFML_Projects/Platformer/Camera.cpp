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
			std::cout << "set current" << std::endl;
			current = bb;
			return;
		}
	}

	ViewManager::Instance()->SetCameraPosition(currentPosition);
}

void Camera::StayInBounds()
{
	sf::Vector2f size = ViewManager::Instance()->GetWindowView().getSize();
	sf::Vector2f position = ViewManager::Instance()->GetWindowView().getCenter();

	float left, right, up, down;

	left = current.getRect().left + size.x / 2;
	up = current.getRect().top + size.y / 2;
	right = left + current.getRect().width - size.x / 2;
	down = up + current.getRect().height - size.y / 2;

	sf::Vector2f newPosition(clamp(currentPosition.x, left, right), clamp(currentPosition.y, up, down));

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
