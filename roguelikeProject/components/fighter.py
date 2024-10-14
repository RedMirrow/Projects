from __future__ import annotations

import copy

from components.base_component import BaseComponent


from typing import TYPE_CHECKING
from render_order import RenderOrder
from effect import StatusEffect

import colour
if TYPE_CHECKING:
    from entity import Actor
class Fighter(BaseComponent):
    parent: Actor
    def __init__(self, hp: int, base_defense: int, base_power: int):
        self.status_effects = []
        self.base_max_hp = hp
        self._hp = hp
        self.max_hp = hp
        self.base_defense = base_defense
        self.base_power = base_power
        self.status_effects = []

    # Accessor for entity's hp
    @property
    def hp(self) -> int:
        return self._hp

    # When called this will heal up the entity to the maximum
    def heal(self, amount: int) -> int:
        if self.hp == self.max_hp:
            return 0

        new_hp_value = self.hp + amount

        if new_hp_value > self.max_hp:
            new_hp_value = self.max_hp

        # Calculates hp recovered for feedback
        amount_recovered = new_hp_value - self.hp
        self.hp = new_hp_value
        return amount_recovered

    def take_damage(self, amount: int) -> None:
        self.hp -= amount

    def apply_status_effect(self, effect: StatusEffect) -> None:

        for status_effect in self.status_effects:
            if status_effect.name == effect.name:
                status_effect.duration += effect.duration
                return
        self.status_effects.append(copy.deepcopy(effect))

        self.engine.message_log.add_message(f"{self.parent.name} is now {effect.name}!", colour.status_effect_applied)

    def update_status_effects(self) -> None:
        for status_effect in self.status_effects:
            status_effect.on_tick(self.parent, self.engine)
            if status_effect.duration <= 0 and status_effect in self.status_effects:
                self.status_effects.remove(status_effect)
                self.engine.message_log.add_message(f"{self.parent.name} is no longer {status_effect.name}.")

    def has_status_effect(self, effect: StatusEffect) -> bool:
        for status_effect in self.status_effects:
            if status_effect.name == effect.name:
                return True
        return False

    def take_damage(self, amount: int) -> None:
        self.hp -= amount

    @hp.setter
    def hp(self, value: int) -> None:
        self._hp = max(0, min(value, self.max_hp))
        if self._hp == 0 and self.parent.ai:
            self.die()

    @property
    def defense(self) -> int:
        return self.base_defense + self.defense_bonus

    @property
    def power(self) -> int:
        return self.base_power + self.power_bonus

    @property
    def defense_bonus(self) -> int:
        if self.parent.equipment:
            return self.parent.equipment.defense_bonus
        else:
            return 0

    @property
    def power_bonus(self) -> int:
        if self.parent.equipment:
            return self.parent.equipment.power_bonus
        else:
            return 0


    # Death function for both player and npc
    def die(self) -> None:
        if self.engine.player is self.parent:
            death_message = "You died!"
            death_message_colour = colour.player_die

        else:
            death_message = f"{self.parent.name} is dead!"
            death_message_colour = colour.enemy_die

        self.parent.char = "%"
        self.parent.color = (191, 0, 0)
        self.parent.blocks_movement = False
        self.parent.ai = None
        self.parent.name = f"remains of {self.parent.name}"
        self.parent.render_order = RenderOrder.CORPSE
        self.engine.player.level.add_xp(self.parent.level.xp_given)

        self.engine.message_log.add_message(death_message, death_message_colour)