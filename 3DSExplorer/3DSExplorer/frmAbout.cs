using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using SharpGL;

namespace _3DSExplorer
{
    public partial class frmAbout : Form
    {
        private const int TexturesNum = 3;
        private const int TextureCube3 = 0;
        private const int TextureCubeD = 1;
        private const int TextureCubeS = 2;
        private double _rquad;
        private readonly uint[] _textures = new uint[TexturesNum];

        private readonly int[][][] _cube = {  new[]{new[]{-1,1,-1}, new[]{-1,1,1}, new[]{1,1,1},new[]{1,1,-1}},
                                    new[]{new[]{-1,-1,1},new[]{-1,-1,-1}, new[]{1,-1,-1}, new[]{1,-1,1}},
                                    new[]{new[]{-1,-1,1}, new[]{1,-1,1}, new[]{1,1,1},new[]{-1,1,1}},
                                    new[]{new[]{1,-1,-1},new[]{-1,-1,-1}, new[]{-1,1,-1}, new[]{1,1,-1}},
                                    new[]{new[]{-1,-1,-1}, new[]{-1,-1,1}, new[]{-1,1,1},new[]{-1,1,-1}},
                                    new[]{new[]{1,-1,1},new[]{1,-1,-1}, new[]{1,1,-1}, new[]{1,1,1}}
                                };
        public frmAbout()
        {
            InitializeComponent();
            lblTitle.Text = 'v' + Application.ProductVersion;
        }

        private void DrawQuadsWithTexture(OpenGL gl, int texture, params int[][][] quads)
        {
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, _textures[texture]);
            gl.Begin(OpenGL.GL_QUADS);
            foreach (var quad in quads)
            {
                gl.TexCoord(0.0f, 0.0f); gl.Vertex(quad[0]);
                gl.TexCoord(1.0f, 0.0f); gl.Vertex(quad[1]);
                gl.TexCoord(1.0f, 1.0f); gl.Vertex(quad[2]);
                gl.TexCoord(0.0f, 1.0f); gl.Vertex(quad[3]);
            }
            gl.End();
        }

        private void openGLControl1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            var gl = openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            gl.Translate(0.0f, 0.0f, -5.0f); //Camera
            gl.Rotate(50, 1.0f, 0.5f, 0.5f);  //Rotation
            gl.Rotate(_rquad += 3.0f, 0.0f, 1.0f, 0.0f);

            DrawQuadsWithTexture(gl, TextureCube3, _cube[0]); // _cube[1] isn't visible
            DrawQuadsWithTexture(gl, TextureCubeD, _cube[2], _cube[3]);
            DrawQuadsWithTexture(gl, TextureCubeS, _cube[4], _cube[5]);
            gl.Flush();
        }

        private void BindBitmapToTexture(OpenGL gl, Bitmap bmp, int textureNumber)
        {
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, _textures[textureNumber]);
            gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, 3, bmp.Width, bmp.Height, 0, 
                OpenGL.GL_BGR, 
                OpenGL.GL_UNSIGNED_BYTE, 
                bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height), 
                    ImageLockMode.ReadOnly, 
                    PixelFormat.Format24bppRgb
                    ).Scan0
                );
            //  Specify linear filtering.
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);
            // the texture wraps over at the edges (repeat)
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
        }

        private void openGLControl1_OpenGLInitialized(object sender, System.EventArgs e)
        {
            //  Get the OpenGL object, for quick access.
            var gl = openGLControl1.OpenGL;

            //  A bit of extra initialisation here, we have to enable textures.
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            //  Get one texture id, and stick it into the textures array.
            gl.GenTextures(TexturesNum, _textures);
            //  Make the textures
            BindBitmapToTexture(gl, Properties.Resources.cube_3, TextureCube3);
            BindBitmapToTexture(gl, Properties.Resources.cube_d, TextureCubeD);
            BindBitmapToTexture(gl, Properties.Resources.cube_s, TextureCubeS);
        }
    }
}
