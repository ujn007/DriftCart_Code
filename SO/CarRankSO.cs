using UnityEngine;

[CreateAssetMenu (menuName = "SO/Rank")]
public class CarRankSO : ScriptableObject
{
    public int rank;
    public int point;

    private void OnEnable()
    {
        ResetInfo();
    }

    public void ResetInfo()
    {
        rank = 0;
        point = 0;
    }
}
