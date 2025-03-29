using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine.UI;

using System.Text;

public class LevelGenerator : MonoBehaviour
{
    // Step 1: Persian character set
    private static readonly char[] PersianCharacters = new char[] {
        'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'چ', 'ح', 'خ', 'د', 'ذ', 'ر', 'ز', 'ژ', 'س',
        'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ک', 'گ', 'ل', 'م', 'ن', 'و', 'ه', 'ی'
    };

    public GameObject letterBoxPrefab;
    public Transform gridContainer;
    public TMP_FontAsset persianFont;
    
    Dictionary<char, int> currentCipher;
    
    public void GenerateNewLevel(string inputText)
    {
        ClearGrid();
        currentCipher = GenerateCipher();
        StartCoroutine(CreateLevelRoutine(inputText));
    }

    IEnumerator CreateLevelRoutine(string text)
    {
        foreach(char c in text)
        {
            GameObject newBox = Instantiate(letterBoxPrefab, gridContainer);
            SetupLetterBox(newBox, c);
            yield return new WaitForEndOfFrame();
        }
    }

    void SetupLetterBox(GameObject box, char character)
    {
        LetterBoxController controller = box.GetComponent<LetterBoxController>();
        
        if(IsSpecialCharacter(character))
        {
            controller.SetSpecialCharacter(character.ToString());
        }
        else if(IsBoldCharacter(character)) // Implement your bold detection logic
        {
            controller.SetBoldCharacter(character.ToString());
        }
        else
        {
            controller.SetEncryptedCharacter(
                character.ToString(),
                currentCipher[character].ToString()
            );
        }
    }
    
    bool IsSpecialCharacter(char c) => !PersianCharacters.Contains(c);
    bool IsBoldCharacter(char c) => /* Your bold detection logic */;
    
    // Step 2: Cipher generator (as a private method)
    private Dictionary<char, int> GenerateCipher()
    {
        var cipher = new Dictionary<char, int>();
        var shuffled = PersianCharacters.OrderBy(c => Random.value).ToArray();
        
        for(int i = 0; i < shuffled.Length; i++)
        {
            cipher[shuffled[i]] = i + 1;
        }
        return cipher;
    }

    
}