using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Peg_Solitaire
{
    public partial class frm_menu : Form
    {
        // Global variables for form communication
        Agent selectedAgent;
        GameState selectedGame;
        List<List<List<int>>> movesToWin;
        int moveIndex = 0;
        bool custom = false;
        bool enableCustom = false;
        List<List<bool>> customPegMap;
        bool isTimeout = false;
        DateTime selectedTimeout;
        DateTime startTime;
        DateTime endTime;
        TimeSpan elapsedTime;

        /// <summary>
        /// Constructor
        /// </summary>
        public frm_menu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event that fires when the GUI is loaded.
        /// Used to setup the default configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            cmb_timeout.SelectedIndex = 0;
        }

        /// <summary>
        /// Generates a peg map list for use with custom board configurations
        /// The boolean values for each peg are created and initialized to the
        /// default board configuration. This allows simple indexing logic
        /// to customize the board layout as peg locations are selected
        /// </summary>
        /// <param name="numRows"></param>
        /// <returns>Initialized custom peg map</returns>
        private List<List<bool>> initializeCustomPegMap(int numRows)
        {
            List<bool> firstRow = new List<bool> { false };
            List<bool> currentRow = new List<bool> { true, true };
            List<List<bool>> pegMap = new List<List<bool>> { };

            pegMap.Add(new List<bool>(firstRow));

            for (int i = 0; i < numRows - 1; i++)
            {
                pegMap.Add(new List<bool>(currentRow));
                currentRow.Add(true);
            }

            return pegMap;
        }

        /// <summary>
        /// Reads the timeout configuration settings from the GUI
        /// and sets up a datetime object to pass to the Agent.solve() function
        /// to control the timeout exception. Also sets the isTimeout boolean variable
        /// to indicate if a timeout has been set or should be ignored. This boolean is
        /// also passed to the Agent.solve() function.
        /// </summary>
        private void setTimeout()
        {
            double selectedTime = 0;
            selectedTimeout = DateTime.Now;
            if(cmb_timeout.Text != "Never")
            {
                selectedTime = Convert.ToDouble(txt_timeout.Text);
            }
            if(cmb_timeout.Text == "Seconds")
            {
                selectedTimeout = selectedTimeout.AddSeconds(selectedTime);
                isTimeout = true;
            }
            else if (cmb_timeout.Text == "Minutes")
            {
                selectedTimeout = selectedTimeout.AddMinutes(selectedTime);
                isTimeout = true;
            }
            else if (cmb_timeout.Text == "Hours")
            {
                selectedTimeout = selectedTimeout.AddHours(selectedTime);
                isTimeout = true;
            }
            else
            {
                isTimeout = false;
            }
        }

        /// <summary>
        /// Resets the pegs on the visual gameboard representations
        /// to their default layout.
        /// </summary>
        private void resetPegs()
        {
            resetPegsTri5();
            resetPegsTri6();
        }

        /// <summary>
        /// Resets the 5 row triangle board animation to its original state.
        /// </summary>
        private void resetPegsTri5()
        {
            tri5P0_0.Hide();
            tri5P1_0.Show();
            tri5P1_1.Show();
            tri5P2_0.Show();
            tri5P2_1.Show();
            tri5P2_2.Show();
            tri5P3_0.Show();
            tri5P3_1.Show();
            tri5P3_2.Show();
            tri5P3_3.Show();
            tri5P4_0.Show();
            tri5P4_1.Show();
            tri5P4_2.Show();
            tri5P4_3.Show();
            tri5P4_4.Show();
        }

        /// <summary>
        /// Resets the 6 row triangle board animation to its original state.
        /// </summary>
        private void resetPegsTri6()
        {
            tri6P0_0.Hide();
            tri6P1_0.Show();
            tri6P1_1.Show();
            tri6P2_0.Show();
            tri6P2_1.Show();
            tri6P2_2.Show();
            tri6P3_0.Show();
            tri6P3_1.Show();
            tri6P3_2.Show();
            tri6P3_3.Show();
            tri6P4_0.Show();
            tri6P4_1.Show();
            tri6P4_2.Show();
            tri6P4_3.Show();
            tri6P4_4.Show();
            tri6P5_0.Show();
            tri6P5_1.Show();
            tri6P5_2.Show();
            tri6P5_3.Show();
            tri6P5_4.Show();
            tri6P5_5.Show();
        }

        /// <summary>
        /// Resets the visual representation of the custom game board
        /// for animation replay or for making a new custom board
        /// using the previous setup as a starting point.
        /// </summary>
        private void resetCustomTri5()
        {
            if(customPegMap[0][0])
            {
                tri5P0_0.Show();
            }
            else
            {
                tri5P0_0.Hide();
            }
            if (customPegMap[1][0])
            {
                tri5P1_0.Show();
            }
            else
            {
                tri5P1_0.Hide();
            }
            if (customPegMap[1][1])
            {
                tri5P1_1.Show();
            }
            else
            {
                tri5P1_1.Hide();
            }
            if (customPegMap[2][0])
            {
                tri5P2_0.Show();
            }
            else
            {
                tri5P2_0.Hide();
            }
            if (customPegMap[2][1])
            {
                tri5P2_1.Show();
            }
            else
            {
                tri5P2_1.Hide();
            }
            if (customPegMap[2][2])
            {
                tri5P2_2.Show();
            }
            else
            {
                tri5P2_2.Hide();
            }
            if (customPegMap[3][0])
            {
                tri5P3_0.Show();
            }
            else
            {
                tri5P3_0.Hide();
            }
            if (customPegMap[3][1])
            {
                tri5P3_1.Show();
            }
            else
            {
                tri5P3_1.Hide();
            }
            if (customPegMap[3][2])
            {
                tri5P3_2.Show();
            }
            else
            {
                tri5P3_2.Hide();
            }
            if (customPegMap[3][3])
            {
                tri5P3_3.Show();
            }
            else
            {
                tri5P3_3.Hide();
            }
            if (customPegMap[4][0])
            {
                tri5P4_0.Show();
            }
            else
            {
                tri5P4_0.Hide();
            }
            if (customPegMap[4][1])
            {
                tri5P4_1.Show();
            }
            else
            {
                tri5P4_1.Hide();
            }
            if (customPegMap[4][2])
            {
                tri5P4_2.Show();
            }
            else
            {
                tri5P4_2.Hide();
            }
            if (customPegMap[4][3])
            {
                tri5P4_3.Show();
            }
            else
            {
                tri5P4_3.Hide();
            }
            if (customPegMap[4][4])
            {
                tri5P4_4.Show();
            }
            else
            {
                tri5P4_4.Hide();
            }
        }

        /// <summary>
        /// Event that fies when the RUN button is clicked.
        /// This is used to run the selected agent and game in search of a solution.
        /// Upon completion a message gox is displayed and the animation of the solution
        /// is played.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_run_Click(object sender, EventArgs e)
        {
            if(cmb_gameSelect.SelectedItem == null || cmb_agentSelect.SelectedItem == null)
            {
                MessageBox.Show("Please Select Game Board and Agent.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            cmb_gameSelect.Enabled = false;
            cmb_agentSelect.Enabled = false;
            btn_run.Enabled = false;
            btn_replay.Hide();
            string messageBoxMesage;
            // Setup selected game type
            if (cmb_gameSelect.Text == "Triangle, 5 Row Basic")
            {
                selectedGame = TriangleGames.BasicTriangle(5);
            }
            else if (cmb_gameSelect.Text == "Triangle, 6 Row Basic")
            {
                selectedGame = TriangleGames.BasicTriangle(6);
            }
            else if (cmb_gameSelect.Text == "Triangle, 7 Row")
            {
                selectedGame = TriangleGames.BasicTriangle(7);
            }
            if (cmb_gameSelect.Text == "Triangle, 5 Row Custom")
            {
                selectedGame = new GameState(customPegMap);
            }

            // Setup selected agent type
            if (cmb_agentSelect.Text == "Breadth First")
            {
                selectedAgent = new BreadthFirstAgent(selectedGame);
            }
            else if (cmb_agentSelect.Text == "Depth First")
            {
                selectedAgent = new DepthFirstAgent(selectedGame);
            }
            else if (cmb_agentSelect.Text == "Iterative Deepening")
            {
                selectedAgent = new IterativeDeepeningAgent(selectedGame);
            }
            else if (cmb_agentSelect.Text == "Q-Learning")
            {
                selectedAgent = new QLearningAgent(selectedGame);
            }

            txt_output.Clear();

            if(!custom)
                resetPegs();

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                startTime = DateTime.Now;
                setTimeout();
                movesToWin = selectedAgent.Solve(isTimeout, selectedTimeout);
                endTime = DateTime.Now;
            }
            catch(Exception except)
            {
                string failureMessage = except.ToString();
                if(except.ToString().Contains("No solution exists for this game."))
                {
                    failureMessage = string.Format("No solution exists for this game. {0} states expanded.",
                        selectedAgent.getTotalExpandedStates());
                    MessageBox.Show(failureMessage, "No solution found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (except.ToString().Contains("timed out."))
                {
                    failureMessage = string.Format("Search time limit reached before a solution was found. {0} states expanded.", selectedAgent.getTotalExpandedStates());
                    MessageBox.Show(failureMessage, "No solution found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (except.ToString().Contains("format"))
                {
                    failureMessage = string.Format("Timeout must be a numeric value.");
                    MessageBox.Show(failureMessage, "Invalid timeout entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                Cursor.Current = Cursors.Default;
                cmb_gameSelect.Enabled = true;
                cmb_agentSelect.Enabled = true;
                btn_run.Enabled = true;
                return;
            }

            int moveNumber = 0;
            string lineToAdd;
            elapsedTime = endTime - startTime;
            lineToAdd = string.Format("Elapsed Time (h:m:s.ms): {0}:{1}:{2}.{3}\n", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds);
            txt_output.AppendText(lineToAdd);
            if(cmb_agentSelect.Text != "Q-Learning")
            {
                lineToAdd = string.Format("Expanded {0} states to find solution:\n\n", selectedAgent.getTotalExpandedStates());
                txt_output.AppendText(lineToAdd);
            }

            foreach (List<List<int>> move in movesToWin)
            {
                moveNumber++;
                lineToAdd = string.Format("Action {0}: ", moveNumber);
                txt_output.AppendText(lineToAdd);
                lineToAdd = string.Format("Move Peg ({0},{1}) to ({2},{3}) and Remove Peg ({4},{5})\n", 
                    move[0][0], move[0][1], move[2][0], move[2][1], move[1][0], move[1][1]);
                txt_output.AppendText(lineToAdd);
            }

            Cursor.Current = Cursors.Default;
            if (cmb_agentSelect.Text == "Q-Learning")
            {
                messageBoxMesage = string.Format("Learning sequence complete. See animation to the right.");
            }
            else
            {
                messageBoxMesage = string.Format("Solution Found! {0} states expanded. See animation to the right.", selectedAgent.getTotalExpandedStates());
            }
            
            MessageBox.Show(messageBoxMesage, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            moveIndex = 0;
            enableCustom = false;
            animationTimer.Enabled = true;
        }

        /// <summary>
        /// Event that fires when the game type selection is changed.
        /// This event is used to display the correct game board image
        /// and animation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_gameSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetPegs();
            btn_replay.Hide();

            if (cmb_gameSelect.Text.Contains("Triangle, 5 Row"))
            {
                pnl_tri5.Show();
            }
            else
            {
                pnl_tri5.Hide();
            }

            if (cmb_gameSelect.Text == "Triangle, 6 Row Basic")
            {
                pnl_tri6.Show();
            }
            else
            {
                pnl_tri6.Hide();
            }

            if (cmb_gameSelect.Text == "Triangle, 5 Row Custom")
            {
                custom = true;
                enableCustom = true;
                customPegMap = initializeCustomPegMap(5);
                MessageBox.Show("Custom board selected. Click pegs/holes to toggle.", "Setup Custom Board", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                custom = false;
                enableCustom = false;
            }
        }

        /// <summary>
        /// Runs the move sequence animation, going through each move in the list, one per time tick.
        /// The timer that generates this event is enabled after a solution is found and disabled
        /// once the animation is complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void animationTimer_Tick(object sender, EventArgs e)
        {
            List<List<int>> move = movesToWin[moveIndex];
            if (cmb_gameSelect.Text.Contains("Triangle, 5 Row"))
            {
                animateMoveTri5(move);
            }
            else if (cmb_gameSelect.Text.Contains("Triangle, 6 Row"))
            {
                animateMoveTri6(move);
            }
            else
            {
                animationTimer.Enabled = false;
                cmb_gameSelect.Enabled = true;
                cmb_agentSelect.Enabled = true;
                btn_run.Enabled = true;
            }

            moveIndex++;
            if (moveIndex >= movesToWin.Count)
            {
                animationTimer.Enabled = false;
                cmb_gameSelect.Enabled = true;
                cmb_agentSelect.Enabled = true;
                btn_run.Enabled = true;
                enableCustom = true;
                btn_replay.Show();
            }
        }

        private void btn_replay_Click(object sender, EventArgs e)
        {
            if (!custom)
                resetPegs();
            else
                resetCustomTri5();
            moveIndex = 0;
            animationTimer.Enabled = true;
        }

        /// <summary>
        /// Adjusts the 5 row triangle game board animation and peg locations to reflect the input move
        /// </summary>
        /// <param name="move">List of 3 coordinate pairs. These pairs indicate 
        /// Original peg location, peg to remove, new peg location</param>
        private void animateMoveTri5(List<List<int>> move)
        {
            // Hide the peg that is moved
            if (move[0][0] == 0 && move[0][1] == 0)
                tri5P0_0.Hide();
            else if (move[0][0] == 1 && move[0][1] == 0)
                tri5P1_0.Hide();
            else if (move[0][0] == 1 && move[0][1] == 1)
                tri5P1_1.Hide();
            else if (move[0][0] == 2 && move[0][1] == 0)
                tri5P2_0.Hide();
            else if (move[0][0] == 2 && move[0][1] == 1)
                tri5P2_1.Hide();
            else if (move[0][0] == 2 && move[0][1] == 2)
                tri5P2_2.Hide();
            else if (move[0][0] == 3 && move[0][1] == 0)
                tri5P3_0.Hide();
            else if (move[0][0] == 3 && move[0][1] == 1)
                tri5P3_1.Hide();
            else if (move[0][0] == 3 && move[0][1] == 2)
                tri5P3_2.Hide();
            else if (move[0][0] == 3 && move[0][1] == 3)
                tri5P3_3.Hide();
            else if (move[0][0] == 4 && move[0][1] == 0)
                tri5P4_0.Hide();
            else if (move[0][0] == 4 && move[0][1] == 1)
                tri5P4_1.Hide();
            else if (move[0][0] == 4 && move[0][1] == 2)
                tri5P4_2.Hide();
            else if (move[0][0] == 4 && move[0][1] == 3)
                tri5P4_3.Hide();
            else if (move[0][0] == 4 && move[0][1] == 4)
                tri5P4_4.Hide();

            // Hide the peg that is jumped
            if (move[1][0] == 0 && move[1][1] == 0)
                tri5P0_0.Hide();
            else if (move[1][0] == 1 && move[1][1] == 0)
                tri5P1_0.Hide();
            else if (move[1][0] == 1 && move[1][1] == 1)
                tri5P1_1.Hide();
            else if (move[1][0] == 2 && move[1][1] == 0)
                tri5P2_0.Hide();
            else if (move[1][0] == 2 && move[1][1] == 1)
                tri5P2_1.Hide();
            else if (move[1][0] == 2 && move[1][1] == 2)
                tri5P2_2.Hide();
            else if (move[1][0] == 3 && move[1][1] == 0)
                tri5P3_0.Hide();
            else if (move[1][0] == 3 && move[1][1] == 1)
                tri5P3_1.Hide();
            else if (move[1][0] == 3 && move[1][1] == 2)
                tri5P3_2.Hide();
            else if (move[1][0] == 3 && move[1][1] == 3)
                tri5P3_3.Hide();
            else if (move[1][0] == 4 && move[1][1] == 0)
                tri5P4_0.Hide();
            else if (move[1][0] == 4 && move[1][1] == 1)
                tri5P4_1.Hide();
            else if (move[1][0] == 4 && move[1][1] == 2)
                tri5P4_2.Hide();
            else if (move[1][0] == 4 && move[1][1] == 3)
                tri5P4_3.Hide();
            else if (move[1][0] == 4 && move[1][1] == 4)
                tri5P4_4.Hide();

            // Show the peg where it landed
            if (move[2][0] == 0 && move[2][1] == 0)
                tri5P0_0.Show();
            else if (move[2][0] == 1 && move[2][1] == 0)
                tri5P1_0.Show();
            else if (move[2][0] == 1 && move[2][1] == 1)
                tri5P1_1.Show();
            else if (move[2][0] == 2 && move[2][1] == 0)
                tri5P2_0.Show();
            else if (move[2][0] == 2 && move[2][1] == 1)
                tri5P2_1.Show();
            else if (move[2][0] == 2 && move[2][1] == 2)
                tri5P2_2.Show();
            else if (move[2][0] == 3 && move[2][1] == 0)
                tri5P3_0.Show();
            else if (move[2][0] == 3 && move[2][1] == 1)
                tri5P3_1.Show();
            else if (move[2][0] == 3 && move[2][1] == 2)
                tri5P3_2.Show();
            else if (move[2][0] == 3 && move[2][1] == 3)
                tri5P3_3.Show();
            else if (move[2][0] == 4 && move[2][1] == 0)
                tri5P4_0.Show();
            else if (move[2][0] == 4 && move[2][1] == 1)
                tri5P4_1.Show();
            else if (move[2][0] == 4 && move[2][1] == 2)
                tri5P4_2.Show();
            else if (move[2][0] == 4 && move[2][1] == 3)
                tri5P4_3.Show();
            else if (move[2][0] == 4 && move[2][1] == 4)
                tri5P4_4.Show();
        }

        /// <summary>
        /// Adjusts the 6 row triangle game board animation and peg locations to reflect the input move
        /// </summary>
        /// <param name="move">List of 3 coordinate pairs. These pairs indicate 
        /// Original peg location, peg to remove, new peg location</param>
        private void animateMoveTri6(List<List<int>> move)
        {
            // Hide the peg that is moved
            if (move[0][0] == 0 && move[0][1] == 0)
                tri6P0_0.Hide();
            else if (move[0][0] == 1 && move[0][1] == 0)
                tri6P1_0.Hide();
            else if (move[0][0] == 1 && move[0][1] == 1)
                tri6P1_1.Hide();
            else if (move[0][0] == 2 && move[0][1] == 0)
                tri6P2_0.Hide();
            else if (move[0][0] == 2 && move[0][1] == 1)
                tri6P2_1.Hide();
            else if (move[0][0] == 2 && move[0][1] == 2)
                tri6P2_2.Hide();
            else if (move[0][0] == 3 && move[0][1] == 0)
                tri6P3_0.Hide();
            else if (move[0][0] == 3 && move[0][1] == 1)
                tri6P3_1.Hide();
            else if (move[0][0] == 3 && move[0][1] == 2)
                tri6P3_2.Hide();
            else if (move[0][0] == 3 && move[0][1] == 3)
                tri6P3_3.Hide();
            else if (move[0][0] == 4 && move[0][1] == 0)
                tri6P4_0.Hide();
            else if (move[0][0] == 4 && move[0][1] == 1)
                tri6P4_1.Hide();
            else if (move[0][0] == 4 && move[0][1] == 2)
                tri6P4_2.Hide();
            else if (move[0][0] == 4 && move[0][1] == 3)
                tri6P4_3.Hide();
            else if (move[0][0] == 4 && move[0][1] == 4)
                tri6P4_4.Hide();
            else if (move[0][0] == 5 && move[0][1] == 0)
                tri6P5_0.Hide();
            else if (move[0][0] == 5 && move[0][1] == 1)
                tri6P5_1.Hide();
            else if (move[0][0] == 5 && move[0][1] == 2)
                tri6P5_2.Hide();
            else if (move[0][0] == 5 && move[0][1] == 3)
                tri6P5_3.Hide();
            else if (move[0][0] == 5 && move[0][1] == 4)
                tri6P5_4.Hide();
            else if (move[0][0] == 5 && move[0][1] == 5)
                tri6P5_5.Hide();

            // Hide the peg that is jumped
            if (move[1][0] == 0 && move[1][1] == 0)
                tri6P0_0.Hide();
            else if (move[1][0] == 1 && move[1][1] == 0)
                tri6P1_0.Hide();
            else if (move[1][0] == 1 && move[1][1] == 1)
                tri6P1_1.Hide();
            else if (move[1][0] == 2 && move[1][1] == 0)
                tri6P2_0.Hide();
            else if (move[1][0] == 2 && move[1][1] == 1)
                tri6P2_1.Hide();
            else if (move[1][0] == 2 && move[1][1] == 2)
                tri6P2_2.Hide();
            else if (move[1][0] == 3 && move[1][1] == 0)
                tri6P3_0.Hide();
            else if (move[1][0] == 3 && move[1][1] == 1)
                tri6P3_1.Hide();
            else if (move[1][0] == 3 && move[1][1] == 2)
                tri6P3_2.Hide();
            else if (move[1][0] == 3 && move[1][1] == 3)
                tri6P3_3.Hide();
            else if (move[1][0] == 4 && move[1][1] == 0)
                tri6P4_0.Hide();
            else if (move[1][0] == 4 && move[1][1] == 1)
                tri6P4_1.Hide();
            else if (move[1][0] == 4 && move[1][1] == 2)
                tri6P4_2.Hide();
            else if (move[1][0] == 4 && move[1][1] == 3)
                tri6P4_3.Hide();
            else if (move[1][0] == 4 && move[1][1] == 4)
                tri6P4_4.Hide();
            else if (move[1][0] == 5 && move[1][1] == 0)
                tri6P5_0.Hide();
            else if (move[1][0] == 5 && move[1][1] == 1)
                tri6P5_1.Hide();
            else if (move[1][0] == 5 && move[1][1] == 2)
                tri6P5_2.Hide();
            else if (move[1][0] == 5 && move[1][1] == 3)
                tri6P5_3.Hide();
            else if (move[1][0] == 5 && move[1][1] == 4)
                tri6P5_4.Hide();
            else if (move[1][0] == 5 && move[1][1] == 5)
                tri6P5_5.Hide();

            // Show the peg where it landed
            if (move[2][0] == 0 && move[2][1] == 0)
                tri6P0_0.Show();
            else if (move[2][0] == 1 && move[2][1] == 0)
                tri6P1_0.Show();
            else if (move[2][0] == 1 && move[2][1] == 1)
                tri6P1_1.Show();
            else if (move[2][0] == 2 && move[2][1] == 0)
                tri6P2_0.Show();
            else if (move[2][0] == 2 && move[2][1] == 1)
                tri6P2_1.Show();
            else if (move[2][0] == 2 && move[2][1] == 2)
                tri6P2_2.Show();
            else if (move[2][0] == 3 && move[2][1] == 0)
                tri6P3_0.Show();
            else if (move[2][0] == 3 && move[2][1] == 1)
                tri6P3_1.Show();
            else if (move[2][0] == 3 && move[2][1] == 2)
                tri6P3_2.Show();
            else if (move[2][0] == 3 && move[2][1] == 3)
                tri6P3_3.Show();
            else if (move[2][0] == 4 && move[2][1] == 0)
                tri6P4_0.Show();
            else if (move[2][0] == 4 && move[2][1] == 1)
                tri6P4_1.Show();
            else if (move[2][0] == 4 && move[2][1] == 2)
                tri6P4_2.Show();
            else if (move[2][0] == 4 && move[2][1] == 3)
                tri6P4_3.Show();
            else if (move[2][0] == 4 && move[2][1] == 4)
                tri6P4_4.Show();
            else if (move[2][0] == 5 && move[2][1] == 0)
                tri6P5_0.Show();
            else if (move[2][0] == 5 && move[2][1] == 1)
                tri6P5_1.Show();
            else if (move[2][0] == 5 && move[2][1] == 2)
                tri6P5_2.Show();
            else if (move[2][0] == 5 && move[2][1] == 3)
                tri6P5_3.Show();
            else if (move[2][0] == 5 && move[2][1] == 4)
                tri6P5_4.Show();
            else if (move[2][0] == 5 && move[2][1] == 5)
                tri6P5_5.Show();
        }

        // All functions below this point are click events
        // for pegs and holes used to setup custom 
        // game board configurations

        private void tri5H0_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[0][0] = true;
            tri5P0_0.Show();
            btn_replay.Hide();
        }

        private void tri5H1_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[1][0] = true;
            tri5P1_0.Show();
            btn_replay.Hide();
        }

        private void tri5H1_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[1][1] = true;
            tri5P1_1.Show();
            btn_replay.Hide();
        }

        private void tri5H2_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[2][0] = true;
            tri5P2_0.Show();
            btn_replay.Hide();
        }

        private void tri5H2_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[2][1] = true;
            tri5P2_1.Show();
            btn_replay.Hide();
        }

        private void tri5H2_2_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[2][2] = true;
            tri5P2_2.Show();
            btn_replay.Hide();
        }

        private void tri5H3_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][0] = true;
            tri5P3_0.Show();
            btn_replay.Hide();
        }

        private void tri5H3_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][1] = true;
            tri5P3_1.Show();
            btn_replay.Hide();
        }

        private void tri5H3_2_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][2] = true;
            tri5P3_2.Show();
            btn_replay.Hide();
        }

        private void tri5H3_3_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][3] = true;
            tri5P3_3.Show();
            btn_replay.Hide();
        }

        private void tri5H4_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][0] = true;
            tri5P4_0.Show();
            btn_replay.Hide();
        }

        private void tri5H4_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][1] = true;
            tri5P4_1.Show();
            btn_replay.Hide();
        }

        private void tri5H4_2_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][2] = true;
            tri5P4_2.Show();
            btn_replay.Hide();
        }

        private void tri5H4_3_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][3] = true;
            tri5P4_3.Show();
            btn_replay.Hide();
        }

        private void tri5H4_4_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][4] = true;
            tri5P4_4.Show();
            btn_replay.Hide();
        }

        private void tri5P0_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[0][0] = false;
            tri5P0_0.Hide();
            btn_replay.Hide();
        }

        private void tri5P1_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[1][0] = false;
            tri5P1_0.Hide();
            btn_replay.Hide();
        }

        private void tri5P1_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[1][1] = false;
            tri5P1_1.Hide();
            btn_replay.Hide();
        }

        private void tri5P2_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[2][0] = false;
            tri5P2_0.Hide();
            btn_replay.Hide();
        }

        private void tri5P2_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[2][1] = false;
            tri5P2_1.Hide();
            btn_replay.Hide();
        }

        private void tri5P2_2_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[2][2] = false;
            tri5P2_2.Hide();
            btn_replay.Hide();
        }

        private void tri5P3_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][0] = false;
            tri5P3_0.Hide();
            btn_replay.Hide();
        }

        private void tri5P3_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][1] = false;
            tri5P3_1.Hide();
            btn_replay.Hide();
        }

        private void tri5P3_2_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][2] = false;
            tri5P3_2.Hide();
            btn_replay.Hide();
        }

        private void tri5P3_3_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[3][3] = false;
            tri5P3_3.Hide();
            btn_replay.Hide();
        }

        private void tri5P4_0_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][0] = false;
            tri5P4_0.Hide();
            btn_replay.Hide();
        }

        private void tri5P4_1_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][1] = false;
            tri5P4_1.Hide();
            btn_replay.Hide();
        }

        private void tri5P4_2_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][2] = false;
            tri5P4_2.Hide();
            btn_replay.Hide();
        }

        private void tri5P4_3_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][3] = false;
            tri5P4_3.Hide();
            btn_replay.Hide();
        }

        private void tri5P4_4_Click(object sender, EventArgs e)
        {
            if (!custom || !enableCustom)
                return;
            resetCustomTri5();
            customPegMap[4][4] = false;
            tri5P4_4.Hide();
            btn_replay.Hide();
        }
    }
}
