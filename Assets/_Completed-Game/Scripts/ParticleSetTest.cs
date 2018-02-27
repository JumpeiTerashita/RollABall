using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ParticleTest
{
    public class ParticleSetTest : MonoBehaviour
    {
        // データ型の定義
        public struct HipData
        {
            public int hipId;
            public Vector3 pos;
            public Color color;
            public float magnitude; // 等級
            public float parallax; // 視差
            public HipData(int _id, Vector3 _pos, Color _color, float _magnitude, float _parallax)
            {
                hipId = _id;
                pos = _pos;
                color = _color;
                magnitude = _magnitude;
                parallax = _parallax;
            }
            public HipData(HipData _data)
            {
                hipId = _data.hipId;
                pos = _data.pos;
                color = _data.color;
                magnitude = _data.magnitude;
                parallax = _data.parallax;
            }
        }

        // 星座線データ型の定義
        public struct HipLine
        {
            public string constellationNameShort;
            public HipData sttData;
            public HipData endData;
            public HipLine(string _name, HipData _sttData, HipData _endData)
            {
                constellationNameShort = _name;
                sttData = _sttData;
                endData = _endData;
            }
        }

        // 読み込むデータ
        [SerializeField] TextAsset lightFile = null;
        [SerializeField] TextAsset lineFile = null;
        [SerializeField] Material pointCloudMaterial = null;
        // 読み込んだデータを格納するリスト
        public List<HipData> hipList;
        public List<HipLine> hipLineList;
        // 星の距離(一律とする)
        public float distance = 100f;

        // Use this for initialization
        void Start()
        {
            hipList = createHipList(lightFile);
            hipLineList = createHipLineList(lineFile, hipList);
            if (hipList != null)
            {
                //                SetEfcStars(hipList, transform, distance);
                SetMeshStars(hipList, transform, pointCloudMaterial, distance);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        // ファイルからデータを読み込みデータリストに格納する
        List<HipData> createHipList(TextAsset _lightsFile)
        {
            List<HipData> list = new List<HipData>();
            StringReader sr = new StringReader(_lightsFile.text);
            while (sr.Peek() > -1)
            {
                string lineStr = sr.ReadLine();
                HipData data;
                if (stringToHipData(lineStr, out data))
                {
                    list.Add(data);
                }
            }
            sr.Close();
            return list;
        }

        // レンダリング終了後にラインを描画
        public void OnRenderObject()
        {
            if (!lineMaterial)
            {
                lineMaterial = CreateLineMaterial();
            }
            RenderLines(hipLineList, lineMaterial, transform, distance);
        }

        // ファイルからデータを読み込みLineデータリストに格納する
        List<HipLine> createHipLineList(TextAsset _linesFile, List<HipData> _hipList)
        {
            List<HipLine> list = new List<HipLine>();
            StringReader sr = new StringReader(_linesFile.text);
            while (sr.Peek() > -1)
            {
                string lineStr = sr.ReadLine();
                HipLine data;
                if (stringToHipLine(lineStr, _hipList, out data))
                {
                    list.Add(data);
                }
            }
            sr.Close();
            return list;
        }

        // CSV文字列からデータ型に変換
        bool stringToHipData(string _hipStr, out HipData data)
        {
            bool ret = true;
            data = new HipData();
            // カンマ区切りのデータを文字列の配列に変換
            string[] dataArr = _hipStr.Split(',');
            try
            {
                // 文字列をint,floatに変換する
                
                int hipId = int.Parse(dataArr[0]);
                float hlH = float.Parse(dataArr[1]);
                float hlM = float.Parse(dataArr[2]);
                float hlS = float.Parse(dataArr[3]);
                int hsSgn = int.Parse(dataArr[4]);
                float hsH = float.Parse(dataArr[5]);
                float hsM = float.Parse(dataArr[6]);
                float hsS = float.Parse(dataArr[7]);
                float mag = float.Parse(dataArr[8]);
                Color col = Color.gray;
                float hDeg = (360f / 24f) * (hlH + hlM / 60f + hlS / 3600f);
                float sDeg = (hsH + hsM / 60f + hsS / 3600f) * (hsSgn == 0 ? -1f : 1f);
                Quaternion rotL = Quaternion.AngleAxis(hDeg, Vector3.up);
                Quaternion rotS = Quaternion.AngleAxis(sDeg, Vector3.right);
                Vector3 pos = rotL * rotS * Vector3.forward;
                float parallax = 0f;
                if (dataArr.Length > 9)
                { // parallax
                    float.TryParse(dataArr[9], out parallax);
                }
                data = new HipData(hipId, pos, Color.white, mag, parallax);
            }
            catch
            {
                ret = false;
                Debug.Log("data err");
            }
            return ret;
        }

        // CSV文字列からLineデータ型に変換
        bool stringToHipLine(string _hipLineStr, List<HipData> _hipList, out HipLine data)
        {
            bool ret = true;
            data = new HipLine();
            string[] dataArr = _hipLineStr.Split(',');
            string shortName = dataArr[0];
            try
            {
                int sttId = int.Parse(dataArr[1]);
                int endId = int.Parse(dataArr[2]);
                HipData sttData = _hipList.First(d => (d.hipId == sttId));
                HipData endData = _hipList.First(d => (d.hipId == endId));
                data = new HipLine(shortName, sttData, endData);
            }
            catch
            {
                ret = false;
                Debug.Log("linedataerr:" + shortName);
            }
            return ret;
        }

        // パーティクルで星を表示する
        static public void SetEfcStars(List<HipData> _hipList, Transform _paricleTr, float _distance)
        {
            ParticleSystem _ps = _paricleTr.GetComponent<ParticleSystem>();
            if (_ps != null)
            {
                ParticleSystem.MainModule pmm = _ps.main;
                _ps.Clear();
                pmm.maxParticles = 200000;
                pmm.playOnAwake = false;
                pmm.startSize = 1f;
                pmm.simulationSpace = ParticleSystemSimulationSpace.Local;
                pmm.scalingMode = ParticleSystemScalingMode.Hierarchy;
                _ps.Emit(_hipList.Count);
                ParticleSystem.Particle[] stars = new ParticleSystem.Particle[_hipList.Count];
                _ps.GetParticles(stars);
                for (int i = 0; i < _hipList.Count; ++i)
                {
                    stars[i].position = _hipList[i].pos * _distance;
                    stars[i].startSize = 10f / _hipList[i].magnitude;
                }
                _ps.Play();
                _ps.SetParticles(stars, _hipList.Count);
                _ps.Pause();
            }
        }

        static Material lineMaterial;
        static public Material CreateLineMaterial()
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            Material mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            mat.SetInt("_ZWrite", 0);
            return mat;
        }

        // ラインを描画する
        static public void RenderLines(List<HipLine> _lineList, Material _mat, Transform _transform, float _distance)
        {
            // Apply the line material
            _mat.SetPass(0);

            // Matrixを退避
            GL.PushMatrix();
            if (_transform != null)
            { // 自分基準で描画する
                GL.MultMatrix(_transform.localToWorldMatrix);
            }
            // ラインを描画
            GL.Begin(GL.LINES);
            for (int i = 0; i < _lineList.Count; ++i)
            {
                GL.Color(Color.blue);
                Vector3 sttPos = _lineList[i].sttData.pos * _distance;
                Vector3 endPos = _lineList[i].endData.pos * _distance;
                GL.Vertex3(sttPos.x, sttPos.y, sttPos.z);
                GL.Vertex3(endPos.x, endPos.y, endPos.z);
            }
            GL.End();
            // Matrixを復帰
            GL.PopMatrix();
        }

        static public void SetMeshStars(List<HipData> _hipList, Transform _meshTr, Material _meshMat, float _distance)
        {
            if (_meshTr != null)
            {
                List<Vector3> vtcs = new List<Vector3>();
                List<Color> cols = new List<Color>();
                for (int i = 0; i < _hipList.Count; ++i)
                {
                    Vector3 pos = _hipList[i].pos * _distance;
                    Color col = _hipList[i].color; // 色補正
                    col.r *= 4f;
                    col.g *= 2f;
                    col.b *= 3f;
                    vtcs.Add(pos);
                    cols.Add(col);
                    if ((vtcs.Count > 65533) || (i == _hipList.Count - 1))
                    {
                        GameObject meshObj = new GameObject("starMesh", new[] { typeof(MeshFilter), typeof(MeshRenderer) });
                        meshObj.transform.SetParent(_meshTr);
                        meshObj.transform.localPosition = Vector3.zero;
                        meshObj.transform.localRotation = Quaternion.identity;
                        meshObj.transform.localScale = Vector3.one;
                        meshObj.GetComponent<MeshRenderer>().material = _meshMat;
                        MeshFilter mf = meshObj.GetComponent<MeshFilter>();
                        Mesh mesh = CreatePointCloud(vtcs.ToArray(), cols.ToArray());
                        mf.mesh = mesh;
                        vtcs.Clear();
                        cols.Clear();
                    }
                }
            }
        }

        public static Mesh CreatePointCloud(Vector3[] _vertices, Color[] _cvtxCols)
        {
            int vertNum = _vertices.Length;
            int[] incides = new int[vertNum];
            Vector2[] uv = new Vector2[vertNum];
            Color[] colors = new Color[vertNum];
            Vector3[] normals = new Vector3[vertNum];

            for (int ii = 0; ii < _vertices.Length; ++ii)
            {
                incides[ii] = ii;
                uv[ii] = new Vector2(_vertices[ii].x + 0.5f, _vertices[ii].y + 0.5f);
                colors[ii] = _cvtxCols[(_cvtxCols.Length > ii) ? ii : 0];
                normals[ii] = _vertices[ii].normalized;
            }
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices;
            mesh.uv = uv;
            mesh.colors = colors;
            mesh.normals = normals;
            //            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.SetIndices(incides, MeshTopology.Points, 0);
            return mesh;
        }
    }
}