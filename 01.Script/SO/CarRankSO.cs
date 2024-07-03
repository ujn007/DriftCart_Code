using UnityEngine;

[CreateAssetMenu (menuName = "SO/Rank")]
public class CarRankSO : ScriptableObject
{
    public int rank;
    public int point;

    private void OnEnable()
    {
        rank = 0;
    }
}
