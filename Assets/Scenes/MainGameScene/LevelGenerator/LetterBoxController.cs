using UnityEngine;
using TMPro;
using UniversalTextBox;
using UnityEngine.UI;

public class LetterBoxController : MonoBehaviour
{
    public UTB normalLetter;
    public UTB numberText;
    public UTB boldLetter;
    public Image frame;

    public void SetEncryptedCharacter(string letter, string number)
    {
        normalLetter.gameObject.SetActive(true);
        numberText.gameObject.SetActive(true);
        boldLetter.gameObject.SetActive(false);
        
        
        normalLetter.text = letter;
        numberText.text = number;
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
    
    public void SetEmptyCharacter(string text)
    {
        frame.gameObject.SetActive(false);
        normalLetter.gameObject.SetActive(false);
        numberText.gameObject.SetActive(false);
        boldLetter.gameObject.SetActive(false);
    }
}