#pragma once
#include <SFML/Graphics.hpp>
#include <cmath>
#include <algorithm>
#include <SFML/System.hpp>

#include <cmath>

float degreesToRadians(float degrees);

// Linear interpolation for sf::Vector2f
sf::Vector2f lerp(const sf::Vector2f& a, const sf::Vector2f& b, float t);
float lerp(const float a, const float b, float t);

sf::Vector2f Normalize(sf::Vector2f vector, int multiplier);

float clamp(float value, float low, float high);

float Distance(const sf::Vector2f v1, const sf::Vector2f v2);

int sign(float x);

float Cross2D(const sf::Vector2f a, const sf::Vector2f b);

float Dot2D(const sf::Vector2f a, const sf::Vector2f b);

float AngleBetweenPoints(const sf::Vector2f& p1, const sf::Vector2f& p2, const sf::Vector2f& reference);

void SortPointsClockwise(std::vector<sf::Vector2f>& points, const sf::Vector2f& reference);