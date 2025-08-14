using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
   public void LoadCredits()
   {
      SceneManager.LoadScene("Credits");
   }
   
   public void ReturnToMenu()
   {
      SceneManager.LoadScene("MainMenu");
   }
}
