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

        // Parse text and remove tags (existing code remains the same)
        for (int i = 0; i < text.Length; )
        {
            if (i + 2 < text.Length && text[i] == '<' && text[i+1] == 'b' && text[i+2] == '>')
            {
                insideBoldTag = true;
                i += 3;
                continue;
            }
            else if (i + 3 < text.Length && text[i] == '<' && text[i+1] == '/' && text[i+2] == 'b' && text[i+3] == '>')
            {
                insideBoldTag = false;
                i += 4;
                continue;
            }

            char c = text[i];
            cleanText.Append(c);
            boldFlags.Add(insideBoldTag);
            i++;
        }

        string processedText = cleanText.ToString();
        
        // Create first word container
        GameObject currentWordContainer = CreateWordContainer();
        float currentLineWidth = 0f;
        float maxWidth = gridContainer.GetComponent<RectTransform>().rect.width;
        float letterWidth = letterBoxPrefab.GetComponent<LayoutElement>().preferredWidth;

        for (int i = 0; i < processedText.Length; i++)
        {
            char c = processedText[i];
            
            if (c == ' ')
            {
                // Only create new container if current word has letters
                if (currentWordContainer.transform.childCount > 0)
                {
                    currentWordContainer = CreateWordContainer();
                    currentLineWidth = 0f;
                }
                continue;
            }

            // Check if we need a new line
            float charWidth = letterWidth + 5f; // Include spacing
            if (currentLineWidth + charWidth > maxWidth && currentWordContainer.transform.childCount > 0)
            {
                currentWordContainer = CreateWordContainer();
                currentLineWidth = 0f;
            }

            GameObject newBox = Instantiate(letterBoxPrefab, currentWordContainer.transform);
            SetupLetterBox(newBox, c, boldFlags[i]);
            currentLineWidth += charWidth;
            
            yield return new WaitForEndOfFrame();
        }
    }

    GameObject CreateWordContainer()
    {
        GameObject wordContainer = new GameObject("WordContainer");
        wordContainer.transform.localScale = Vector3.one;
        wordContainer.transform.SetParent(gridContainer);
        
        // Add layout components
        var hlg = wordContainer.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 5f;
        
        var fitter = wordContainer.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        return wordContainer;
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