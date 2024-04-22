#include "HelperFunctions.h"

float degreesToRadians(float degrees) {
	return degrees * 3.14159265358979323846f / 180.0f;
}

// Linear interpolation for sf::Vector2f
sf::Vector2f lerp(const sf::Vector2f a, const sf::Vector2f b, float t) {
	return (1 - t) * a + t * b;
}

float lerp(const float a, const float b, float t)
{
	return (1 - t) * a + t * b;
}

sf::Vector2f Normalize(sf::Vector2f vector, float multiplier) {
	float length = std::sqrt(vector.x * vector.x + vector.y * vector.y);

	if (length != 0.f) {
		return sf::Vector2f(vector.x / length * multiplier, vector.y / length * multiplier);
	}
	else {
		return vector;
	}
}

int sign(float x){
	if (x > 0) return 1;
	if (x < 0) return -1;
	return 0;
}

float Distance(const sf::Vector2f v1, const sf::Vector2f v2) {
	float dx = v2.x - v1.x;
	float dy = v2.y - v1.y;
	return std::sqrt(dx * dx + dy * dy);
}

float clamp(float value, float low, float high) {

	if (value > high) return high;
	if (value < low) return low;
	
	return value;
}

float Cross2D(const sf::Vector2f a, const sf::Vector2f b)
{
	return a.x * b.y - a.y * b.x;
}

float Dot2D(const sf::Vector2f a, const sf::Vector2f b)
{
	return a.x * b.x + a.y * b.y;
}

float AngleBetweenPoints(const sf::Vector2f& p1, const sf::Vector2f& p2, const sf::Vector2f& reference) {
	float angle = std::atan2(p1.y - reference.y, p1.x - reference.x) - std::atan2(p2.y - reference.y, p2.x - reference.x);
	if (angle < 0)
		angle += 2 * 3.14159265358979323846f; 
	return angle;
}

void SortPointsClockwise(std::vector<sf::Vector2f>& points, const sf::Vector2f& reference) {
	
	std::sort(points.begin(), points.end(), [&](const sf::Vector2f& p1, const sf::Vector2f& p2) {
		return AngleBetweenPoints(p1, p2, reference) < AngleBetweenPoints(p2, p1, reference);
		});
}

float magnitude(const sf::Vector2f& vec) {
	return std::sqrt(std::pow(vec.x, 2) + std::pow(vec.y, 2));
}

// Function to limit the distance of a vector
sf::Vector2f limitDistance(const sf::Vector2f& vec, float maxLength) {
	float mag = magnitude(vec);
	if (mag > maxLength) {
		// Scale down the vector to maxLength while preserving direction
		return vec * (maxLength / mag);
	}
	return vec;
}