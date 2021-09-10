using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Peg_Solitaire
{
    public partial class frm_menu : Form
    {
        Agent selectedAgent;
        GameState selectedGame;
        List<List<List<int>>> movesToWin;
        int moveIndex = 0;


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
        }

        // Will be used to run agents in background, not implemented yet.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Assign the result of the computation
            // to the Result property of the DoWorkEventArgs
            // object. This is will be available to the 
            // RunWorkerCompleted eventhandler.
            e.Result = selectedAgent.Solve();
        }

        // Will be used to run agents in background, not implemented yet.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

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
            // Setup selected game type
            if(cmb_gameSelect.Text == "Triangle, 5 Row")
            {
                selectedGame = TriangleGames.BasicTriangle(5);
            }
            else if (cmb_gameSelect.Text == "Triangle, 6 Row")
            {
                selectedGame = TriangleGames.BasicTriangle(6);
            }
            else if (cmb_gameSelect.Text == "Triangle, 7 Row")
            {
                selectedGame = TriangleGames.BasicTriangle(7);
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
            if (cmb_agentSelect.Text == "Iterative Deepening")
            {
                selectedAgent = new IterativeDeepeningAgent(selectedGame);
            }

            txt_output.Clear();
            movesToWin = selectedAgent.Solve();

            int moveNumber = 0;
            string lineToAdd;
            foreach(List<List<int>> move in movesToWin)
            {
                moveNumber++;
                lineToAdd = string.Format("Action {0}: ", moveNumber);
                txt_output.AppendText(lineToAdd);
                lineToAdd = string.Format("Move Peg ({0},{1}) to ({2},{3}) and Remove Peg ({4},{5})\n", 
                    move[0][0], move[0][1], move[2][0], move[2][1], move[1][0], move[1][1]);
                txt_output.AppendText(lineToAdd);
            }

            MessageBox.Show("Solution found! See animation to the right.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            moveIndex = 0;
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

            if (cmb_gameSelect.Text == "Triangle, 5 Row")
            {
                pnl_tri5.Show();
            }
            else
            {
                pnl_tri5.Hide();
            }

            if (cmb_gameSelect.Text == "Triangle, 6 Row")
            {
                pnl_tri6.Show();
            }
            else
            {
                pnl_tri6.Hide();
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

            if (cmb_gameSelect.Text == "Triangle, 5 Row")
            {
                animateMoveTri5(move);
            }
            else if (cmb_gameSelect.Text == "Triangle, 6 Row")
            {
                animateMoveTri6(move);
            }
            else
            {
                animationTimer.Enabled = false;
                cmb_gameSelect.Enabled = true;
                cmb_agentSelect.Enabled = true;
            }

            moveIndex++;
            if (moveIndex >= movesToWin.Count)
            {
                animationTimer.Enabled = false;
                cmb_gameSelect.Enabled = true;
                cmb_agentSelect.Enabled = true;
            }
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
    }
}
