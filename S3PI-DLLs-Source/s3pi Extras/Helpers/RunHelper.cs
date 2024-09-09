/***************************************************************************
 *  Copyright (C) 2009 by Peter L Jones                                    *
 *  pljones@users.sf.net                                                   *
 *                                                                         *
 *  This file is part of the Sims 3 Package Interface (s3pi)               *
 *                                                                         *
 *  s3pi is free software: you can redistribute it and/or modify           *
 *  it under the terms of the GNU General Public License as published by   *
 *  the Free Software Foundation, either version 3 of the License, or      *
 *  (at your option) any later version.                                    *
 *                                                                         *
 *  s3pi is distributed in the hope that it will be useful,                *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of         *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the          *
 *  GNU General Public License for more details.                           *
 *                                                                         *
 *  You should have received a copy of the GNU General Public License      *
 *  along with s3pi.  If not, see <http://www.gnu.org/licenses/>.          *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using System.Windows.Forms;

namespace s3pi.Helpers
{
    public interface IRunHelper
    {
        byte[] Result { get; }
    }
    public static class RunHelper
    {
        public static int Run(Type mainForm, params string[] args)
        {
            bool useClipboard = false;
            bool useFile = false;
            List<string> files = new List<string>();

            if (!typeof(Form).IsAssignableFrom(mainForm) || !typeof(IRunHelper).IsAssignableFrom(mainForm))
            {
                CopyableMessageBox.Show("Invalid call to RunHelper.Run",
                    "Fail", CopyableMessageBoxButtons.OK, CopyableMessageBoxIcon.Stop);
                return -1;
            }

            List<char> switchChars = new List<char>(new char[] { '/', '-', });
            switchChars.Remove(Path.DirectorySeparatorChar);
            foreach (string s in args)
            {
                string p = s;
                if (p.Length > 1 && switchChars.Contains(p[0]))
                {
                    if ("clipboard".StartsWith(p.Substring(1).ToLower()))
                        useClipboard = true;
                    else
                    {
                        CopyableMessageBox.Show(String.Format("Unrecognised switch: \"{0}\"", p),
                            "Fail", CopyableMessageBoxButtons.OK, CopyableMessageBoxIcon.Stop);
                        return 1;
                    }
                }
                else
                    files.Add(s);
            }

            if (useClipboard && files.Count > 0)
            {
                CopyableMessageBox.Show("Do not use /Clipboard with other arguments",
                    "Fail", CopyableMessageBoxButtons.OK, CopyableMessageBoxIcon.Stop);
                return 2;
            }
            if (files.Count > 1)
            {
                CopyableMessageBox.Show("Only pass a single file argument",
                    "Fail", CopyableMessageBoxButtons.OK, CopyableMessageBoxIcon.Stop);
                return 3;
            }

            useFile = files.Count > 0;
            useClipboard = !useFile;

            Stream ms;

            if (useClipboard)
            {
                ms = Clipboard.GetData(DataFormats.Serializable) as MemoryStream;
                if (ms == null)
                {
                    CopyableMessageBox.Show("Invalid clipboard content",
                        "Fail", CopyableMessageBoxButtons.OK, CopyableMessageBoxIcon.Stop);
                    return 4;
                }
                Clipboard.Clear();
            }
            else
            {
                try
                {
                    ms = File.Open(files[0], FileMode.Open, FileAccess.ReadWrite);
                }
                catch (Exception ex)
                {
                    CopyableMessageBox.IssueException(ex, files[0] + "\n" + mainForm.Assembly.FullName, "Failed to open file");
                    return -1;
                }
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            byte[] result = null;
            try
            {
                Form theForm = (Form)mainForm.GetConstructor(new Type[] { typeof(Stream), }).Invoke(new object[] { ms, });
                Environment.ExitCode = 1;
                Application.Run(theForm);
                if (Environment.ExitCode != 0)
                    return 0;

                result = ((IRunHelper)theForm).Result;
                if (result == null)
                    return 0;
            }
            catch (Exception ex)
            {
                CopyableMessageBox.IssueException(ex, mainForm.Assembly.FullName, "Program exception");
                return -1;
            }

            if (useClipboard)
            {
                Clipboard.SetData(DataFormats.Serializable, new MemoryStream(result));
            }
            else
            {
                ms.Position = 0;
                ms.SetLength(0);
                ms.Write(result, 0, result.Length);
            }

            return 0;
        }
    }
}
