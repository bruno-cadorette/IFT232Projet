using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Core;

namespace Ift232UI
{
    class UiTools
    {
        public static Game Load()
        {
            OpenFileDialog window = new OpenFileDialog();
            window.Filter = "save file|*.sav";
            window.Title = "Séléctionnez le fichier de chargement.";
            var dialogResult = window.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value == true)
            {
                return Game.Load(window.FileName);
            }

            return null;
        }

    }
}
