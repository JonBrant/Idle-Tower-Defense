public class SharedData 
{
    public GameSettings Settings { get; private set; }

    public void InitDefaultValues(GameSettings inputSettings)
    {
        // ToDo: Load default values from ScriptableObject or something
        Settings = inputSettings;
    }
}
