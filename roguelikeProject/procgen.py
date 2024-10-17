from __future__ import annotations
#Procedural generation for the map
import random
from typing import Dict, Iterator, List, Tuple, TYPE_CHECKING

import tcod

import entity_factories
from game_map import GameMap
import tile_types


if TYPE_CHECKING:
    from engine import Engine
    from entity import Entity

# Hard limits on items and monsters per room based on floor
max_items_by_floor = [
    (1, 1),
    (3, 2),
    (5, 3),
    (7, 4),
]

max_monsters_by_floor = [
    (1, 3),
    (4, 4),
    (6, 6),
]
# Item chance table
item_chances: Dict[int, List[Tuple[Entity, int]]] = {
    0: [
        # To represent fallen adventurers, potentially throw in starter gear
        (entity_factories.dagger,5),
        (entity_factories.cloth_armour,5),
        # Actual loot of the floors
        (entity_factories.health_potion, 60),

    ],
    1: [ # Upgrades available - weapon, armour,
        (entity_factories.weaken_scroll, 20),
        (entity_factories.hatchet,5),
        (entity_factories.leather_armour,5),
    ],
    2: [ # Upgrades available - weapon, armour, helmet,
        (entity_factories.confusion_scroll, 10),

        (entity_factories.hatchet, 10),
        (entity_factories.leather_armour, 10),
        (entity_factories.leather_cap, 10),
    ],
    3: [ # Upgrades available - weapon, armour, helmet, Tier 1 ring

        (entity_factories.strength_scroll, 5),
        (entity_factories.greater_health_potion, 30),

        (entity_factories.sword,5),
        (entity_factories.axe,5),
        (entity_factories.swordAndShield,5),

        (entity_factories.hatchet, 15),
        (entity_factories.leather_armour, 15),
        (entity_factories.strong_leather_armour, 5),
        (entity_factories.leather_cap, 10),
        (entity_factories.featherCap, 5),
        (entity_factories.agileCap, 5),

        (entity_factories.strengthRing,5),
        (entity_factories.hardRing,5),

    ],
    4: [ # Upgrades available - weapon, armour, helmet, Tier 1 ring,  Tier 1 necklace
        # To show that many fell by the 3rd floor
        (entity_factories.dagger,0),
        (entity_factories.cloth_armour,0),
        (entity_factories.hatchet, 2),
        (entity_factories.leather_armour, 2),
        (entity_factories.leather_cap, 2),
        # Actual loot of the floor
        (entity_factories.lightning_scroll, 25),
        (entity_factories.sword,10),
        (entity_factories.axe,10),
        (entity_factories.swordAndShield,10),

        (entity_factories.featherCap, 5),
        (entity_factories.agileCap, 5),

        (entity_factories.strengthAmulet,5),
        (entity_factories.agileAmulet,5),
    ],
    5:[ # Upgrades available - weapon, armour, helmet, Tier 1, 2 ring, Tier 1 necklace
        #Low tier loot filter
        (entity_factories.hatchet, 0),
        (entity_factories.leather_armour, 0),
        (entity_factories.leather_cap, 0),
        (entity_factories.strengthRing,2),
        (entity_factories.hardRing,2),
        (entity_factories.strengthAmulet,2),
        (entity_factories.agileAmulet,2),

        # Actual loot
        (entity_factories.powerRing,5),
        (entity_factories.shieldRing,5),
        (entity_factories.powerAmulet,5),
        (entity_factories.steelAmulet,5),
        (entity_factories.swordAndBigShield,15),

        (entity_factories.chain_mail,10),
        (entity_factories.chainCoif,10),

    ],

    6: [ # Upgrades available - weapon, armour, helmet, Tier 2 ring, Tier 2 necklace
        #Low tier loot filter
        (entity_factories.hardRing,0),
        (entity_factories.strengthAmulet,0),
        (entity_factories.agileAmulet,0),
        (entity_factories.health_potion, 0),

        # Actual loot
        (entity_factories.greater_health_potion, 95),
        (entity_factories.fireball_scroll, 25),
        (entity_factories.greatsword,5),
        (entity_factories.towershield,5),
    ],

    7: [  # Upgrades available - weapon, armour, helmet, Tier 3 ring, Tier 3 necklace
        # Low tier loot filter

        (entity_factories.fireball_scroll, 30),
        (entity_factories.fireblast_scroll, 10),
        (entity_factories.greatsword,10),
        (entity_factories.towershield, 10),
        (entity_factories.knightHelm, 10),
        (entity_factories.strong_chain_mail, 5),

        (entity_factories.titansAmulet, 5),
        (entity_factories.berserkAmulet, 5),
        (entity_factories.knightsAmulet, 5),
        (entity_factories.recklessRing, 5),
        (entity_factories.turtleRing, 5),

    ],

}

# Enemy chance
enemy_chances: Dict[int, List[Tuple[Entity, int]]] = {
    0: [
        (entity_factories.rat, 80),
        (entity_factories.hound, 30),
        (entity_factories.skeleton, 30)
        ],
    1: [
        (entity_factories.mama_slime, 10)
    ],
    2: [
        (entity_factories.rat, 40),
        (entity_factories.lostSoul, 10),
        (entity_factories.skeleton, 60)
    ],
    3: [
        (entity_factories.orc, 15),
        (entity_factories.mama_bile, 25),
        (entity_factories.skeleton, 80),
        (entity_factories.rat, 5),
        (entity_factories.hound, 10),
        (entity_factories.mama_slime, 20)
    ],
    5: [
        (entity_factories.lostSoul, 5),
        (entity_factories.greatLostSoul, 20),
        (entity_factories.orc, 30),
        (entity_factories.troll, 5),
        (entity_factories.skeleton, 60),
        (entity_factories.rat, 0),
        (entity_factories.hound, 5),
    ],
    6:[
        (entity_factories.skeleton, 5),
        (entity_factories.houndTindalos, 10),
    ],
    7: [
        (entity_factories.orc, 60),
        (entity_factories.troll, 15),
        (entity_factories.houndTindalos, 30),
        (entity_factories.skeletonKnight, 20),
    ],
}

# Get the limit values
def get_max_value_for_floor(
    max_value_by_floor: List[Tuple[int, int]], floor: int
) -> int:
    current_value = 0

    for floor_minimum, value in max_value_by_floor:
        if floor_minimum > floor:
            break
        else:
            current_value = value

    return current_value

# Randomly chooses items/monsters based on weights
def get_entities_at_random(
    weighted_chances_by_floor: Dict[int, List[Tuple[Entity, int]]],
    number_of_entities: int,
    floor: int,
) -> List[Entity]:
    entity_weighted_chances = {}

    for key, values in weighted_chances_by_floor.items():
        if key > floor:
            break
        else:
            for value in values:
                entity = value[0]
                weighted_chance = value[1]

                entity_weighted_chances[entity] = weighted_chance

    entities = list(entity_weighted_chances.keys())
    entity_weighted_chance_values = list(entity_weighted_chances.values())

    chosen_entities = random.choices(
        entities, weights=entity_weighted_chance_values, k=number_of_entities
    )

    return chosen_entities

class RectangularRoom:
    def __init__(self, x: int, y: int, width: int, height: int):
        self.x1 = x
        self.y1 = y
        self.x2 = x + width
        self.y2 = y + height

    @property
    def center(self) -> Tuple[int, int]:
        center_x = int((self.x1 + self.x2) / 2)
        center_y = int((self.y1 + self.y2) / 2)

        return center_x, center_y

    @property
    def inner(self) -> Tuple[slice, slice]:
        # Return the inner area of this room as a 2D array index.
        return slice(self.x1 + 1, self.x2), slice(self.y1 + 1, self.y2)

    # Checks whether 2 rooms intersect
    def intersects(self, other: RectangularRoom) -> bool:
        # Return True if this room overlaps with another RectangularRoom.
        return (
                self.x1 <= other.x2
                and self.x2 >= other.x1
                and self.y1 <= other.y2
                and self.y2 >= other.y1
        )
def place_entities(room: RectangularRoom, dungeon: GameMap, floor_number: int,) -> None:
    number_of_monsters = random.randint(
        0, get_max_value_for_floor(max_monsters_by_floor, floor_number)
    )
    number_of_items = random.randint(
        0, get_max_value_for_floor(max_items_by_floor, floor_number)
    )

    monsters: List[Entity] = get_entities_at_random(
        enemy_chances, number_of_monsters, floor_number
    )
    items: List[Entity] = get_entities_at_random(
        item_chances, number_of_items, floor_number
    )
    for entity in monsters + items:
        x = random.randint(room.x1 + 1, room.x2 - 1)
        y = random.randint(room.y1 + 1, room.y2 - 1)

        if not any(entity.x == x and entity.y == y for entity in dungeon.entities):
            entity.spawn(dungeon, x, y)

# Create a tunnel between the 2 rooms
def tunnel_between(
    start: Tuple[int, int], end: Tuple[int, int]
) -> Iterator[Tuple[int, int]]:
    # Return an L-shaped tunnel between these two points.
    x1, y1 = start
    x2, y2 = end
    if random.random() < 0.5:  # 50% chance.
        # Move horizontally, then vertically.
        corner_x, corner_y = x2, y1
    else:
        # Move vertically, then horizontally.
        corner_x, corner_y = x1, y2

    # Generate the coordinates for this tunnel.
    for x, y in tcod.los.bresenham((x1, y1), (corner_x, corner_y)).tolist():
        yield x, y
    for x, y in tcod.los.bresenham((corner_x, corner_y), (x2, y2)).tolist():
        yield x, y

def generate_dungeon(
    max_rooms: int,
    room_min_size: int,
    room_max_size: int,
    map_width: int,
    map_height: int,
    engine: Engine,
) -> GameMap:
    # Generate a new dungeon map.
    player = engine.player
    dungeon = GameMap(engine, map_width, map_height, entities=[player])

    rooms: List[RectangularRoom] = []
    center_of_last_room = (0, 0)

    for r in range(max_rooms):
        room_width = random.randint(room_min_size, room_max_size)
        room_height = random.randint(room_min_size, room_max_size)

        x = random.randint(0, dungeon.width - room_width - 1)
        y = random.randint(0, dungeon.height - room_height - 1)

        # "RectangularRoom" class makes rectangles easier to work with
        new_room = RectangularRoom(x, y, room_width, room_height)

        # Run through the other rooms and see if they intersect with this one.
        if any(new_room.intersects(other_room) for other_room in rooms):
            continue  # This room intersects, so go to the next attempt.
        # If there are no intersections then the room is valid.

        # Dig out this rooms inner area.
        dungeon.tiles[new_room.inner] = tile_types.floor

        if len(rooms) == 0:
            # The first room, where the player starts.
            player.place(*new_room.center, dungeon)
        else:  # All rooms after the first.
            # Dig out a tunnel between this room and the previous one.
            for x, y in tunnel_between(rooms[-1].center, new_room.center):
                dungeon.tiles[x, y] = tile_types.floor
        center_of_last_room = new_room.center
        dungeon.tiles[center_of_last_room] = tile_types.down_stairs
        dungeon.downstairs_location = center_of_last_room
        place_entities(new_room, dungeon, engine.game_world.current_floor)
        # Finally, append the new room to the list.
        rooms.append(new_room)

    return dungeon