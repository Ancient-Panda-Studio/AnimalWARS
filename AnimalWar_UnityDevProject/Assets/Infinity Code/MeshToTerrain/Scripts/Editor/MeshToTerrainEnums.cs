/*       INFINITY CODE 2013 - 2019         */
/*     http://www.infinity-code.com        */

public enum MeshToTerrainBounds
{
    autoDetect,
    fromGameobject,
    selectBounds
}

public enum MeshToTerrainDirection
{
    normal,
    reversed
}

public enum MeshToTerrainFindType
{
    gameObjects,
    layers
}

public enum MeshToTerrainHoles
{
    minimumValue,
    neighborAverage
}

public enum MeshToTerrainPhase
{
    idle,
    prepare,
    createTerrains,
    generateHeightmaps,
    generateTextures,
    finish
}

public enum MeshToTerrainSelectTerrainType
{
    existTerrains,
    newTerrains
}

public enum MeshToTerrainTextureCaptureMode
{
    camera,
    raycast
}

public enum MeshToTerrainYRange
{
    minimalRange,
    longMeshSide,
    fixedValue,
}