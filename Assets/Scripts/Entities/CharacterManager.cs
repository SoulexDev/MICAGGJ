using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public List<Character> characters = new List<Character>();

    private void Awake()
    {
        Instance = this;
    }
    public void AddCharacter(Character c)
    {
        if (c && !characters.Contains(c))
            characters.Add(c);
    }
    public void RemoveCharacter(Character c)
    {
        if (c && characters.Contains(c))
            characters.Remove(c);
    }
}