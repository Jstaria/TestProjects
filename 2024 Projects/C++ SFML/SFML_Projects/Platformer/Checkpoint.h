#pragma once
#include "Entity.h"
#include "BoundingBox.h"
#include "Input.h"
#include "GlobalVariables.h"
#include "IInteractable.h"

class Checkpoint :
    public Entity, IInteractable
{
private:
    sf::Sprite currentSprite;
    std::map<std::string, sf::Sprite> spriteMap;
    BoundingBox boundingBox;
    bool isActive;

    // Inherited via IInteractable
    void GetInteraction() override;
    bool CheckCollision() override;

public:
    Checkpoint(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

    void Draw(sf::RenderWindow& window);

    void Update();
};

