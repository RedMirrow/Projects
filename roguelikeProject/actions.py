#Declaring new classes to import into the main.py when needed
from __future__ import annotations
from typing import Optional, Tuple, TYPE_CHECKING
import colour
import exceptions
import random

if TYPE_CHECKING:
    from engine import Engine
    from entity import Actor, Entity, Item
# An actor action
class Action:
    def __init__(self, entity: Actor) -> None:
        super().__init__()
        self.entity = entity

    @property
    def engine(self) -> Engine:
        # Return the engine this action belongs to.
        return self.entity.gamemap.engine

    def perform(self) -> None:
        """Perform this action with the objects needed to determine its scope.
        `self.engine` is the scope this action is being performed in.
        `self.entity` is the object performing the action.
        This method must be overridden by Action subclasses."""
        raise NotImplementedError()

# Picking up items
class PickupAction(Action):
    #Pickup an item and add it to the inventory, if there is room for it.

    def __init__(self, entity: Actor):
        super().__init__(entity)

    def perform(self) -> None:
        actor_location_x = self.entity.x
        actor_location_y = self.entity.y
        inventory = self.entity.inventory

        for item in self.engine.game_map.items:
            if actor_location_x == item.x and actor_location_y == item.y:
                if len(inventory.items) >= inventory.capacity:
                    raise exceptions.Impossible("Your inventory is full.")

                self.engine.game_map.entities.remove(item)
                item.parent = self.entity.inventory
                inventory.items.append(item)

                self.engine.message_log.add_message(f"You picked up the {item.name}!")
                return

        raise exceptions.Impossible("There is nothing here to pick up.")

# An action using an item
class ItemAction(Action):
    def __init__(
        self, entity: Actor, item: Item, target_xy: Optional[Tuple[int, int]] = None
    ):
        super().__init__(entity)
        self.item = item
        if not target_xy:
            target_xy = entity.x, entity.y
        self.target_xy = target_xy

    @property
    def target_actor(self) -> Optional[Actor]:
        #Return the actor at this actions destination.
        return self.engine.game_map.get_actor_at_location(*self.target_xy)

    def perform(self) -> None:
        #Invoke the items ability, this action will be given to provide context.
        if self.item.consumable:
            self.item.consumable.activate(self)

# Drop an item
class DropItem(ItemAction):
    def perform(self) -> None:
        if self.entity.equipment.item_is_equipped(self.item):
            self.entity.equipment.toggle_equip(self.item)
        self.entity.inventory.drop(self.item)
# Wait a turn
class WaitAction(Action):
    def perform(self) -> None:
        # Only the player is meant to heal whilst waiting
        if self.entity.name == "Player":
            heal_chance = random.random()
            if heal_chance > 0.85:
                self.entity.fighter.hp += 1


        pass

# Equips an item
class EquipAction(Action):
    def __init__(self, entity: Actor, item: Item):
        super().__init__(entity)

        self.item = item

    def perform(self) -> None:
        self.entity.equipment.toggle_equip(self.item)


# Take the stairs if present
class TakeStairsAction(Action):
    def perform(self) -> None:

        if (self.entity.x, self.entity.y) == self.engine.game_map.downstairs_location:
            self.engine.game_world.generate_floor()
            self.engine.message_log.add_message(
                "You descend the staircase.", colour.descend
            )
        else:
            raise exceptions.Impossible("There are no stairs here.")

# Attempt to move to a square
class ActionWithDirection(Action):
    def __init__(self, entity: Actor, dx: int, dy: int):
        super().__init__(entity)

        self.dx = dx
        self.dy = dy

    @property
    def dest_xy(self) -> Tuple[int, int]:
        # Returns this actions destination.
        return self.entity.x + self.dx, self.entity.y + self.dy

    @property
    def blocking_entity(self) -> Optional[Entity]:
        # Return the blocking entity at this actions destination.
        return self.engine.game_map.get_blocking_entity_at_location(*self.dest_xy)

    @property
    def target_actor(self) -> Optional[Actor]:
        # Return the actor at this actions destination.
        return self.engine.game_map.get_actor_at_location(*self.dest_xy)

    def perform(self) -> None:
        raise NotImplementedError()


class MeleeAction(ActionWithDirection):
    def perform(self) -> None:
        target = self.target_actor
        if not target:
            raise exceptions.Impossible("Nothing to attack.")

        damage = self.entity.fighter.power - target.fighter.defense

        attack_desc = f"{self.entity.name.capitalize()} attacks {target.name}"
        if self.entity is self.engine.player:
            attack_colour = colour.player_atk
        else:
            attack_colour = colour.enemy_atk
        if damage > 0:
            if damage > target.fighter.max_hp :
                # Prevents the player from one-hitting mobs
                if self.entity is self.engine.player:
                    damage = target.fighter.max_hp - 1
                    self.engine.message_log.add_message(
                        f"{attack_desc} for {damage} hit points.", attack_colour
                    )

            else:
                self.engine.message_log.add_message(
                    f"{attack_desc} for {damage} hit points.", attack_colour
                )

            target.fighter.hp -= damage
        else:
            # Player attacks are ineffective against high armour
            if self.entity is self.engine.player:
                # Adding variation to blocked attacks
                # Approximately 33% chance for each
                text_choice = random.random()
                if text_choice <= 0.35:
                    self.engine.message_log.add_message(
                        f"{target.name} dodges the attack!.", attack_colour
                    )
                elif text_choice <= 0.70:
                    self.engine.message_log.add_message(
                        f"{target.name} parries the attack!.", attack_colour
                    )
                else:
                    self.engine.message_log.add_message(
                        f"{target.name} blocks the attack!.", attack_colour
                    )
            # Enemies can pierce for at least 1 damage if player's defence exceeds their damage
            else:
                # Calculating NPC's hit chance if the attack connects
                hit_chance = random.random()
                dodge_chance = (target.fighter.defense - self.entity.fighter.power)

                if hit_chance > (dodge_chance * 0.05):
                    target.fighter.hp -= 1
                    self.engine.message_log.add_message(
                        f"{attack_desc} for 1 hit points.", attack_colour
                    )
                else:
                    # Adding variation to blocked attacks
                    # Approximately 33% chance for each
                    text_choice = random.random()
                    if text_choice <= 0.35:
                        self.engine.message_log.add_message(
                            f"{target.name} dodges the attack!.", attack_colour
                        )
                    elif text_choice <= 0.70:
                        self.engine.message_log.add_message(
                            f"{target.name} parries the attack!.", attack_colour
                        )
                    else:
                        self.engine.message_log.add_message(
                            f"{target.name} blocks the attack!.", attack_colour
                        )



class MovementAction(ActionWithDirection):

    def perform(self) -> None:
        dest_x, dest_y = self.dest_xy
        if not self.engine.game_map.in_bounds(dest_x, dest_y):
            # Destination is out of bounds.
            raise exceptions.Impossible("That way is blocked.")
        if not self.engine.game_map.tiles["walkable"][dest_x, dest_y]:
            # Destination is blocked by a tile.
            raise exceptions.Impossible("That way is blocked.")
        if self.engine.game_map.get_blocking_entity_at_location(dest_x, dest_y):
            # Destination is blocked by an entity.
            raise exceptions.Impossible("That way is blocked.")
        self.entity.move(self.dx, self.dy)
        # Only the player is meant to heal whilst waiting
        if self.entity.name == "Player":
            heal_chance = random.random()
            if heal_chance > 0.95:
                self.entity.fighter.hp += 1





# When the player bumps into an entity do this
class BumpAction(ActionWithDirection):
    def perform(self) -> None:
        if self.target_actor:
            return MeleeAction(self.entity, self.dx, self.dy).perform()

        else:
            return MovementAction(self.entity, self.dx, self.dy).perform()