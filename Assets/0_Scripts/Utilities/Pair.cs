[System.Serializable]
public class Pair<TK, TV>
{
    public TK Key;
    public TV Value;

    public Pair(TK key, TV val)
    {
        this.Key = key;
        this.Value = val;
    }
}