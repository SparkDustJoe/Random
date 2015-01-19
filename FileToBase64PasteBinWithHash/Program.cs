using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FileToBase64PasteBinWithHash
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string cmdLine = Environment.CommandLine + " ";
            if (cmdLine.Contains(" -help ") || cmdLine.Contains(" -h ") || 
                cmdLine.Contains(" /help ") || cmdLine.Contains(" /h ") || 
                cmdLine.Contains(" -? ") || cmdLine.Contains(" /? "))
            {
                MessageBox.Show("To use this program with command-line arguments:\r\n\r\n" +
                    "Use flag -encode or -e to open in Encode mode directly.\r\n" +
                    "Use flag -decode or -d to open in Decode mode directly.\r\n" +
                    "Encode and Decode are mutually exclusive!\r\n" +
                    "Use flag -compress or -c to compress source document first (ignored in Decode mode)\r\n"+
                    "Use flag -source \"filename\" (quotes are required!) to indicate the source filename.\r\n" +
                    "Use flag -destination \"path only\" (again, quotes are required!) to indicate the destination path.\r\n" +
                    "  The original filename specified in the source XML will be used as the filename when decoding,\r\n"+
                    "   and a default extension of .b64.txt will be added when encoding.\r\n"+
                    "Use flag -description \"plain text\" to set a description for the file (outer quotes required, no inner quotes allowed!)\r\n"+
                    "  Any HTML or XML tags will be neutered (no <'s or >'s allowed!)\r\n"+
                    "  255 character limit, longer descriptions will be truncated.\r\n"+
                    "Use flag -o or -overwrite to specify that if the destination exists, overwrite it without warning.\r\n"+
                    "  This flag is implied if -silent is specified.\r\n"+
                    "Use flag -auto to perform the action automatically (-e/-d, -source are required\r\n" +
                    "  when using the -auto flag).  The source dir will be used if -destination isn't specified.\r\n" +
                    "Use flag -autoexit to perform the same action as -auto but close the program regardless of the outcome.\r\n" +
                    "  Errors will be reported, but the program will exit when the user closes the dialog.\r\n"+
                    "Use flag -s or -silent to suppress ALL messages (including errors or problems, only recommended when batching).\r\n"+
                    "  This will suppress any messages to allow -autoexit to close the program without ANY user prompts.\r\n"+
                    "  This flag will not force processing if there are major errors, but will force an overwrite to the destination!\r\n"+
                    "  USE WITH CAUTION!  Only recommended with batching!"
                    , "COMMAND LINE HELP");
                     
                Application.Exit();
            }
            else if (cmdLine.Contains(" -encode ") || cmdLine.Contains(" -e "))
                Application.Run(new frmExecute(frmExecute.Direction.Encode));
            else if (cmdLine.Contains(" -decode ") || cmdLine.Contains(" -d "))
                Application.Run(new frmExecute(frmExecute.Direction.Decode));
            else
                Application.Run(new frmDecide());
        }
    }
}
