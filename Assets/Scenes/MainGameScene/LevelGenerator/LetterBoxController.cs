using UnityEngine;
using TMPro;

public class LetterBoxController : MonoBehaviour
{
    public TMP_Text normalLetter;
    public TMP_Text numberText;
    public TMP_Text boldLetter;

    public void SetEncryptedCharacter(string letter, string number)
    {
        normalLetter.gameObject.SetActive(true);
        numberText.gameObject.SetActive(true);
        boldLetter.gameObject.SetActive(false);
        
        normalLetter.text = letter;
        numberText.text = number;
        //
    }

    public void SetBoldCharacter(string letter)
    {
        boldLetter.gameObject.SetActive(true);
        normalLetter.gameObject.SetActive(false);
        numberText.gameObject.SetActive(false);
        boldLetter.text = letter;
    }

    public void SetSpecialCharacter(string symbol)
    {
        normalLetter.gameObject.SetActive(true);
        numberText.gameObject.SetActive(false);
        boldLetter.gameObject.SetActive(false);
        normalLetter.text = symbol;
    }
}