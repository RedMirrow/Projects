from __future__ import annotations
from typing import TYPE_CHECKING
from components.base_component import BaseComponent
from effect import Poisoned

if TYPE_CHECKING:
    from entity import Actor, Hazard


class HazardComponent(BaseComponent):
    parent: Hazard

    def __init__(self, hazard: Hazard):
        self.hazard = hazard

    def on_actor_tick(self, actor: Actor) -> None:
        """
        Called when an actor is updated while in this zone.

        This method must be implemented by subclasses.
        """
        raise NotImplementedError()


class PoisonGas(HazardComponent):
    def on_actor_tick(self, actor: Actor) -> None:
        # Poison the actor standing in the gas
        if actor.fighter:
            actor.fighter.apply_status_effect(Poisoned(7, 2))