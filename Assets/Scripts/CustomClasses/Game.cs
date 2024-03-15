using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tyler
{
    public class Game : MonoBehaviour
    {
        public GameObject Console;
        public TextMeshProUGUI MessageText;
        private string mess;
        private float life;

        public void Message(string mess, float life)
        {
            StartCoroutine(ShowMessage(mess, life));
        }

        private IEnumerator ShowMessage(string mes, float life)
        {
            Console.SetActive(true);
            MessageText.text = mes;
            yield return new WaitForSeconds(life);
            Console.SetActive(false);
            MessageText.text = "";
        }

    }

}
