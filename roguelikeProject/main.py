#!/usr/bin/env python3
import copy
#Import calls
import tcod
from engine import Engine
import entity_factories
from input_handlers import EventHandler
from procgen import generate_dungeon

def main() -> None:
    #Determines the size of the playscreen and used tileset
    screen_width = 80
    screen_height = 50
    map_width = 80
    map_height = 45
    room_max_size = 10
    room_min_size = 6
    max_rooms = 30
    max_monsters_per_room = 2
    tileset = tcod.tileset.load_tilesheet("tileMap.png",32,8,tcod.tileset.CHARMAP_TCOD)

    #A new event handler object is created to handle user inputs
    player = copy.deepcopy(entity_factories.player)
    engine = Engine(player=player)

    #Creates a game map with generate dungeon method
    engine.game_map = generate_dungeon(
        max_rooms=max_rooms,
        room_min_size=room_min_size,
        room_max_size=room_max_size,
        map_width=map_width,
        map_height=map_height,
        max_monsters_per_room=max_monsters_per_room,
        engine=engine,
    )
    engine.update_fov()

    #Creating the screen within the given parameters
    with tcod.context.new_terminal(
        screen_width,
        screen_height,
        tileset=tileset,
        title="Python Roguelike",
        vsync=True,
    ) as context:
        #Creating the 'terminal' for the screen
        root_console = tcod.console.Console(screen_width,screen_height,order="F")
        #Game loop
        while True:
            engine.render(console=root_console, context=context)
            engine.event_handler.handle_events()


if __name__ == "__main__":
    main()