using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildleEditor
{

    public static string ABCONFIGPATH = "Assets/Editor/ABConfig.asset";

    //key:ab包名，value:路径。所有文件夹ab包dic
    public static Dictionary<string,string> m_AllFileDir=new Dictionary<string, string>();

    //过滤的list
    public static List<string> m_AllFileAB=new List<string>();


    [MenuItem("Tools/打包")]
    public static void Build()
    {
        m_AllFileDir.Clear();
        ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);

        foreach (ABConfig.FileDirABName fileDir in abConfig.m_AllFileDirAB)
        {
            if (m_AllFileDir.ContainsKey(fileDir.ABName))
            {
                Debug.LogError("AB包配置名字重复，请检查!");
            }
            else
            {
                m_AllFileDir.Add(fileDir.ABName,fileDir.Path);
                m_AllFileAB.Add(fileDir.Path);
            }
        }

        string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.m_AllPrefabPath.ToArray());
        for (int i = 0; i < allStr.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
            EditorUtility.DisplayProgressBar("查找Prefab","Prefab:"+path,i*1.0f/allStr.Length);

            if (!ContainAllFileAB(path))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                //获取所有与他有依赖关系的物体
                string[] allDepend = AssetDatabase.GetDependencies(path);

                List<string> allDependPath=new List<string>();
                

                for (int j = 0; j < allDepend.Length; j++)
                {
                    Debug.Log(allDepend[j]);
                    if (!ContainAllFileAB(allDepend[j])&&!allDepend[j].EndsWith(".cs"))
                    {
                        
                    }
                }
            }
        }
        EditorUtility.ClearProgressBar();

    }



    static bool ContainAllFileAB(string path)
    {
        for (int i = 0; i < m_AllFileAB.Count; i++)
        {
            if (path == m_AllFileAB[i] || path.Contains(m_AllFileAB[i]))
            {
                
                return true;
            }
        }
        return false;
    }
}
