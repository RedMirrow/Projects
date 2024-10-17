from __future__ import annotations

from typing import TYPE_CHECKING

from components.base_component import BaseComponent
from equipment_types import EquipmentType

if TYPE_CHECKING:
    from entity import Item


class Equippable(BaseComponent):
    parent: Item

    def __init__(
        self,
        equipment_type: EquipmentType,
        power_bonus: int = 0,
        defense_bonus: int = 0,
    ):
        self.equipment_type = equipment_type

        self.power_bonus = power_bonus
        self.defense_bonus = defense_bonus

# WEAPONS
class Dagger(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, power_bonus=1)

class Hatchet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, power_bonus=2)

class ShortSwordAndShield(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, power_bonus=2, defense_bonus=1)

class Sword(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, power_bonus=3)

class Axe(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, power_bonus=3)

class SwordAndGreatShield(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, power_bonus=3, defense_bonus=2)

class Greatsword(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, power_bonus=4)
class GreaterShield(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.WEAPON, defense_bonus=4, power_bonus=-1)

# ARMOUR - BODY
class cloths(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.ARMOUR, defense_bonus=1)

class LeatherArmour(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.ARMOUR, defense_bonus=2)


class StrongLeatherArmour(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.ARMOUR, defense_bonus=2, power_bonus=1)

class ChainMail(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.ARMOUR, defense_bonus=3)
class StrongChainMail(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.ARMOUR, defense_bonus=3, power_bonus=1)

# HELMETS
class LeatherCap(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.HELMET, defense_bonus=1)

class FeatherCap(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.HELMET, power_bonus=1, defense_bonus=1)

class AgileCap(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.HELMET, defense_bonus=2)

class ChainCoif(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.HELMET, defense_bonus=3)

class FullPlateHelmet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.HELMET, defense_bonus=4)

# RINGS
# Tier 1
class StrengthRing(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.RING, power_bonus=1)
class HardenedRing(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.RING, defense_bonus=1)
# Tier 2
class PowerRing(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.RING, power_bonus=2)
class ShieldingRing(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.RING, defense_bonus=2)
# Tier 3
class RecklessRing(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.RING, power_bonus=4, defense_bonus=-3)
class TurtleRing(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.RING, defense_bonus=3, power_bonus=-3)

# NECKLACES
# Tier 1
class StrengthAmulet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.AMULET, power_bonus=1)
class AgileAmulet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.AMULET, defense_bonus=1)
# Tier 2
class PowerAmulet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.AMULET, power_bonus=2)
class SteelAmulet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.AMULET, defense_bonus=2)
# Tier 3
class BerserkAmulet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.AMULET, power_bonus=3)
class KnightsAmulet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.AMULET, defense_bonus=3, power_bonus=-1)
class TitansAmulet(Equippable):
    def __init__(self) -> None:
        super().__init__(equipment_type=EquipmentType.AMULET,  defense_bonus=2, power_bonus=2)
