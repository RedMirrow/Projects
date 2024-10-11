# To enable the prioritization of which object gets
# Rendered over others
# auto() function works with Python 3.6 and above
from enum import auto, Enum


class RenderOrder(Enum):
    CORPSE = auto()
    ITEM = auto()
    ACTOR = auto()