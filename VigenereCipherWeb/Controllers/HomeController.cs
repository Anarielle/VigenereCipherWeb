using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System.Text;
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

        private static CipherViewModel cipher = new CipherViewModel();

        public IActionResult Index()
        {
            return View(cipher);
        }

        public IActionResult Crypt(CipherViewModel getData)
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

        private static string Encrypt(CipherViewModel cipher)
        {
            string[] textArr = cipher.Text.Split(' ');
            int row;
            int collumn;
            int countLength = 0;
            StringBuilder newText = new StringBuilder();
            StringBuilder newKey = new StringBuilder();

            foreach (var item in cipher.Key)
            {
                if (cipher.alphabet.Contains(Char.ToLower(item)))
                {
                    newKey.Append(item);
                }
            }
            
            for (int i = 0; i < textArr.Length; i++)
            {
                char[] word = textArr[i].ToCharArray();
                for (int j = 0; j < word.Length; j++)
                {
                    if (cipher.alphabet.Contains(Char.ToLower(word[j])))
                    {
                        collumn = cipher.alphabet.IndexOf(Char.ToLower(word[j]));
                        row = cipher.alphabet.IndexOf(Char.ToLower(newKey.ToString()[countLength % newKey.ToString().Length]));
                        if (char.IsUpper(word[j]))
                        {
                            newText.Append(Char.ToUpper(cipher.vigenerSquare[row][collumn]));
                        }
                        else
                        {
                            newText.Append(Char.ToLower(cipher.vigenerSquare[row][collumn]));
                        }
                        countLength++;
                    }
                    else
                    {
                        newText.Append(word[j]);
                    }
                }
                if (i != textArr.Length - 1)
                {
                    newText.Append(' ');
                }
            }

            return newText.ToString();
        }

        private static string Decrypt(CipherViewModel cipher)
        {
            string[] textArr = cipher.Text.Split(' ');
            int row;
            int collumn;
            int countLength = 0;
            StringBuilder newText = new StringBuilder();
            StringBuilder newKey = new StringBuilder();

            foreach (var item in cipher.Key)
            {
                if (cipher.alphabet.Contains(Char.ToLower(item)))
                {
                    newKey.Append(item);
                }
            }

            for (int i = 0; i < textArr.Length; i++)
            {
                char[] word = textArr[i].ToCharArray();
                for (int j = 0; j < word.Length; j++)
                {
                    if (cipher.alphabet.Contains(Char.ToLower(word[j])))
                    {
                        row = cipher.alphabet.IndexOf(Char.ToLower(newKey.ToString()[countLength % newKey.ToString().Length]));
                        collumn = cipher.vigenerSquare[row].IndexOf(Char.ToLower(word[j]));
                        if (char.IsUpper(word[j]))
                        {
                            newText.Append(Char.ToUpper(cipher.alphabet[collumn]));
                        }
                        else
                        {
                            newText.Append(Char.ToLower(cipher.alphabet[collumn]));
                        }
                        countLength++;
                    }
                    else
                    {
                        newText.Append(word[j]);
                    }
                }
                if (i != textArr.Length - 1)
                {
                    newText.Append(' ');
                }
            }
            return newText.ToString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}