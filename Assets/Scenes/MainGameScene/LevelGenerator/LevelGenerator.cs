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
    private bool insideBoldTag = false; // Track if we're inside <b> tags
    
    public void GenerateNewLevel(string inputText)
    {
        ClearGrid();
        currentCipher = GenerateCipher();
        StartCoroutine(CreateLevelRoutine(inputText));
    }

    IEnumerator CreateLevelRoutine(string text)
    {
        StringBuilder cleanText = new StringBuilder();
        List<bool> boldFlags = new List<bool>();

        // First parse the text to detect bold regions and remove tags
        for (int i = 0; i < text.Length; )
        {
            if (i + 2 < text.Length && text[i] == '<')
            {
                insideBoldTag = true;
                i += 3; // Skip past <b>
                continue;
            }
            else if (i + 3 < text.Length && text[i+3] == '>')
            {
                insideBoldTag = false;
                i += 4; // Skip past </b>
                continue;
            }

            // Only process non-tag characters
            char c = text[i];
            cleanText.Append(c);
            boldFlags.Add(insideBoldTag);
            i++;
        }

        // Now create letter boxes with proper bold flags
        string processedText = cleanText.ToString();
        for (int i = 0; i < processedText.Length; i++)
        {
            char c = processedText[i];
            GameObject newBox = Instantiate(letterBoxPrefab);
            SetupLetterBox(newBox, c, boldFlags[i]);
            yield return new WaitForEndOfFrame();
        }
    }

    void SetupLetterBox(GameObject box, char character, bool isBold)
    {
        LetterBoxController controller = box.GetComponent<LetterBoxController>();
        
        if(character == ' ')
        {
            controller.SetEmptyCharacter(character.ToString());
        }
        else if(IsSpecialCharacter(character))
        {
            controller.SetSpecialCharacter(character.ToString());
        }
        else if(isBold) // Use the precomputed bold flag
        {
            controller.SetBoldCharacter(character.ToString());
        }
        else if (char.IsDigit(character))
        {
            controller.SetEncryptedCharacter(
                character.ToString(),
                character.ToString()
            );
        }
        else
        {
            controller.SetEncryptedCharacter(
                character.ToString(),
                currentCipher[character].ToString()
            );
        }
    }

    private void ClearGrid()
    {
        // Optimized clearing that works even while iterating
        while (gridContainer.childCount > 0)
        {
            DestroyImmediate(gridContainer.GetChild(0).gameObject);
        }
    }
    
    bool IsSpecialCharacter(char c) => !PersianCharacters.Contains(c);
    
    // Step 2: Cipher generator
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