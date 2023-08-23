public enum CurrencyType
{
    TempCurrencyAmount,
    PermCurrencyAmount,
}

public enum AbilityStatusEffect
{
    Poison,
    Rot,
    none,
}

public enum PlayerActionState
{
    IsUsingBasicAbility,
    IsUsingRangeAbility,
    IsUsingAreaAbility,
    IsDashing,
    none,
}

public enum SceneName
{
    ManagerScene,
    MainMenu,
    DemoSceneHub,
    DemoSceneDungeon,
    DemoSceneBossRoom1,
    DemoSceneBossRoom2,
}

public enum GameState
{
    MainMenu,
    GameplayHub,
    GameplayDungeon,
    PausedGameplay,
}