#pragma once
#include "Entity.h"
#include "BoundingBox.h"
#include "Input.h"
#include "GlobalVariables.h"

class Checkpoint :
    public Entity
{
private:
    sf::Sprite currentSprite;
    BoundingBox bb;

    bool CheckCollision();

public:
    Checkpoint(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

    void Draw(sf::RenderTarget& window);

    void Update();
};

