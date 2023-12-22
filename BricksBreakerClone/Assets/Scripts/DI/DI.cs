using TinyIoC;

public static class DI
{
    public static TinyIoCContainer Container => TinyIoCContainer.Current;
    public static TinyIoCContainer Game;
    
    public static void CreateGameContainer()
    {
        Game = Container.GetChildContainer();
    }
    
    public static void DisposeOfGameContainer()
    {
        Game.Dispose();
        Game = null;
    }
}