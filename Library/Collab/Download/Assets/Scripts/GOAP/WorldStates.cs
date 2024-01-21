using System.Collections.Generic;

public class WorldStates
{
    private Dictionary<States, int> states;

    public WorldStates() => states = new Dictionary<States, int>();

    public Dictionary<States, int> States => states;

    public bool HasState(States key) => states.ContainsKey(key);

    public void ModifyState(States key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;

            if (states[key] <= 0) RemoveState(key);
        }
        else states.Add(key, value);
    }

    public void RemoveState(States key)
    {
        if (states.ContainsKey(key)) states.Remove(key);
    }
}
