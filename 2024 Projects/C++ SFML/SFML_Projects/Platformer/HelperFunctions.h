#pragma once
#include <SFML/Graphics.hpp>
#include <cmath>
#include <algorithm>

// Linear interpolation for sf::Vector2f
sf::Vector2f lerp(const sf::Vector2f& a, const sf::Vector2f& b, float t);

sf::Vector2f Normalize(sf::Vector2f& vector, int multiplier);

float clamp(float value, float low, float high);

int sign(float x);
