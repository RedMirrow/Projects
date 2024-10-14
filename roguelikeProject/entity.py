from __future__ import annotations

import copy
import random

from typing import Optional, Tuple, Type, TypeVar, TYPE_CHECKING, Union
import math
from effect import StatusEffect
from components.hazard import HazardComponent
from render_order import RenderOrder

if TYPE_CHECKING:
    from components.ai import BaseAI
    from components.fighter import Fighter
    from components.consumable import Consumable
    from components.inventory import Inventory
    from components.equippable import Equippable
    from components.equipment import Equipment
    from game_map import GameMap
    from components.level import Level

T = TypeVar("T", bound="Entity")

class Entity:
    #A generic object to represent players, enemies, items, etc.
    parent: Union[GameMap, Inventory]
    def __init__(
            self,
            parent: Optional[GameMap] = None,
            x: int = 0,
            y: int = 0,
            char: str = "?",
            color: Tuple[int, int, int] = (255, 255, 255),
            name: str = "<Unnamed>",
            blocks_movement: bool = False,
            render_order: RenderOrder = RenderOrder.CORPSE,
    ):
        self.x = x
        self.y = y
        self.char = char
        self.color = color
        self.name = name
        self.blocks_movement = blocks_movement
        self.render_order = render_order
        self.last_position = (x, y)
        self.is_swarm = False
        self.visible = True
        if parent:
            # If parent isn't provided now then it will be set later.
            self.parent = parent
            parent.entities.add(self)

    @property
    def gamemap(self) -> GameMap:
        return self.parent.gamemap

    def spawn(self: T, gamemap: GameMap, x: int, y: int, is_swarm: Optional[bool] = False) -> T:
        # Spawn a copy of this instance at the given location.
        # Optional tag for swarm enemies
        clone = copy.deepcopy(self)
        clone.x = x
        clone.y = y
        clone.is_swarm = is_swarm
        clone.parent = gamemap
        gamemap.entities.add(clone)
        return clone
    def place(self, x: int, y: int, gamemap: Optional[GameMap] = None, is_swarm: Optional[bool] = False) -> None:
        # Place this entity at a new location.  Handles moving across GameMaps.
        self.x = x
        self.y = y
        if gamemap:
            if hasattr(self, "parent"):  # Possibly uninitialized.
                if self.parent is self.gamemap:
                    self.gamemap.entities.remove(self)
            self.parent = gamemap
            self.is_swarm = is_swarm  # If this is a swarm, then it will not give XP when it dies.
            gamemap.entities.add(self)
            self.is_swarm = False

    def move(self, dx: int, dy: int) -> None:
        # Move the entity by a given amount
        self.x += dx
        self.y += dy

    def distance(self, x: int, y: int) -> float:

        # Return the distance between the current entity and
        # the given (x, y) coordinate.
        return math.sqrt((x - self.x) ** 2 + (y - self.y) ** 2)

# A non player entity with AI and fighter components
class Actor(Entity):
    def __init__(
        self,
        *,
        x: int = 0,
        y: int = 0,
        char: str = "?",
        color: Tuple[int, int, int] = (255, 255, 255),
        name: str = "<Unnamed>",
        ai_cls: Type[BaseAI],
        fighter: Fighter,
        equipment: Equipment,
        inventory: Inventory,
        level: Level,
        effect: Optional[StatusEffect] = None,
    ):
        super().__init__(
            x=x,
            y=y,
            char=char,
            color=color,
            name=name,
            blocks_movement=True,
            render_order=RenderOrder.ACTOR,
        )

        self.ai: Optional[BaseAI] = ai_cls(self)
        self.equipment: Equipment = equipment
        self.equipment.parent = self
        self.fighter = fighter
        self.fighter.parent = self
        self.inventory = inventory
        self.inventory.parent = self
        self.level = level
        self.level.parent = self

    @property
    def is_alive(self) -> bool:
        # Returns True as long as this actor can perform actions.
        return bool(self.ai)

# Item type Entity
class Item(Entity):
    def __init__(
        self,
        *,
        x: int = 0,
        y: int = 0,
        char: str = "?",
        color: Tuple[int, int, int] = (255, 255, 255),
        name: str = "<Unnamed>",
        consumable: Optional[Consumable] = None,
        equippable: Optional[Equippable] = None,
    ):
        super().__init__(
            x=x,
            y=y,
            char=char,
            color=color,
            name=name,
            blocks_movement=False,
            render_order=RenderOrder.ITEM,
        )

        self.consumable = consumable
        if self.consumable:
            self.consumable.parent = self

        self.equippable = equippable

        if self.equippable:
            self.equippable.parent = self

class Hazard(Entity):

    # A walkable entity that can inflict a status effect to any actors steping on it,
    # and can also disapear after some time. Used for water, gases, etc.
    def __init__(
            self,
            *,
            x: int = 0,
            y: int = 0,
            char: str = "?",
            color: Tuple[int, int, int] = (255, 255, 255),
            name: str = "<Unnamed>",
            duration: int = 0,
            is_permanent: bool = False,
            moves_around: bool = False,
            hazard_component: type[HazardComponent],
    ):
        super().__init__(
            x=x,
            y=y,
            char=char,
            color=color,
            name=name,
            blocks_movement=False,
            render_order=RenderOrder.HAZARD,
        )

        self.duration = duration
        self.is_permanent = is_permanent
        self.zone_component = hazard_component(self)
        self.zone_component.parent = self
        self.moves_around = moves_around

    def on_tick_actor(self, actor: Actor) -> None:
        self.zone_component.on_actor_tick(actor)
    def on_update(self) -> None:
        if self.moves_around:
            # Pick a random direction to move to
            direction_x, direction_y = random.choice(
                [
                    (-1, -1),  # Northwest
                    (0, -1),  # North
                    (1, -1),  # Northeast
                    (-1, 0),  # West
                    (1, 0),  # East
                    (-1, 1),  # Southwest
                    (0, 1),  # South
                    (1, 1),  # Southeast
                ]
            )

            # Check if there's a walkable tile in the random direction
            if self.gamemap.is_walkable_tile(self.x + direction_x, self.y + direction_y):
                # Check if there's not another zone in the random direction.
                if not any(
                    entity
                    for entity in self.gamemap.hazards
                    if entity.x == self.x + direction_x and entity.y == self.y + direction_y
                ):
                    self.move(direction_x, direction_y)