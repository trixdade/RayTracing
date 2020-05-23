using OpenTK.Graphics.ES10;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayTracing
{
    public partial class RayTracing : Form
    {
        private OpenTK.GLControl GLViewer;

        private float TrackBarStepX = 1.0F;
        private float TrackBarStepY = 1.0F;

        private View SV;
        public RayTracing()
        {
            InitializeComponent();

            GLViewer = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));

            GLViewer.Paint += GLPaint;

            Controls.Add(GLViewer);

            GLViewer.Top = 12;
            GLViewer.Left = 12;
            GLViewer.Width = 595;
            GLViewer.Height = 595;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GLViewer.MakeCurrent();

            SV = new View(GLViewer.Width, GLViewer.Height, GLViewer.Width, GLViewer.Height);
            TrackBarStepX = (4.0f + 4.0f) / (tbPosX.Maximum - tbPosX.Minimum);
            TrackBarStepY = (4.0f + 4.0f) / (tbPosY.Maximum - tbPosY.Minimum);
        }

        private void GLPaint(object sender, PaintEventArgs e)
        {
            GLViewer.MakeCurrent();
            SV.DrawQuads();
            GLViewer.SwapBuffers();
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            GLViewer.Invalidate();
        }

        private void tbPosX_Scroll(object sender, EventArgs e)
        {
            SV.LightSourcePosition.X = TrackBarStepX * tbPosX.Value;
            GLViewer.Invalidate();
        }

        private void tbPosY_Scroll(object sender, EventArgs e)
        {
            SV.LightSourcePosition.Y = TrackBarStepY * tbPosY.Value;
            GLViewer.Invalidate();
        }
        private void radioPositionChanged(object sender, EventArgs e)
        {           
            tbPosX.Value = (int)(SV.LightSourcePosition.X / TrackBarStepX);
            tbPosY.Value = (int)(SV.LightSourcePosition.Y / TrackBarStepY);           
        }

        private void buttonAddCube_Click(object sender, EventArgs e)
        {
            float x = (float)Convert.ToDouble(textCubePosX.Text.Replace('.', ',').Trim());
            float y = (float)Convert.ToDouble(textCubePosY.Text.Replace('.', ',').Trim());
            float z = (float)Convert.ToDouble(textCubePosZ.Text.Replace('.', ',').Trim());

            string sizeStr = comboSize.SelectedItem as string;
            float  sizeFlt = (float)Convert.ToDouble(sizeStr.Replace('.', ',').Trim());

            if (SV.CUBE_COUNT <= 10)
            {
                int nextPos = SV.CUBE_COUNT;
                SV.CUBE_COUNT++;

                SV.CubePositions[nextPos] = new OpenTK.Vector3(x, y, z);
                SV.CubeSizes[nextPos] = sizeFlt;
                SV.CubeMaterials[nextPos] = comboColor.SelectedIndex;
            }

            GLViewer.Invalidate();
        }
    }
}
