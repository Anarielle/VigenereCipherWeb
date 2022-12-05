using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using VigenereCipherWeb.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VigenereCipherWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static Cipher cipher = new Cipher();

        public IActionResult Index()
        {
            return View(cipher);
        }

        public IActionResult Crypt(Cipher getData)
        {
            cipher.Text = getData.Text;
            cipher.Key = getData.Key;

            if (getData.IsEncrypt)
            {
                cipher.Result = Encrypt(getData);
            }
            else
            {
                cipher.Result = Decrypt(getData);
            }

            return RedirectToAction(nameof(Index));
        }

        private static string Encrypt(Cipher cipher)
        {
            string[] textArr = cipher.Text.Split(' ');
            int row;
            int collumn;
            int countLength = 0;
            string newText = "";
            string newKey = "";

            foreach (var item in cipher.Key)
            {
                if (Cipher.alphabet.Contains(Char.ToLower(item)))
                {
                    newKey += item;
                }
            }

            for (int i = 0; i < textArr.Length; i++)
            {
                char[] word = textArr[i].ToCharArray();
                for (int j = 0; j < word.Length; j++)
                {
                    if (Cipher.alphabet.Contains(Char.ToLower(word[j])))
                    {
                        collumn = Cipher.alphabet.IndexOf(Char.ToLower(word[j]));
                        row = Cipher.alphabet.IndexOf(Char.ToLower(newKey[countLength % newKey.Length]));
                        if (char.IsUpper(word[j]))
                        {
                            newText += Char.ToUpper(cipher.vigenerSquare[row][collumn]);
                        }
                        else
                        {
                            newText += Char.ToLower(cipher.vigenerSquare[row][collumn]);
                        }
                        countLength++;
                    }
                    else
                    {
                        newText += word[j];
                    }
                }
                if (i != textArr.Length - 1)
                {
                    newText += ' ';
                }
            }

            return newText;
        }

        private static string Decrypt(Cipher cipher)
        {
            string[] textArr = cipher.Text.Split(' ');
            int row;
            int collumn;
            int countLength = 0;
            string newText = "";
            string newKey = "";

            foreach (var item in cipher.Key)
            {
                if (Cipher.alphabet.Contains(Char.ToLower(item)))
                {
                    newKey += item;
                }
            }

            for (int i = 0; i < textArr.Length; i++)
            {
                char[] word = textArr[i].ToCharArray();
                for (int j = 0; j < word.Length; j++)
                {
                    if (Cipher.alphabet.Contains(Char.ToLower(word[j])))
                    {
                        row = Cipher.alphabet.IndexOf(Char.ToLower(newKey[countLength % newKey.Length]));
                        collumn = cipher.vigenerSquare[row].IndexOf(Char.ToLower(word[j]));
                        if (char.IsUpper(word[j]))
                        {
                            newText += Char.ToUpper(Cipher.alphabet[collumn]);
                        }
                        else
                        {
                            newText += Char.ToLower(Cipher.alphabet[collumn]);
                        }
                        countLength++;
                    }
                    else
                    {
                        newText += word[j];
                    }
                }
                if (i != textArr.Length - 1)
                {
                    newText += ' ';
                }
            }
            return newText;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}