#pragma once
#include "Entity.h"
#include "BoundingBox.h"
#include "Input.h"
#include "GlobalVariables.h"
#include "IInteractable.h"
#include "HelperFunctions.h"

class Checkpoint :
    public Entity, IInteractable
{
private:
    sf::Sprite currentSprite;
    sf::Sprite drawnSprite;

    std::map<std::string, sf::Sprite> spriteMap;
    BoundingBox boundingBox;
    bool isActive;
    bool inCollision;
    AnimationState currentState;
    sf::Clock clock;
    sf::Time coolDown;
    sf::Time timeOfSwitch;
    int count;

    // Inherited via IInteractable
    void GetInteraction() override;
    bool CheckCollision() override;
    sf::Sprite GetDrawnSprite();
    void IncrementFrameNum(int modulus);
    sf::Sprite CreateSprite(const sf::Texture* texture);

public:
    Checkpoint(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

    void Draw(sf::RenderWindow& window);

    void Update();
};

