using UnityEngine;

public class TestLevel : MonoBehaviour
{
    public LevelGenerator generator;
    public string testText = "این یک *مثال* است";
    
    void Start()
    {
        generator.GenerateNewLevel(testText);
    }
}
