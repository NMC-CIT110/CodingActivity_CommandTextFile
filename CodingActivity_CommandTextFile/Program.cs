using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;
using System.IO;

namespace CodingActivity_TextFile
{
    class Program
    {
        #region GLOBALS
        private enum FinchCommand
        {
            DONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            DELAY,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            TEMPERATURE,
            AVERAGELIGHT
        }

        private const int NUMBER_OF_COMMNANDS = 6;
        private const int DELAY_DURATION = 2000;
        private const int MOTOR_SPEED = 100;
        private const int LED_BRIGHTNESS = 200;

        #endregion

        static void Main(string[] args)
        {
            List<FinchCommand> commands;
            Finch myFinch = new Finch();

            DisplayWelcomeScreen();

            InitializeFinch(myFinch);

            commands = ReadFinchCommands();

            ProcessFinchCommands(myFinch, commands);

            TerminateFinch(myFinch);

            DisplayClosingScreen();
        }

        /// <summary>
        /// Turn the cursor off and display a continue prompt to the user
        /// </summary>
        private static void DisplayContinuePrompt()
        {
            Console.WriteLine();

            //
            // turn cursor off
            //
            Console.CursorVisible = false;

            Console.Write("Press any key to continue.");
            Console.ReadKey();

            //
            // turn cursor on
            //
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Display a welcome screen including the purpose of the application
        /// </summary>
        private static void DisplayWelcomeScreen()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Welcome to the Command Array Application");
            Console.WriteLine("Author: John E Velis");
            Console.WriteLine();
            Console.WriteLine("The application will read a series of commands for the");
            Console.WriteLine("Finch robot from a data file.");
            Console.WriteLine();
            Console.WriteLine("The Finch robot will then execute each of the commands");
            Console.WriteLine("for the Finch robot from a data file.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Display a closing screen
        /// </summary>
        private static void DisplayClosingScreen()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Thank you for using the Command Array Application");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Initialize the Finch robot
        /// </summary>
        private static void InitializeFinch(Finch myFinch)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Attempting to connect to the Finch robot.");
            Console.WriteLine();

            myFinch.connect();

            //
            // Audio/visual feedback to user
            //
            //for (int increment = 0; increment < 255; increment += 10)
            //{
            //    myFinch.setLED(0, increment, 0);
            //    //myFinch.noteOn(increment * 100);
            //    myFinch.wait(200);
            //}
            //myFinch.setLED(0, 0, 0);
            //myFinch.noteOff();

            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("The Finch robot is now connected.");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Shut down the Finch robot
        /// </summary>
        /// <param name="myFinch"></param>
        private static void TerminateFinch(Finch myFinch)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Attempting to disconnect from the Finch robot.");
            Console.WriteLine();

            //
            // Audio/visual feedback to user
            //
            //for (int increment = 255; increment > 0; increment -= 10)
            //{
            //    myFinch.setLED(0, increment, 0);
            //    myFinch.noteOn(increment * 100);
            //    myFinch.wait(200);
            //}
            //myFinch.setLED(0, 0, 0);
            //myFinch.noteOff();

            myFinch.disConnect();

            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("The Finch robot is now disconnected.");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Read the commands from a data file and add them to an array
        /// </summary>
        /// <param name="commands">array of FinchCommand</param>
        private static List<FinchCommand> ReadFinchCommands()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Reading Command Sequence From Data File");
            Console.WriteLine();

            //
            // Declare a list of FinchCommands and a FinchCommand variable
            //
            List<FinchCommand> commands = new List<FinchCommand>();
            FinchCommand command;

            //
            // Declare StreamReader object
            //
            StreamReader myReader = null;

            //
            // Declare a string variable to hold each line as it is read from the file
            //
            string readLineData;

            //
            // Use a try-catch block to catch file I/O errors
            //
            try
            {
                //
                // Instantiate the StreamReader object providing command file to read
                //
                myReader = new StreamReader("FinchCommands01.txt");

                //
                // Read the first line of the data file
                //
                readLineData = myReader.ReadLine();

                //
                // Continue reading the data file lines until the end of file is reached
                //
                while (readLineData != null)
                {
                    if (Enum.TryParse<FinchCommand>(readLineData, out command))
                    {
                        commands.Add(command);
                        Console.WriteLine("Finch Command: {0}", command);
                    }
                    else
                    {
                        throw new Exception("\nInvalid command encountered in Commands text file.");
                    }

                    //
                    // Read the next line of the data file
                    //
                    readLineData = myReader.ReadLine();
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("\nUnable to locate the text file is the current folder.");
            }
            catch (Exception ex)
            {
                commands = null;
                Console.WriteLine("\n" + ex.Message);
            }
            finally
            {
                //use finally block to be sure the file is always closed prior to exit
                if (myReader != null)
                    myReader.Close();
            }

            DisplayContinuePrompt();

            return commands;
        }

        /// <summary>
        /// Process each command 
        /// </summary>
        /// <param name="myFinch">Finch robot object</param>
        /// <param name="commands">FinchCommand</param>
        private static void ProcessFinchCommands(Finch myFinch, List<FinchCommand> commands)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("The application will now process your command sequence.");
            Console.WriteLine();

            foreach (FinchCommand command in commands)
            {
                Console.WriteLine("Command Currently Executing: " + command.ToString());

                switch (command)
                {
                    case FinchCommand.DONE:
                        Environment.Exit(1);
                        break;
                    case FinchCommand.MOVEFORWARD:
                        myFinch.setMotors(MOTOR_SPEED, MOTOR_SPEED);
                        break;
                    case FinchCommand.MOVEBACKWARD:
                        myFinch.setMotors(-MOTOR_SPEED, -MOTOR_SPEED);
                        break;
                    case FinchCommand.STOPMOTORS:
                        myFinch.setMotors(0, 0);
                        break;
                    case FinchCommand.DELAY:
                        myFinch.wait(DELAY_DURATION);
                        break;
                    case FinchCommand.TURNRIGHT:
                        myFinch.setMotors(MOTOR_SPEED, -MOTOR_SPEED);
                        break;
                    case FinchCommand.TURNLEFT:
                        myFinch.setMotors(-MOTOR_SPEED, MOTOR_SPEED);
                        break;
                    case FinchCommand.LEDON:
                        myFinch.setLED(LED_BRIGHTNESS, LED_BRIGHTNESS, LED_BRIGHTNESS);
                        break;
                    case FinchCommand.LEDOFF:
                        myFinch.setLED(0, 0, 0);
                        break;
                    case FinchCommand.TEMPERATURE:
                        DisplaySensorInfo("Temperature");
                        break;
                    case FinchCommand.AVERAGELIGHT:
                        myFinch.setLED(0, 0, 0);
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("The command sequence is now complete.");

            DisplayContinuePrompt();
        }

        private static void DisplaySensorInfo(string sensorName)
        {
            Console.WriteLine();
 

        }
    }
}
