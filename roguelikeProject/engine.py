#Engine class used to minimise bloat of main.py

from __future__ import annotations

from typing import TYPE_CHECKING

from tcod.console import Console
from tcod.map import compute_fov
import exceptions

import lzma
import pickle

from message_log import MessageLog
import render_functions


if TYPE_CHECKING:
    from entity import Actor
    from game_map import GameMap, GameWorld



class Engine:
    game_map: GameMap
    game_world: GameWorld

    def __init__(self, player: Actor):

        self.message_log = MessageLog()
        self.mouse_location = (0, 0)
        self.player = player

    def handle_enemy_turns(self) -> None:
        for entity in set(self.game_map.actors) - {self.player}:
            if entity.ai:
                # The ai entity will try to perform
                try:
                    entity.ai.perform()
                except exceptions.Impossible:
                    pass  # Ignore impossible action exceptions from AI.

    def handle_status_effects(self) -> None:
        for entity in set(self.game_map.actors):
            entity.fighter.update_status_effects()

    # Handling hazards
    def handle_hazard(self) -> None:
        hazard_to_remove = []
        for hazard in self.game_map.hazards:
            hazard.on_update()
            # Check if there's an actor in the hazard.
            for actor in self.game_map.actors:
                if hazard.x == actor.x and hazard.y == actor.y:
                    if actor.fighter and actor.fighter.hp > 0:
                        hazard.on_tick_actor(actor)

            # If the hazard is not permanent, then tick down its countdown.
            if not hazard.is_permanent:
                hazard.duration -= 1
                if hazard.duration <= 0:
                    hazard_to_remove.append(hazard)

        # Remove the hazards that are done.
        for hazard in hazard_to_remove:
            self.game_map.entities.remove(hazard)

    def update_fov(self) -> None:
        # Recompute the visible area based on the players point of view.
        self.game_map.visible[:] = compute_fov(
            self.game_map.tiles["transparent"],
            (self.player.x, self.player.y),
            radius=7,
        )
        # If a tile is "visible" it should be added to "explored".
        self.game_map.explored |= self.game_map.visible

    def render(self, console: Console) -> None:
        self.game_map.render(console)
        self.message_log.render(console=console, x=21, y=45, width=60, height=5)
        # Health bar, same thing could be done for mana later on
        render_functions.render_bar(
            console=console,
            current_value=self.player.fighter.hp,
            maximum_value=self.player.fighter.max_hp,
            total_width=20,
        )

        # States which floor player is on
        render_functions.render_dungeon_level(
            console=console,
            dungeon_level=self.game_world.current_floor,
            location=(0, 47),
        )
        # Describes what the mouse pointer is hovering at
        render_functions.render_names_at_mouse_location(
            console=console, x=21, y=44, engine=self
        )

    # Saving the game
    def save_as(self, filename: str) -> None:
        """Save this Engine instance as a compressed file."""
        save_data = lzma.compress(pickle.dumps(self))
        with open(filename, "wb") as f:
            f.write(save_data)