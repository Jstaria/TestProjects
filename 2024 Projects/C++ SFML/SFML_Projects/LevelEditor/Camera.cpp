#include "Camera.h"

void Camera::CheckCollisions()
{
	for (auto& pair : boundingEdges) {
		// if player enters new bounding box, set view position to new camera
		// position that is at the players current location but still is locked within bounds
	}
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
	CheckCollisions();

	// Some other camera nonsense
}

std::vector<BoundingBox> Camera::GetBoundingEdges()
{
	return boundingEdges;
}
