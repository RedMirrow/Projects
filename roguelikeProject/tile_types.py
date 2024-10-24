from operator import index
from random import choice
from typing import Tuple
import colour
import numpy as np  # type: ignore

# Tile graphics structured type compatible with Console.tiles_rgb.
graphic_dt = np.dtype(
    [
        ("ch", np.int32),  # Unicode codepoint.
        ("fg", "3B"),  # 3 unsigned bytes, for RGB colors.
        ("bg", "3B"),
    ]
)

# Tile struct used for statically defined tile data.
tile_dt = np.dtype(
    [
        ("walkable", np.bool),  # True if this tile can be walked over.
        ("transparent", np.bool),  # True if this tile doesn't block FOV.
        ("dark", graphic_dt),  # Graphics for when this tile is not in FOV.
        ("light", graphic_dt),  # Graphics for when the tile is in FOV.
    ]
)


def new_tile(
    *,  # Enforce the use of keywords, so that parameter order doesn't matter.
    walkable: int, # the tile can be walked on
    transparent: int, # does the tile block the field of view
    dark: Tuple[int, Tuple[int, int, int], Tuple[int, int, int]],
    # graphics for when not in field of view
    light: Tuple[int, Tuple[int, int, int], Tuple[int, int, int]],
) -> np.ndarray:
    # Helper function for defining individual tile types
    return np.array((walkable, transparent, dark, light), dtype=tile_dt)

# SHROUD represents unexplored, unseen tiles
SHROUD = np.array((ord(" "), (255, 255, 255), (0, 0, 0)), dtype=graphic_dt)


down_stairs = new_tile(
    walkable=True,
    transparent=True,
    dark=(ord(">"), (100, 0, 100), (0, 0, 0)),
    light=(ord(">"), (255, 255, 255), (0, 0, 0)),
)
floor = new_tile(
            walkable=True,
            transparent=True,
            dark=(ord("."), (128, 128, 128), (0, 0, 0)),
            light=(ord("."), (255, 255, 255), (0, 0, 0))

        )
wall = new_tile(
            walkable=False,
            transparent=False,
            dark=(ord("#"), (128, 128, 128), (0, 0, 0)),
            light=(ord("#"), (255, 255, 255), (0, 0, 0)),

        )

