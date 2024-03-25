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
    BoundingBox bb;

    // Inherited via IInteractable
    void GetInteraction() const override;
    void CheckCollision() const override;

public:
    Checkpoint(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

    void Draw(sf::RenderTarget& window);

    void Update();
};

