using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface // Taken from the Unity Learn example

[System.Serializable]
public struct PlayerScore : IComparable<PlayerScore>
{
    public string name;
    public int finalScore;

    public int CompareTo(PlayerScore other)
    {
        if (other.Equals(null))
        {
            return 1;
        }

        //Return the difference in score
        return finalScore - other.finalScore;
    }

}

