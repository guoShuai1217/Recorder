/*
 * 		Description: 录制视频
 *
 *  	CreatedBy:  国帅
 *
 *  	DataTime: 2018.12.20
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class RecorderRoot : MonoBehaviour 
{

    public AVProMovieCaptureBase movieCapture; // 录屏对象 (在摄像机上)

    public Text txt_FileLength; // 文件时长

    public Text txt_FileSize; // 文件大小

    private uint seconds;
    private uint minutes;
    private long fileSize; // 录制文件大小

    private string filePath; // 路径
    private string fileName; // 文件名

    private void Start()
    {
        Button[] bts = GetComponentsInChildren<Button>();
        for (int i = 0; i < bts.Length; i++)
        {
            BtnArgs args = new BtnArgs(i, bts[i].gameObject);
            bts[i].onClick.AddListener(() => { OnClickBtn(args); });
        }
    }

  

    private void Update()
    {
        if (movieCapture.IsCapturing())
        {
            seconds = movieCapture.TotalEncodedSeconds;//获取录制时长
            minutes = seconds / 60;
            seconds = seconds % 60;
            txt_FileLength.text = "录制时长：    " + minutes + " : " + seconds + " 秒";
                    
            fileSize = movieCapture.GetCaptureFileSize();//获取录制文件大小
            float FileSize = ((float)fileSize / (1024f * 1024f));
            txt_FileSize.text = "文件大小:      " + FileSize.ToString("F1") + " MB";
        }
    }

    private void OnClickBtn(BtnArgs args)
    {
        switch (args.Id)
        {
            case 0: // 播放
                OnClickBtn_Recorde();
                break;
            case 1: // 停止             
                movieCapture.StopCapture();
                txt_FileSize.text = string.Empty;
                txt_FileLength.text = string.Empty;
                // TODO 提示文件保存在哪里
                Debug.Log("文件已保存至" + filePath );
                break;
            case 2: // 暂停
                movieCapture.PauseCapture();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 点击开始播放
    /// </summary>
    private void OnClickBtn_Recorde()
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "AVI文件(*.avi)\0*.avi";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = UnityEngine.Application.dataPath.Replace('/', '\\');//默认路径
        openFileName.title = "选择文件存放位置";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetSaveFileName(openFileName))
        {
            filePath = openFileName.file.Replace(openFileName.fileTitle, null);
            movieCapture._outputFolderPath = filePath;
            movieCapture._autoFilenamePrefix = openFileName.fileTitle;
            fileName = movieCapture._autoFilenamePrefix;
            Debug.Log(filePath + fileName);
            movieCapture.StartCapture();
            
        }
    }



}

public class BtnArgs
{
    public int Id;
    public GameObject obj;
    public BtnArgs(int id,GameObject obj)
    {
        this.Id = id;
        this.obj = obj;
    }
}