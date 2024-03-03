#include "HelperFunctions.h"

// Linear interpolation for sf::Vector2f
sf::Vector2f lerp(const sf::Vector2f& a, const sf::Vector2f& b, float t) {
	return (1 - t) * a + t * b;
}

sf::Vector2f Normalize(sf::Vector2f& vector, int multiplier) {
	float length = std::sqrt(vector.x * vector.x + vector.y * vector.y);

	if (length != 0.f) {
		return sf::Vector2f(vector.x / length * multiplier, vector.y / length * multiplier);
	}
	else {
		return vector;
	}
}

int sign(int x){
	if (x > 0) return 1;
	if (x < 0) return -1;
	return 0;
}