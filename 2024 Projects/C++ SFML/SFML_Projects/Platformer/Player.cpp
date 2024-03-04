#include "Player.h"
#include <iostream>
#include "GlobalVariables.h"

Player::Player(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames)
	: Entity(sprites, position, maxFrames) {

	for (const auto& pair : *Player::sprites) {
		sf::Sprite& sprite = (*Player::sprites)[pair.first];

		sprite.setScale(GlobalVariables::getTextureScaler(), GlobalVariables::getTextureScaler());
		sprite.setPosition(position);
	}

	std::string key = "walk";
	this->frameWidth = (*this->sprites)[key].getLocalBounds().width / maxFrames;
	this->frameHeight = (*this->sprites)[key].getLocalBounds().height;

	this->speedMultiplier = 10;
	this->velocity = sf::Vector2f(0, 0);
	this->maxVelocity = sf::Vector2f(speedMultiplier, 30);

	this->gravity = 1.f;
	this->jumpVelocity = -20.f;
	this->isGrounded = false;

	this->acceleration = .5f;
	this->deceleration = .75f;

	this->clock = sf::Clock();
	this->coyoteTime = sf::seconds(.5f);

	lastFacedDirectionX = 1;
	currentSprite = (*this->sprites)[key];
	drawnSprite = GetCurrentSprite(currentSprite, lastFacedDirectionX);

	CreateBB();
}

void Player::Update() {

	currentSprite = (*sprites)["idle"];

	bool canMove = true;
	bool isMoving = false;

	if (!isGrounded) {
		velocity.y += gravity;
		velocity.y = velocity.y < -maxVelocity.y ? -maxVelocity.y : velocity.y;
	}

	bool anyCollision = false;

	for (auto& bb : *currentLevel->getBBArray()) {
		sf::FloatRect intersection;

		//boundingBoxes["futureHitBox"].SetRect(GetFutureRect())

		if (bb.CheckCollision(boundingBoxes["futureHitbox"].SetRect(GetFutureRect()))) {

			std::cout << "isInCollision" << std::endl;

			// Adjust position based on intersection
			if (velocity.x > 0) {
				position.x -= intersection.width;
			}
			else if (velocity.x < 0) {
				position.x += intersection.width;
			}
			// Stop horizontal movement
			velocity.x = 0;
			canMove = false;
		}

		if (boundingBoxes["GroundBox"].CheckCollision(bb)) {
			velocity.y = 0;
			position.y = bb.GetRect().top - drawnSprite.getLocalBounds().height / 2 * GlobalVariables::getTextureScaler();
			isGrounded = true;
			timeOfGrounded = clock.getElapsedTime();
			anyCollision = true;
			// std::cout << "In collision" << std::endl;
		}
	}


	if (!anyCollision) {
		isGrounded = false;
	}

	sf::Time timeSinceBeingGrounded = clock.getElapsedTime() - timeOfGrounded;

	bool wasGrounded = timeSinceBeingGrounded < coyoteTime && !isGrounded;

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Space) && isGrounded) {
		velocity.y += jumpVelocity;
		isGrounded = false;
	}


	if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
		if (velocity.x > 0) {
			velocity.x -= deceleration * 2;
		}

		if (canMove) {
			velocity.x -= acceleration;
			velocity.x = velocity.x < -maxVelocity.x ? -maxVelocity.x : velocity.x;
			currentSprite = (*sprites)["walk"];
		}
		isMoving = true;
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {
		if (velocity.x < 0) {
			velocity.x += deceleration * 2;
		}

		if (canMove) {
			velocity.x += acceleration;
			velocity.x = velocity.x > maxVelocity.x ? maxVelocity.x : velocity.x;
			currentSprite = (*sprites)["walk"];
		}
		isMoving = true;
	}



	if (!isMoving) {
		if (velocity.x > 0) {
			velocity.x -= deceleration;
			velocity.x = velocity.x < 0 ? 0 : velocity.x;
		}
		else if (velocity.x < 0) {
			velocity.x += deceleration;
			velocity.x = velocity.x > 0 ? 0 : velocity.x;
		}
	}

	//Player::direction = lerp(Player::direction, direction, lerpSpeed);

	Move(velocity);

	drawnSprite = Player::GetCurrentSprite(currentSprite, lastFacedDirectionX);

	if (velocity.x != 0) {
		lastFacedDirectionX = sign(velocity.x);
	}

}

void Player::setCurrentLevel(Level* level)
{
	currentLevel = level;
	MoveTo(level->getPlayerPos());
}

void Player::Draw(sf::RenderWindow& window) {
	window.draw(drawnSprite);

	for (auto& pair : boundingBoxes) {
		pair.second.Draw(window);
	}
}

sf::Sprite Player::GetCurrentSprite(sf::Sprite& currentSprite, int xDirection) {

	const sf::Texture* texture = currentSprite.getTexture();

	sf::Sprite newSprite(*texture, sf::IntRect(frameWidth * frameNum, 0, frameWidth, frameHeight));
	newSprite.setScale(GlobalVariables::getTextureScaler() * sign(xDirection), GlobalVariables::getTextureScaler());
	newSprite.setOrigin(newSprite.getLocalBounds().width / 2, newSprite.getLocalBounds().height / 2);
	newSprite.setPosition(position);

	if (count % (6) == 0) {
		frameNum = (frameNum + 1) % maxFrames;
		count = 0;
	}

	count++;
	return newSprite;
}

void Player::Move(sf::Vector2f speed) {
	position += speed;
	//std::cout << position.x << "," << position.y << std::endl;
	currentSprite.setPosition(position);

	for (auto& pair : boundingBoxes) {
		BoundingBox bb = pair.second;

		if (pair.first == "futureHitbox") continue;

		bb.Move(speed);

		boundingBoxes[pair.first] = bb;
	}
}

void Player::MoveTo(sf::Vector2f pos)
{
	position = pos;
	//std::cout << position.x << "," << position.y << std::endl;
	currentSprite.setPosition(position);

	for (auto& pair : boundingBoxes) {
		BoundingBox bb = pair.second;

		//sf::Vector2f offset(bb.GetRect().width / 2, bb.GetRect().height / 2);

		bb.MoveTo(position);

		boundingBoxes[pair.first] = bb;
	}
}

sf::FloatRect Player::GetFutureRect()
{
	sf::Vector2f tempVelocity(velocity.x * 4, velocity.y * 4);
	return sf::FloatRect(position + tempVelocity, boundingBoxes["Hitbox"].GetRect().getSize());
}

void Player::CreateBB()
{
	int scaler = GlobalVariables::getTextureScaler();
	BoundingBox box(
		position - sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler),
		position + sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler),
		sf::Color::Yellow,
		-sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler));

	boundingBoxes.emplace("Hitbox", box);

	BoundingBox ground(
		position - sf::Vector2f(16, 0),
		position + sf::Vector2f(16, 10),
		sf::Color::Magenta,
		sf::Vector2f(-16, drawnSprite.getLocalBounds().height / 2 * scaler));

	boundingBoxes.emplace("GroundBox", ground);

	BoundingBox fbox(
		position - sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler),
		position + sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler - 50),
		sf::Color::Red,
		-sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler));

	boundingBoxes.emplace("futureHitbox", fbox);
}
