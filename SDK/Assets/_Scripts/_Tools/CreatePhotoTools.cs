using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Common;

namespace MyToolsSetting
{

    public static class CreatePhotoTools
    {
        public static string m_PhotoPath = "C:/wamp64/www/Photo";
        #region
        public static string[] ScreenCaptureAndSave()
        {
            ReadExternalFilesTools.CreateFolder(m_PhotoPath);
            string name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
            Application.CaptureScreenshot(m_PhotoPath + "/" + name);
            MyDebug.Log(m_PhotoPath + "/" + name, MyColor.State.red);
            return new string[2] { m_PhotoPath, name };
        }
        #endregion
        #region 生成二维码
        public static Texture2D GenerateQRImageConstantSize(string content, int width, int height)
        {
                // 编码成color32
                MultiFormatWriter writer = new MultiFormatWriter();
                Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
                hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
                hints.Add(EncodeHintType.MARGIN, 1);
                hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M);
                BitMatrix bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, width, height, hints);
                // 转成texture2d
                int w = bitMatrix.Width;
                int h = bitMatrix.Height;
                Texture2D texture = new Texture2D(w, h);
                for (int x = 0; x < h; x++)
                {
                    for (int y = 0; y < w; y++)
                    {
                        if (bitMatrix[x, y])
                        {
                            //可在此改颜色
                            texture.SetPixel(y, x, Color.black);
                        }
                        else
                        {
                            texture.SetPixel(y, x, Color.white);
                        }
                    }
                }
                texture.Apply();
                return texture;
        }
        #endregion
    }
}

