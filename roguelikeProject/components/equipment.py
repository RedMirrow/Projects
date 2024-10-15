from __future__ import annotations

from typing import Optional, TYPE_CHECKING

from components.base_component import BaseComponent
from equipment_types import EquipmentType

if TYPE_CHECKING:
    from entity import Actor, Item
    from fighter import Fighter


class Equipment(BaseComponent):
    parent: Actor

    def __init__(self, weapon: Optional[Item] = None, armour: Optional[Item] = None, ring: Optional[Item] = None, helmet: Optional[Item] = None, amulet: Optional[Item] = None):
        self.weapon = weapon # Holds the weapon used
        self.armour = armour # Holds the armour used
        self.helmet = helmet  # Holds the armour used
        self.ring = ring # Holds the ring used
        self.amulet = amulet  # Holds the necklace used

    @property
    def defense_bonus(self) -> int:
        # Defense bonus from armour worn
        bonus = 0

        if self.weapon is not None and self.weapon.equippable is not None:
            bonus += self.weapon.equippable.defense_bonus

        if self.armour is not None and self.armour.equippable is not None:
            bonus += self.armour.equippable.defense_bonus
        if self.helmet is not None and self.helmet.equippable is not None:
            bonus += self.helmet.equippable.defense_bonus
        if self.amulet is not None and self.amulet.equippable is not None:
            bonus += self.amulet.equippable.defense_bonus
        if self.ring is not None and self.ring.equippable is not None:
            bonus += self.ring.equippable.defense_bonus
        return bonus

    @property
    def power_bonus(self) -> int:
        # Attack bonus from weapon used
        bonus = 0

        if self.weapon is not None and self.weapon.equippable is not None:
            bonus += self.weapon.equippable.power_bonus

        if self.armour is not None and self.armour.equippable is not None:
            bonus += self.armour.equippable.power_bonus

        if self.helmet is not None and self.helmet.equippable is not None:
            bonus += self.helmet.equippable.power_bonus
        if self.amulet is not None and self.amulet.equippable is not None:
            bonus += self.amulet.equippable.power_bonus
        if self.ring is not None and self.ring.equippable is not None:
            bonus += self.ring.equippable.power_bonus


        return bonus

    # Checks if equipment is worn
    def item_is_equipped(self, item: Item) -> bool:
        return self.weapon == item or self.armour == item or self.ring == item or self.helmet == item or self.amulet == item

    # Unequip item message
    def unequip_message(self, item_name: str) -> None:
        self.parent.gamemap.engine.message_log.add_message(
            f"You remove the {item_name}."
        )

    # Equip item message
    def equip_message(self, item_name: str) -> None:
        self.parent.gamemap.engine.message_log.add_message(
            f"You equip the {item_name}."
        )

    # Add a chosen item to a slot
    def equip_to_slot(self, slot: str, item: Item, add_message: bool) -> None:
        current_item = getattr(self, slot)

        if current_item is not None:
            self.unequip_from_slot(slot, add_message)


        setattr(self, slot, item)

        if add_message:
            self.equip_message(item.name)

    # Takes off a chosen item from a slot
    def unequip_from_slot(self, slot: str, add_message: bool) -> None:
        current_item = getattr(self, slot)

        if add_message:
            self.unequip_message(current_item.name)

        setattr(self, slot, None)

    def toggle_equip(self, equippable_item: Item, add_message: bool = True) -> None:
        if (
            equippable_item.equippable
            and equippable_item.equippable.equipment_type == EquipmentType.WEAPON
        ):
            slot = "weapon"
        elif (
                equippable_item.equippable
                and equippable_item.equippable.equipment_type == EquipmentType.ARMOUR
        ):
            slot = "armour"
        elif (
                equippable_item.equippable
                and equippable_item.equippable.equipment_type == EquipmentType.HELMET
        ):
            slot = "helmet"
        elif (
                equippable_item.equippable
                and equippable_item.equippable.equipment_type == EquipmentType.AMULET
        ):
            slot = "amulet"
        else:
            slot = "ring"

        if getattr(self, slot) == equippable_item:
            self.unequip_from_slot(slot, add_message)
        else:
            self.equip_to_slot(slot, equippable_item, add_message)