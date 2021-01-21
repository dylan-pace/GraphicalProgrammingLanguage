using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment2
{
    /// <summary>
    /// Winform that adds functionality to the buttons and menus on the form.
    /// </summary>
    public partial class Form1 : Form
    {
        //Sets up an array to store the shapes in.
        ArrayList shapes = new ArrayList();
        //Integars used for position of the pen, measurements of the shapes.
        protected int xpos, ypos, x, y, height = 0, width = 0, radius = 0, loopTimes, loop = 0, loopLine, loopIndex;

        /// <summary>
        /// Constructor that sets up calls the method to add manufactured code.
        /// </summary>
        public Form1()
        {
            //Method to set up manufactured code.
            InitializeComponent();
        }

        /// <summary>
        /// Method that gives functionality to the exit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="r"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs r)
        {
            //Displays a message asking for confirmation to close the program.
            String message = "Exit the program?";
            String caption = "Exit";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            //Displays the exit popup.
            result = MessageBox.Show(message, caption, buttons);

            //If yes then close the program.
            if (result == DialogResult.Yes)
            {
                //Closes the program.
                Application.Exit();
            }
        }

        /// <summary>
        /// Method that gives functionality to the about button which give the user information about the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="r"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs r)
        {
            //Displays a message with information telling the user what this program will do.
            String message = "This program is used to teach people simple programming concepts using a graphical interface.";
            String caption = "About this program";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            //Displays the popup.
            result = MessageBox.Show(message, caption, buttons);
        }

        /// <summary>
        /// Adds functionality to the save button which lets the user save the block of code they have written in the textbox..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="r"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs r)
        {
            SaveFileDialog save = new SaveFileDialog();

            //If the user presses th ok button the text will be saved.
            if (save.ShowDialog() == DialogResult.OK)
            {
                //Saves the code from the textbox.
                String code = richTextBox1.Text;
                File.WriteAllText(save.FileName, code);
            }
        }

        /// <summary>
        /// Method that performs a function if the button 'Clear' is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ShapeFactory factory = new ShapeFactory();
            try
            {
                //Looks in the factory for 'clear'.
                shapes.Add(factory.getShape("clear"));

            }
            //Catches an exception if no such phrase exists in the factory.
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid shape: " + e);

            }
            //Draws a black box to the screen, resetting it back to blank.
            Shape s;
            Color newColor = Color.Black;
            s = factory.getShape("clear");
            s.set(newColor, 0, 0, 10000, 10000);
            shapes.Add(s);
            pictureBox1.Refresh();
            //Resets the x and y coordinates.
            xpos = 0;
            ypos = 0;

        }

        /// <summary>
        /// Method that allows the user to open programs they have previously written to the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="r"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs r)
        {
            //If the file cannot be opened, the program will watch the error.
            try
            {

                //Allows the user to selected a file.
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == DialogResult.OK)
                {

                    //Reads the files and save is to a string.
                    string sourceCode = File.ReadAllText(file.FileName);
                    richTextBox1.AppendText(sourceCode);

                }
            }
            //Exception for input/output errors with files.
            catch (IOException)
            {
                //Message saying that the file format chosen was usuitable for the program to open..
                String message = "Cannot open this specific file.";
                String caption = "Unable to open.";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                //Displays the pop up.
                result = MessageBox.Show(message, caption, buttons);
            }
        }

        /// <summary>
        /// Method that gives functionality to the 'Run' button, running the program the user has written in the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Sets up an array to store the user's commands in.
            string[] program = { };

            //Checks to see if there are commands in the richtextbox.
            if (richTextBox1.Text != "")
            {
                //Saves the richtextbox commands in the program array.
                program = richTextBox1.Text.Split('\n');
            }
            //Checks to see if there are commands in the text box.
            if (textBox1.Text != "")
            {
                /*Saves the text box commands in the program array, if the textbox and richtextbox both have commands in then
                  the priority will go to the 'command line' text box. */
                program = textBox1.Text.Split('\n');
            }
            //Sets up an integar that will increment through the for loop allowing the user to see which line an error is on.
            int lines = 1;
            //Sets up a new colour so they user may change it later.
            Color userColour = Color.Blue;

            //The for loop will loop through the lines of the text box running the commands given by the user, line by line.
            for (int i = 0; i < program.Length; i++)
            {
                try
                {
                    string ifCode = "";
                    //Converts all the commands in the textbox to lower case.
                    program = Array.ConvertAll(program, x => x.ToLower());

                    //Calls the ShapeFactory method so it can auto load the different shapes.
                    ShapeFactory factory = new ShapeFactory();

                    //Checks whether the user wants to loop their code.
                    if (program[i].Contains("loop"))
                    {
                        try
                        {
                            loopLine = i;
                            //Gets the amount of time the code needs to loop.
                            string[] loopValue = program[i].Split(' ');
                            string loopVal = loopValue[1];
                            loopTimes = Convert.ToInt32(loopVal);
                            //For loop to iterate through the program to find where the block command ends.
                            for (int z = i; z < program.Length; z++)
                            {
                                //Checks for where the loop statement ends.
                                if (program[z].Contains("stop"))
                                {
                                    //Search for the first occurrence of the duplicated value.
                                    String searchString = "stop";
                                    //Saves the array index where 'stop' occurs.
                                    loopIndex = Array.IndexOf(program, searchString, i);
                                }
                            }
                        }
                        //Exception in case there are too many or not enough parameters in the users command.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the if command needs two parameters.
                            String message = "The number of parameters for performing a loop was unsuitable. " +
                                "Error on line: " + lines;
                            String caption = "Unable to perform a loop statement.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception in case the parameters are in the wrong format.
                        catch (FormatException)
                        {
                            //Message saying that the parameters are incorrect, e.g strings instead of integars.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to perform a loop.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks whether the user has issued an if statement in their commands.
                    if (program[i].Contains("if"))
                    {
                        try
                        {
                            //Sets up a variable to save as the comparsion.
                            int vari = 0;
                            //Split the command line and assign the values to variables.
                            string[] value = program[i].Split(' ');
                            string variable = value[1];
                            string symbol = value[2];
                            string num = value[3];
                            int number = 0;
                            //Saves the variable as the value for radius.
                            if (variable == "radius")
                            {
                                vari = radius;
                            }
                            //Saves the variable as the value for height.
                            else if (variable == "height")
                            {
                                vari = height;
                            }
                            //Saves the variable as the value for width.
                            else if (variable == "width")
                            {
                                vari = width;
                            }
                            else
                            {
                                vari = Convert.ToInt32(variable);
                            }
                            if (num == "radius")
                            {
                                number = radius;
                            }
                            else if (num == "height")
                            {
                                number = height;
                            }
                            else if (num == "width")
                            {
                                number = width;
                            }
                            else
                            {
                                number = Convert.ToInt32(num);
                            }
                            //Finds the comparion symbol to see if the two values equal each other.
                            if (symbol == "=")
                            {
                                //Ensure the values are equal.
                                if (vari == number)
                                {
                                    //Checks to see if the if statement was a one line command or a block command.
                                    if (program[i].Contains("then"))
                                    {
                                        //Splits the single line command to get the commmand it is supposed to perfrom.
                                        program[i] = program[i].Split(new[] { "then " }, StringSplitOptions.None)[1];
                                    }
                                    //If the user specified a block command.
                                    else
                                    {
                                        //For loop to iterate through the program to find where the block command ends.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks for where the if statement ends.
                                            if (program[z].Contains("end"))
                                            {
                                                // Search for the first occurrence of the duplicated value.
                                                String searchString = "end";
                                                //Saves the array index where 'end' occurs.
                                                int index = Array.IndexOf(program, searchString);
                                            }
                                        }
                                    }
                                }
                                //If the number doesn't equal the variable.
                                else
                                {
                                    //If the number isn't equal to the variable but still contains 'then'.
                                    if (program[i].Contains("then"))
                                    {
                                        //Checks to see if the next index is empty in the program array.
                                        string nextIndex = program[i + 1];
                                        if (nextIndex != null)
                                        {
                                            //If it's not empty then just skip the if statement as it is false.
                                            i = i + 1;
                                        }

                                    }
                                    //If it is a block if statement where the variable isn't equal to the number.
                                    else
                                    {
                                        //Iterate through a new loop for the program array starting from the current index.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks to see where the end of the if statement is.
                                            if (program[z].Contains("end"))
                                            {
                                                //Search for the occurrence of 'end'.
                                                String searchString = "end";
                                                int index = Array.IndexOf(program, searchString, i);

                                                //Skips what's in the if statement and move to end as the comparison was false.
                                                i = index;
                                            }
                                        }
                                    }
                                }
                            }
                            //If the variable is greater than the number.
                            else if (symbol == ">")
                            {
                                if (vari > number)
                                {
                                    //Checks to see if the if statement was a one line command or a block command.
                                    if (program[i].Contains("then"))
                                    {
                                        //Splits the single line command to get the commmand it is supposed to perfrom.
                                        program[i] = program[i].Split(new[] { "then " }, StringSplitOptions.None)[1];
                                    }
                                    //If the user specified a block command.
                                    else
                                    {
                                        //For loop to iterate through the program to find where the block command ends.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks for where the if statement ends.
                                            if (program[z].Contains("end"))
                                            {
                                                // Search for the first occurrence of the duplicated value.
                                                String searchString = "end";
                                                //Saves the array index where 'end' occurs.
                                                int index = Array.IndexOf(program, searchString);
                                            }
                                        }
                                    }
                                }
                                //If the number doesn't equal the variable.
                                else
                                {
                                    //If the number isn't equal to the variable but still contains 'then'.
                                    if (program[i].Contains("then"))
                                    {
                                        //Checks to see if the next index is empty in the program array.
                                        string nextIndex = program[i + 1];
                                        if (nextIndex != null)
                                        {
                                            //If it's not empty then just skip the if statement as it is false.
                                            i = i + 1;
                                        }

                                    }
                                    //If it is a block if statement where the variable isn't equal to the number.
                                    else
                                    {
                                        //Iterate through a new loop for the program array starting from the current index.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks to see where the end of the if statement is.
                                            if (program[z].Contains("end"))
                                            {
                                                //Search for the occurrence of 'end'.
                                                String searchString = "end";
                                                int index = Array.IndexOf(program, searchString, i);

                                                //Skips what's in the if statement and move to end as the comparison was false.
                                                i = index;
                                            }
                                        }
                                    }
                                }
                            }
                            //If the variable is less than the number.
                            else if (symbol == "<")
                            {
                                if (vari < number)
                                {
                                    //Checks to see if the if statement was a one line command or a block command.
                                    if (program[i].Contains("then"))
                                    {
                                        //Splits the single line command to get the commmand it is supposed to perfrom.
                                        program[i] = program[i].Split(new[] { "then " }, StringSplitOptions.None)[1];
                                    }
                                    //If the user specified a block command.
                                    else
                                    {
                                        //For loop to iterate through the program to find where the block command ends.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks for where the if statement ends.
                                            if (program[z].Contains("end"))
                                            {
                                                // Search for the first occurrence of the duplicated value.
                                                String searchString = "end";
                                                //Saves the array index where 'end' occurs.
                                                int index = Array.IndexOf(program, searchString);
                                            }
                                        }
                                    }
                                }
                                //If the number doesn't equal the variable.
                                else
                                {
                                    //If the number isn't equal to the variable but still contains 'then'.
                                    if (program[i].Contains("then"))
                                    {
                                        //Checks to see if the next index is empty in the program array.
                                        string nextIndex = program[i + 1];
                                        if (nextIndex != null)
                                        {
                                            //If it's not empty then just skip the if statement as it is false.
                                            i = i + 1;
                                        }

                                    }
                                    //If it is a block if statement where the variable isn't equal to the number.
                                    else
                                    {
                                        //Iterate through a new loop for the program array starting from the current index.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks to see where the end of the if statement is.
                                            if (program[z].Contains("end"))
                                            {
                                                //Search for the occurrence of 'end'.
                                                String searchString = "end";
                                                int index = Array.IndexOf(program, searchString, i);

                                                //Skips what's in the if statement and move to end as the comparison was false.
                                                i = index;
                                            }
                                        }
                                    }
                                }
                            }
                            //If the variable is less than or equal to the number.
                            else if (symbol == "<=")
                            {
                                if (vari <= number)
                                {
                                    //Checks to see if the if statement was a one line command or a block command.
                                    if (program[i].Contains("then"))
                                    {
                                        //Splits the single line command to get the commmand it is supposed to perfrom.
                                        program[i] = program[i].Split(new[] { "then " }, StringSplitOptions.None)[1];
                                    }
                                    //If the user specified a block command.
                                    else
                                    {
                                        //For loop to iterate through the program to find where the block command ends.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks for where the if statement ends.
                                            if (program[z].Contains("end"))
                                            {
                                                // Search for the first occurrence of the duplicated value.
                                                String searchString = "end";
                                                //Saves the array index where 'end' occurs.
                                                int index = Array.IndexOf(program, searchString);
                                            }
                                        }
                                    }
                                }
                                //If the number doesn't equal the variable.
                                else
                                {
                                    //If the number isn't equal to the variable but still contains 'then'.
                                    if (program[i].Contains("then"))
                                    {
                                        //Checks to see if the next index is empty in the program array.
                                        string nextIndex = program[i + 1];
                                        if (nextIndex != null)
                                        {
                                            //If it's not empty then just skip the if statement as it is false.
                                            i = i + 1;
                                        }

                                    }
                                    //If it is a block if statement where the variable isn't equal to the number.
                                    else
                                    {
                                        //Iterate through a new loop for the program array starting from the current index.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks to see where the end of the if statement is.
                                            if (program[z].Contains("end"))
                                            {
                                                //Search for the occurrence of 'end'.
                                                String searchString = "end";
                                                int index = Array.IndexOf(program, searchString, i);

                                                //Skips what's in the if statement and move to end as the comparison was false.
                                                i = index;
                                            }
                                        }
                                    }
                                }
                            }
                            //If the variable is greater than or equal to the number.
                            else if (symbol == ">=")
                            {
                                if (vari >= number)
                                {
                                    //Checks to see if the if statement was a one line command or a block command.
                                    if (program[i].Contains("then"))
                                    {
                                        //Splits the single line command to get the commmand it is supposed to perfrom.
                                        program[i] = program[i].Split(new[] { "then " }, StringSplitOptions.None)[1];
                                    }
                                    //If the user specified a block command.
                                    else
                                    {
                                        //For loop to iterate through the program to find where the block command ends.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks for where the if statement ends.
                                            if (program[z].Contains("end"))
                                            {
                                                // Search for the first occurrence of the duplicated value.
                                                String searchString = "end";
                                                //Saves the array index where 'end' occurs.
                                                int index = Array.IndexOf(program, searchString);
                                            }
                                        }
                                    }
                                }
                                //If the number doesn't equal the variable.
                                else
                                {
                                    //If the number isn't equal to the variable but still contains 'then'.
                                    if (program[i].Contains("then"))
                                    {
                                        //Checks to see if the next index is empty in the program array.
                                        string nextIndex = program[i + 1];
                                        if (nextIndex != null)
                                        {
                                            //If it's not empty then just skip the if statement as it is false.
                                            i = i + 1;
                                        }

                                    }
                                    //If it is a block if statement where the variable isn't equal to the number.
                                    else
                                    {
                                        //Iterate through a new loop for the program array starting from the current index.
                                        for (int z = i; z < program.Length; z++)
                                        {
                                            //Checks to see where the end of the if statement is.
                                            if (program[z].Contains("end"))
                                            {
                                                //Search for the occurrence of 'end'.
                                                String searchString = "end";
                                                int index = Array.IndexOf(program, searchString, i);

                                                //Skips what's in the if statement and move to end as the comparison was false.
                                                i = index;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Exception in case there are too many or not enough parameters in the users command.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the if command needs two parameters.
                            String message = "The number of parameters for performing an If statement was unsuitable. " +
                                "Error on line: " + lines;
                            String caption = "Unable to perform If statement.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception in case the parameters are in the wrong format.
                        catch (FormatException)
                        {
                            //Message saying that the parameters are incorrect, e.g strings instead of integars.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to perform If statement.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks to see if the user's commands contains the word 'colour'.
                    if (program[i].Contains("colour"))
                    {
                        //Sets up a colour dialog that will pop up and allow the user to choose their own colour to draw with.
                        ColorDialog colorDlg = new ColorDialog();
                        //Enables the different options for the colour dialog.
                        colorDlg.AllowFullOpen = true;
                        colorDlg.AnyColor = true;
                        colorDlg.SolidColorOnly = false;

                        //When the user clicks 'ok' the program will continue.
                        if (colorDlg.ShowDialog() == DialogResult.OK)
                        {
                            //Changes the previously set up colour variable to the user's desired colour.
                            userColour = colorDlg.Color;
                        }
                    }

                    //Checks to see if the user entered 'drawline' into the txet box.
                    else if (program[i].Contains("drawline") || ifCode.Contains("drawline"))
                    {
                        Console.WriteLine(ifCode);
                        try
                        {
                            //Gets 'drawline' from the factory.
                            shapes.Add(factory.getShape("drawline"));

                        }
                        //Catches an exception for if the command wasn't in the factory.
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid shape: " + e);

                        }
                        //Splits the user's command to get the different x,y parameters.
                        string[] stringRec = program[i].Split(' ');
                        try
                        {
                            //Saves the x,y parameters in seperate strings the converts them to integars.
                            string hei = stringRec[1];
                            string wid = stringRec[2];
                            x = Convert.ToInt32(hei);
                            y = Convert.ToInt32(wid);
                            //Sets up and draws the line to the screen.
                            Shape s;
                            Color newColor = userColour;
                            s = factory.getShape("drawline");
                            s.set(newColor, xpos, ypos, x, y);
                            shapes.Add(s);
                            //Refreshes the picture box so the new line is visible.
                            pictureBox1.Refresh();
                            //Updates the x,y coordinates so the next shape or line will draw from here.
                            xpos = x;
                            ypos = y;
                        }
                        //Exception in case there are too many or not enough parameters in the users command.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the command needs two parameters.
                            String message = "The number of parameters for drawing a line was unsuitable. It takes two parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a line.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception in case the parameters are in the wrong format.
                        catch (FormatException)
                        {
                            //Message saying that the paraemters are incorrect, e.g strings instead of integars.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a line.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks to see if the user has entered 'movepen' in their commands.
                    else if (program[i].Contains("movepen"))
                    {
                        try
                        {
                            //Gets the movepen from the factory.
                            shapes.Add(factory.getShape("movepen"));

                        }
                        //Exception in case the movepen isn't in the factory class. 
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid shape: " + e);

                        }
                        //Splits the user's command to get the x,y parameters.
                        string[] stringRec = program[i].Split(' ');
                        try
                        {
                            //Saves the x,y parameters individually and converts them to integars.
                            string hei = stringRec[1];
                            string wid = stringRec[2];
                            x = Convert.ToInt32(hei);
                            y = Convert.ToInt32(wid);
                            //Sets up the pen and moves is across the screen to the desired location.
                            Shape s;
                            //Makes the pen transparent so nothing actually draws.
                            Color newColor = Color.Transparent;
                            s = factory.getShape("movepen");
                            s.set(newColor, xpos, ypos, x, y);
                            shapes.Add(s);
                            //Refresh the picture box to make sure the pen actually moves.
                            pictureBox1.Refresh();
                            //Updates the x,y coordinates so the next shape or line will draw from here.
                            xpos = x;
                            ypos = y;
                        }
                        //Exception for if the there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the user's command had incorrect parameters.
                            String message = "The number of parameters for moving the pen was unsuitable. It takes two parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to move the pen.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the parameters are in the wrong format of the wrong data type.
                        catch (FormatException)
                        {
                            //Message saying that the parameters shoulc be integars.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to move the pen.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks to see if the user's command contain 'circle'.
                    else if (program[i].Contains("circle"))
                    {
                        try
                        {
                            //Sets up variables for if the circle needs to be repeated.
                            int repeat = 1;
                            int increment = 0;
                            string symbol = "", rad;
                            char[] myChar = new char[0];
                            int radius1 = 10;
                            //Splits the program to get the parameters.
                            string[] stringRadius = program[i].Split(' ');
                            //If the user's command also contains repeat then the cricle needs extra variables.
                            if (program[i].Contains("repeat"))
                            {
                                //Split it further to see how many times the circle needs to repeat itself.
                                string[] stringRep = program[i].Split(' ');

                                //Saves the radius of the circle.
                                rad = stringRadius[3];
                                //Checks to see if the circle needs to add pixels.
                                if (program[i].Contains("+"))
                                {
                                    //Saves the + and makes it a char.
                                    symbol = "+";
                                    myChar = symbol.ToCharArray();
                                }
                                //Checks to see if the circle needs to subtract pixels.
                                else if (program[i].Contains("-"))
                                {
                                    //Saves the - and makes it a char.
                                    symbol = "-";
                                    myChar = symbol.ToCharArray();
                                }
                                //Checks to see if the circle needs to multiply pixels.
                                else if (program[i].Contains("*"))
                                {
                                    //Saves the * and makes it a char.
                                    symbol = "*";
                                    myChar = symbol.ToCharArray();
                                }
                                //Checks to see if the circle needs to divide pixels.
                                else if (program[i].Contains("/"))
                                {
                                    //Saves the / and makes it a char.
                                    symbol = "/";
                                    myChar = symbol.ToCharArray();
                                }
                                //If the program doesn't contain any of these then the repeat command is in the wrong order and the user will be told.
                                else
                                {
                                    //Message saying that the symbol used in the repeat command is incorrect.
                                    String message = "The mathematical symbol was unsuitable. Ensure it is an +, - *, /. " +
                                        "Error on line: " + lines;
                                    String caption = "Unable to draw a repeat rectangle.";
                                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                                    DialogResult result;

                                    //Display the dialog box.
                                    result = MessageBox.Show(message, caption, buttons);
                                }
                                //Saves the repeat command variables and converts the commands to integars.
                                string[] stringInc = program[i].Split(myChar);
                                string inc = stringInc[1];
                                string rep = stringRep[1];
                                repeat = Convert.ToInt32(rep);
                                increment = Convert.ToInt32(inc);
                            }
                            else
                            {
                                //If the circle isn't part of a repeat command, just save the radius normally.
                                rad = stringRadius[1];
                            }
                            try
                            {
                                //Gets the circle command from the factory.
                                shapes.Add(factory.getShape("circle"));

                            }
                            //Catches an exception if the shape isn't in the factory.
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Invalid shape: " + e);
                            }

                            //Checks if the radius is named as a variable or a number by the user.
                            if (rad == "radius")
                            {
                                //If a variable is in place of the number, save it to a new int.
                                radius1 = radius;
                            }
                            if (rad != "radius")
                            {
                                //If a number then convert the number they inputted to an int.
                                radius1 = Convert.ToInt32(rad);
                            }
                            /*Loops through th drawing process for the circle based on how many increments the user defined int he repeat command. If the
                             * command isn't a repeat then it will just go through once as normal.
                             */
                            for (int p = 0; p < repeat; p++)
                            {
                                //Sets up the shape and adds colour to it.
                                Shape s;
                                Color newColor = userColour;
                                s = factory.getShape("circle");
                                if (program[i].Contains("texture"))
                                {
                                    Image image = new Bitmap("wool.jpg");
                                    TextureBrush brush = new TextureBrush(image);
                                    brush.Transform = new Matrix(
                                       75.0f / 640.0f,
                                       0.0f,
                                       0.0f,
                                       75.0f / 480.0f,
                                       0.0f,
                                       0.0f);
                                    //Draws the shapes through the factory, adjusting so the circle draws from the center.
                                    s.set(brush, xpos - (radius1 / 2), ypos - (radius1 / 2), radius1);
                                }
                                else
                                {
                                    //Draws the shapes through the factory, adjusting so the circle draws from the center.
                                    s.set(newColor, xpos - (radius1 / 2), ypos - (radius1 / 2), radius1);
                                }
                                shapes.Add(s);
                                //Refresh the picture box so the circle is visible.
                                pictureBox1.Refresh();
                                //If the symbol entered by the user in the repeat command was addition than add the increment to the radius.
                                if (symbol == "+")
                                {
                                    //Adds the increment to the radius with each loop.
                                    radius1 = radius1 + increment;
                                }
                                //If the symbol entered by the user in the repeat command was subtraction than take away the increment from the radius.
                                else if (symbol == "-")
                                {
                                    //Subtract the increment from the radius.
                                    radius1 = radius1 - increment;
                                }
                                //If the symbol entered by the user in the repeat command was multiplication than times the increment with the radius.
                                else if (symbol == "*")
                                {
                                    //Mulitply the increment with the radius.
                                    radius1 = radius1 * increment;
                                }
                                //If the symbol entered by the user in the repeat command was division than divide the increment from the radius.
                                else if (symbol == "/")
                                {
                                    //Divide the increment with the radius.
                                    radius1 = radius1 / increment;
                                }
                            }
                        }
                        //Exception for if there were too many of not enough parameters for the circle.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the file format chosen was usuitable for the program to open.
                            String message = "The number of parameters for drawing a circle was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Shows the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the circle parameters were not integars.
                        catch (FormatException)
                        {
                            //Message saying that the file format chosen was usuitable for the program to open.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Shows the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }

                    }

                    //Checks the user's commands for if it contains 'rectangle'.
                    else if (program[i].Contains("rectangle"))
                    {
                        try
                        {
                            //Sets up the variables for if there is a repeat command.
                            int repeat = 1;
                            int increment = 0;
                            string symbol = "", hei, wid;
                            char[] myChar = new char[0];
                            int height1 = 10;
                            int width1 = 10;
                            //Splits the command to get the different parameters.
                            string[] stringRec = program[i].Split(' ');
                            //Checks if the rectangle command needs to be repeated.
                            if (program[i].Contains("repeat"))
                            {
                                //Splits it further for the repeat command.
                                string[] stringRep = program[i].Split(' ');

                                //Saves the hieght and width of the rectangle if it's part of a repeat command.
                                hei = stringRec[3];
                                wid = stringRec[4];
                                //Checks if the repeat command needs to add pixels between each rectangle.
                                if (program[i].Contains("+"))
                                {
                                    //Saves the + and converts to a char.
                                    symbol = "+";
                                    myChar = symbol.ToCharArray();
                                }
                                //Checks if the repeat command needs to subtract pixels between each rectangle.
                                else if (program[i].Contains("-"))
                                {
                                    //Saves the - and converts to a char.
                                    symbol = "-";
                                    myChar = symbol.ToCharArray();
                                }
                                //Checks if the repeat command needs to multiply pixels between each rectangle.
                                else if (program[i].Contains("*"))
                                {
                                    //Saves the * and converts to a char.
                                    symbol = "*";
                                    myChar = symbol.ToCharArray();
                                }
                                //Checks if the repeat command needs to divide pixels between each rectangle.
                                else if (program[i].Contains("/"))
                                {
                                    //Saves the / and converts to a char.
                                    symbol = "/";
                                    myChar = symbol.ToCharArray();
                                }
                                //If it contains none of these then the repeat command won't work and it will tell the user.
                                else
                                {
                                    //Message saying that the symbol is incorrect and the rectangle can't repeat.
                                    String message = "The mathematical symbol was unsuitable. Ensure it is an +, - *, /. " +
                                        "Error on line: " + lines;
                                    String caption = "Unable to draw a repeat rectangle.";
                                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                                    DialogResult result;

                                    //Shows the dialog box.
                                    result = MessageBox.Show(message, caption, buttons);
                                }
                                //Saves the repeat variables and converts them to integars.
                                string[] stringInc = program[i].Split(myChar);
                                string inc = stringInc[1];
                                string rep = stringRep[1];
                                repeat = Convert.ToInt32(rep);
                                increment = Convert.ToInt32(inc);
                            }
                            else
                            {
                                //If there is no repeat command then save the rectangle's height and width normally.
                                hei = stringRec[1];
                                wid = stringRec[2];
                            }
                            try
                            {
                                //Get the rectangle functions from the factory.
                                shapes.Add(factory.getShape("rectangle"));

                            }
                            //Exception for if rectangle cannot be found in the factory.
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Invalid shape: " + e);

                            }

                            //Checks to see if the height and width parameters use variables, then orders them.
                            if (hei == "height")
                            {
                                height1 = height;
                            }
                            if (hei == "width")
                            {
                                height1 = width;
                            }
                            if (wid == "height")
                            {
                                width1 = height;
                            }
                            if (wid == "width")
                            {
                                width1 = width;
                            }
                            //If height and width were entered as numbers, it just converts them to integars.
                            if (height1 != height && width1 != height)
                            {
                                //Converts the height to an integar.
                                height1 = Convert.ToInt32(hei);

                            }
                            if (width1 != width && height1 != width)
                            {
                                //Converts the width to an integar.
                                width1 = Convert.ToInt32(wid);
                            }
                            //If the repeat command was set then it will loop through the drawing process as many times as specified.
                            for (int p = 0; p < repeat; p++)
                            {
                                Shape s;
                                //Sets the colour of the rectangle to the one the user specified.
                                Color newColor = userColour;
                                s = factory.getShape("rectangle");
                                //If the command contains the repeat command, it will draw the rectangle from the center.
                                if (height1 == width && width1 == height && program[i].Contains("repeat"))
                                {
                                    //Passes through the variables and draws the shape to screen.
                                    s.set(newColor, xpos - (width1 / 2), ypos - (height1 / 2), height1, width1);
                                    shapes.Add(s);
                                    //Refreshes the picture box so the rectangle is visible.
                                    pictureBox1.Refresh();

                                }
                                else if (height1 != width && width1 != height && program[i].Contains("repeat"))
                                {
                                    //Passes through the variables and draws the shape to screen.
                                    s.set(newColor, xpos - (width1 / 2), ypos - (height1 / 2), width1, height1);
                                    shapes.Add(s);
                                    //Refreshes the picture box so the rectangle is visible.
                                    pictureBox1.Refresh();
                                }
                                //If there was no repeat command, the rectangle will be drawn from the top left.
                                else if (height1 == width && width1 == height)
                                {
                                    //Passes through the variables and draws the shape to screen.
                                    s.set(newColor, xpos, ypos, height1, width1);
                                    shapes.Add(s);
                                    //Refreshes the picture box so the rectangle is visible.
                                    pictureBox1.Refresh();
                                }
                                else if (height1 == width1 && width1 == height1)
                                {
                                    //Passes through the variables and draws the shape to screen.
                                    s.set(newColor, xpos, ypos, height1, width1);
                                    shapes.Add(s);
                                    //Refreshes the picture box so the rectangle is visible.
                                    pictureBox1.Refresh();
                                }
                                else if (height1 != width && width1 != height)
                                {
                                    //Passes through the variables and draws the shape to screen.
                                    s.set(newColor, xpos, ypos, width1, height1);
                                    shapes.Add(s);
                                    //Refreshes the picture box so the rectangle is visible.
                                    pictureBox1.Refresh();
                                }
                                //Uses the + symbol to find the pixel distance between the repeating rectangle.
                                if (symbol == "+")
                                {
                                    //Increments the rectangles pixels.
                                    height1 = height1 + increment;
                                    width1 = width1 + increment;
                                }
                                //Uses the - symbol to find the pixel distance between the repeating rectangle.
                                else if (symbol == "-")
                                {
                                    //Increments the rectangles pixels.
                                    height1 = height1 - increment;
                                    width1 = width1 - increment;
                                }
                                //Uses the * symbol to find the pixel distance between the repeating rectangle.
                                else if (symbol == "*")
                                {
                                    //Increments the rectangles pixels.
                                    height1 = height1 * increment;
                                    width1 = width1 * increment;
                                }
                                //Uses the / symbol to find the pixel distance between the repeating rectangle.
                                else if (symbol == "/")
                                {
                                    //Increments the rectangles pixels.
                                    height1 = height1 / increment;
                                    width1 = width1 / increment;
                                }
                            }
                        }
                        //Catches the exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the rectangle needs two parameters to draw.
                            String message = "The number of parameters for drawing a rectangle was unsuitable. It takes two parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a rectangle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the parameters were the incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the parameters should be integars.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a rectangle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks to see whether the user inputted triangle into their command.
                    else if (program[i].Contains("triangle"))
                    {

                        try
                        {
                            //Gets the triangle class from the factory.
                            shapes.Add(factory.getShape("triangle"));

                        }
                        //Exception for if triangle wasn't in the factory.
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid shape: " + e);

                        }
                        try
                        {
                            //Set up the variables used to draw the triangle.
                            int hyp = 10;
                            int triBase = 10;
                            int adj = 10;
                            //Split the user's command to get the triangle parameters.
                            string[] stringTri = program[i].Split(' ');
                            string hei = stringTri[1];
                            string wid = stringTri[2];
                            string bas = stringTri[3];
                            //Convert the parameters to integars.
                            hyp = Convert.ToInt32(hei);
                            triBase = Convert.ToInt32(wid);
                            adj = Convert.ToInt32(bas);
                            Shape s;
                            //Change the colour to the user's selected colour.
                            Color newColor = userColour;
                            s = factory.getShape("triangle");
                            //Pass through the parameters and draw the triangle.
                            s.set(newColor, xpos, ypos, hyp, triBase, adj);
                            shapes.Add(s);
                            //Refresh the picture box to make the drawn triangle visible.
                            pictureBox1.Refresh();
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the triangle requires three parameters.
                            String message = "The number of parameters for drawing a triangle was unsuitable. It takes three parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a triangle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception saying the paramters for the triangle are incorrect.
                        catch (FormatException)
                        {
                            //Message saying that the parameters must be integars.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a triangle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks to see whether the user inputted triangle into their command.
                    else if (program[i].Contains("pentagon"))
                    {

                        try
                        {
                            //Gets the triangle class from the factory.
                            shapes.Add(factory.getShape("pentagon"));

                        }
                        //Exception for if triangle wasn't in the factory.
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid shape: " + e);

                        }
                        try
                        {
                            //Set up the variables used to draw the triangle.
                            int p1 = 10;
                            int p2 = 10;
                            int p3 = 10;
                            int p4 = 10;
                            //Split the user's command to get the triangle parameters.
                            string[] stringTri = program[i].Split(' ');
                            string hei = stringTri[1];
                            string wid = stringTri[2];
                            string bas = stringTri[3];
                            string srt = stringTri[4];
                            //Convert the parameters to integars.
                            p1 = Convert.ToInt32(hei);
                            p2 = Convert.ToInt32(wid);
                            p3 = Convert.ToInt32(bas);
                            p4 = Convert.ToInt32(srt);
                            Shape s;
                            //Change the colour to the user's selected colour.
                            Color newColor = userColour;
                            s = factory.getShape("pentagon");
                            //Pass through the parameters and draw the triangle.
                            s.set(newColor, xpos, ypos, p1, p2, p3, p4);
                            shapes.Add(s);
                            //Refresh the picture box to make the drawn triangle visible.
                            pictureBox1.Refresh();
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the triangle requires three parameters.
                            String message = "The number of parameters for drawing a pentagon was unsuitable. It takes four parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a pentagon.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception saying the paramters for the triangle are incorrect.
                        catch (FormatException)
                        {
                            //Message saying that the parameters must be integars.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a pentagon.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks the user's command for the shape square.
                    else if (program[i].Contains("square"))
                    {

                        try
                        {
                            //Get the shape from the factory.
                            shapes.Add(factory.getShape("square"));

                        }
                        //Exception for if the square shape couldn't be found in the fatory.
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid shape: " + e);

                        }
                        try
                        {
                            //Sets up the variables for square.
                            int sides = 0;
                            //Splits the parameter for the square's sides.
                            string[] stringSqu = program[i].Split(' ');
                            string side = stringSqu[1];
                            //Converts the side to an integar.
                            sides = Convert.ToInt32(side);
                            Shape s;
                            //Makes the square the user's desired colour.
                            Color newColor = userColour;
                            s = factory.getShape("square");
                            //Passes through the parameter and draws the square.
                            s.set(newColor, xpos, ypos, sides, sides);
                            shapes.Add(s);
                            //Refreshes the picture box to makes the square visible on the screen.
                            pictureBox1.Refresh();
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the square shape requires one parameter.
                            String message = "The number of parameters for drawing a square was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the paramter was the wrong data type.
                        catch (FormatException)
                        {
                            //Message saying that the square parameter needs to be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the height variable in their program.
                    else if (program[i].Contains("height=") || program[i].Contains("height ="))
                    {
                        try
                        {
                            //Splits the user's command to get the height.
                            string[] value = program[i].Split('=');
                            string hei = value[1];
                            //Converts the height to an integar and saves it so it may be used in conjunction with different shapes.
                            height = Convert.ToInt32(hei);
                        }
                        //Exception for if there are too many or not enough paramters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the height variable only needs one paramter.
                            String message = "The number of parameters for saving the height variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception saying that the variable parameter is the incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the file format chosen was usuitable for the program to open..
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the radius variable.
                    else if (program[i].Contains("height+") || program[i].Contains("height + "))
                    {
                        try
                        {
                            //Splits the command to get to the variable's parameter.
                            string[] value = program[i].Split('+');
                            string rad = value[1];
                            //Converts the parameter to an integar.
                            int addRad = Convert.ToInt32(rad);
                            height = height + addRad;
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the radius variable needs only one parameter.
                            String message = "The number of parameters for the radius variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the data type of the radius variable must be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the radius variable.
                    else if (program[i].Contains("height-") || program[i].Contains("height - "))
                    {
                        try
                        {
                            //Splits the command to get to the variable's parameter.
                            string[] value = program[i].Split('+');
                            string rad = value[1];
                            //Converts the parameter to an integar.
                            int addRad = Convert.ToInt32(rad);
                            height = height - addRad;
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the radius variable needs only one parameter.
                            String message = "The number of parameters for the radius variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the data type of the radius variable must be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }


                    //Checks to see if the user defined the width variable.
                    else if (program[i].Contains("width=") || program[i].Contains("width ="))
                    {
                        try
                        {
                            //Splits the command to get to the width's parameter.
                            string[] value = program[i].Split('=');
                            string wid = value[1];
                            //Converts the width parameter to an integar and save it for later use.
                            width = Convert.ToInt32(wid);
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the width variable requires one parameter.
                            String message = "The number of parameters for saving the width variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the parameter must be an intergar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the radius variable.
                    else if (program[i].Contains("width+") || program[i].Contains("width + "))
                    {
                        try
                        {
                            //Splits the command to get to the variable's parameter.
                            string[] value = program[i].Split('+');
                            string rad = value[1];
                            //Converts the parameter to an integar.
                            int addRad = Convert.ToInt32(rad);
                            width = width + addRad;
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the radius variable needs only one parameter.
                            String message = "The number of parameters for the radius variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the data type of the radius variable must be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the radius variable.
                    else if (program[i].Contains("width-") || program[i].Contains("width - "))
                    {
                        try
                        {
                            //Splits the command to get to the variable's parameter.
                            string[] value = program[i].Split('+');
                            string rad = value[1];
                            //Converts the parameter to an integar.
                            int subRad = Convert.ToInt32(rad);
                            width = width - subRad;
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the radius variable needs only one parameter.
                            String message = "The number of parameters for the radius variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the data type of the radius variable must be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the radius variable.
                    else if (program[i].Contains("radius+") || program[i].Contains("radius + "))
                    {
                        try
                        {
                            //Splits the command to get to the variable's parameter.
                            string[] value = program[i].Split('+');
                            string rad = value[1];
                            //Converts the parameter to an integar.
                            int addRad = Convert.ToInt32(rad);
                            radius = radius + addRad;
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the radius variable needs only one parameter.
                            String message = "The number of parameters for the radius variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the data type of the radius variable must be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the radius variable.
                    else if (program[i].Contains("radius-") || program[i].Contains("radius - "))
                    {
                        try
                        {
                            //Splits the command to get to the variable's parameter.
                            string[] value = program[i].Split('+');
                            string rad = value[1];
                            //Converts the parameter to an integar.
                            int subRad = Convert.ToInt32(rad);
                            radius = radius - subRad;
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the radius variable needs only one parameter.
                            String message = "The number of parameters for the radius variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the data type of the radius variable must be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Checks if the user defined the radius variable.
                    else if (program[i].Contains("radius=") || program[i].Contains("radius = "))
                    {
                        try
                        {
                            //Splits the command to get to the variable's parameter.
                            string[] value = program[i].Split('=');
                            string rad = value[1];
                            //Converts the parameter to an integar.
                            radius = Convert.ToInt32(rad);
                        }
                        //Exception for if there are too many or not enough parameters.
                        catch (IndexOutOfRangeException)
                        {
                            //Message saying that the radius variable needs only one parameter.
                            String message = "The number of parameters for the radius variable was unsuitable. It takes one parameters. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a square.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                        //Exception for if the width variable parameter is using an incorrect data type.
                        catch (FormatException)
                        {
                            //Message saying that the data type of the radius variable must be an integar.
                            String message = "The format of the parameters is unsuitable. Ensure they are integars. " +
                                "Error on line: " + lines;
                            String caption = "Unable to draw a circle.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Displays the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //If the user's program contains 'load file' the program will open a previously made piece of code.
                    else if (program[i].Contains("load file"))
                    {
                        //Clear the textbox.
                        richTextBox1.Clear();
                        //If the file cannot be opened, the program will watch the error.
                        try
                        {

                            //Allows the user to selected a file.
                            OpenFileDialog file = new OpenFileDialog();
                            if (file.ShowDialog() == DialogResult.OK)
                            {

                                //Reads the files and save is to a string.
                                string sourceCode = File.ReadAllText(file.FileName);
                                richTextBox1.AppendText(sourceCode);

                            }
                        }
                        //Exception for input/output errors with files.
                        catch (IOException)
                        {
                            //Message saying that the file format chosen was unsuitable for the program to open.
                            String message = "Cannot open this specific file.";
                            String caption = "Unable to open.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result;

                            //Display the dialog box.
                            result = MessageBox.Show(message, caption, buttons);
                        }
                    }

                    //Will exit the program.
                    else if (program[i].Contains("exit"))
                    {
                        //Displays a message asking for confirmation to close the program.
                        String message = "Exit the program?";
                        String caption = "Exit";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;

                        //Display the dialog box.
                        result = MessageBox.Show(message, caption, buttons);

                        //If yes then close the program.
                        if (result == DialogResult.Yes)
                        {
                            //Closes the program.
                            Application.Exit();
                        }
                    }

                    //Will clear the picture box if requested by the user.
                    else if (program[i].Contains("clear"))
                    {
                        try
                        {
                            //Gets the clear function from the factory.
                            shapes.Add(factory.getShape("clear"));

                        }
                        //Exception if the clear function couldn't be found in the factory.
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid shape: " + e);

                        }
                        Shape s;
                        //Sets the colour to black will will reset the picture box.
                        Color newColor = Color.Black;
                        s = factory.getShape("clear");
                        //Passes through the variables and clears the picture box.
                        s.set(newColor, 0, 0, 10000, 10000);
                        shapes.Add(s);
                        //Refresh the picture box so the reset is visible.
                        pictureBox1.Refresh();
                        //Set the x, y coordinates back to the original in the top left corner.
                        xpos = 0;
                        ypos = 0;
                    }

                    else if (program[i].Contains("end"))
                    {

                    }
                    else if (program[i].Contains("stop"))
                    {
                   
                        Console.WriteLine(loop + " " + loopTimes);
                        if (loopTimes == loop)
                        {
                            //If it's not empty then just skip the if statement as it is false.
                            i = loopIndex+1;
                            
                        }
                        if(loopTimes != loop)
                        {
                            loop++;
                            i = loopLine;
                        }
                    }

                    //If the user's program equals nothing perform no actions.
                    else if (program[i].Equals("") || program[i].Equals(" ") || program[i] == null)
                    {

                    }
                    else
                    {

                    }
                    //Increment the line so they user know which line an error occurs on.
                    lines++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Error");
                }
            }
        }

        /// <summary>
        /// Method to paint the lines and shapes to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //Increment throught he saved shapes defined by the user.
            for (int i = 0; i < shapes.Count; i++)
            {
                //Get the shapes from the array.
                Shape s;
                s = (Shape)shapes[i];
                if (s != null)
                {
                    //Pass through the shapes to the method which will draw them.
                    s.draw(g);

                }
                //Shouldn't happen as factory does not produce rubbish
                else
                    Console.WriteLine("invalid shape in array");
            }
        }
    }
}