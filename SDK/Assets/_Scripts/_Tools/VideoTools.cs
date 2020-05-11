using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;
namespace MyToolsSetting
{
    public static class VideoTools
    {
        /// <summary>
        /// 控制视频的音量
        /// </summary>
        /// <param name="media"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        #region
        public static float Video_Volume(MediaPlayer media,float unit)
        {
            if(media)
                media.Control.SetVolume(Mathf.Clamp(media.m_Volume + unit, 0, 1));
            return media.m_Volume;
        }
        #endregion
        /// <summary>
        /// 暂停视频
        /// </summary>
        /// <param name="media"></param>
        #region
        public static void PauseVideo(MediaPlayer media)
        {
            if(media)
                media.Pause();
        }
        #endregion
        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="media"></param>
        #region
        public static void PlayVideo(MediaPlayer media)
        {
            if (media)
                media.Play();
        }
        #endregion
        /// <summary>
        /// 重新播放
        /// </summary>
        /// <param name="media"></param>
        /// <param name="pause">是否播放</param>
        #region
        public static void Replay(MediaPlayer media,bool pause)
        {
            if (media)
                media.Rewind(pause);
        }
        #endregion
        /// <summary>
        /// 是否静音
        /// </summary>
        /// <param name="media"></param>
        /// <param name="isOn"></param>
        #region
        public static void Mute(MediaPlayer media)
        {
            if (media)
                media.Control.MuteAudio(!media.Control.IsMuted());
        }
        #endregion
        /// <summary>
        /// 切换视频
        /// </summary>
        private static int _VideoIndex;
        private static List<string> _Paths = new List<string>();
        private static MediaPlayer.FileLocation _location = MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder;
        #region
        public static void ChangeVideo(MediaPlayer media)
        {
            _Paths = ReadExternalFilesTools.FindAllFiles(Application.streamingAssetsPath, ".mp4");
            media.m_VideoPath =  _Paths[_VideoIndex];
            _VideoIndex = (_VideoIndex + 1) % (_Paths.Count);
            if (string.IsNullOrEmpty(media.m_VideoPath))
            {
                media.CloseVideo();
                _VideoIndex = 0;
            }
            else
            {
                media.OpenVideoFromFile(_location, media.m_VideoPath,true);
            }
        }
        public static void ChangeVideo(MediaPlayer media ,string name)
        {
            _Paths = ReadExternalFilesTools.FindAllFiles(Application.streamingAssetsPath, ".mp4");
            for (int i = 0; i < _Paths.Count; i++)
            {
                if (_Paths[i].Split('\\')[_Paths[i].Split('\\').Length-1].Replace(".mp4", "").Equals(name))
                {
                    media.m_VideoPath = _Paths[i];
                    if (string.IsNullOrEmpty(media.m_VideoPath))
                    {
                        media.CloseVideo();
                        _VideoIndex = 0;
                    }
                    else
                    {
                        Debug.Log(_Paths[i]);
                        media.OpenVideoFromFile(_location, media.m_VideoPath, true);
                    }
                }
            }
        }
        public static void ChangeVideo(MediaPlayer media,bool isOn)
        {
            _Paths = ReadExternalFilesTools.FindAllFiles(Application.streamingAssetsPath, ".mp4");
            media.m_VideoPath = _Paths[_VideoIndex];
            _VideoIndex = (_VideoIndex + 1) % (_Paths.Count);
            if (string.IsNullOrEmpty(media.m_VideoPath))
            {
                media.CloseVideo();
                _VideoIndex = 0;
            }
            else
            {
                media.OpenVideoFromFile(_location, media.m_VideoPath, isOn);
            }
        }
        public static void ChangeVideo(MediaPlayer media, string name,bool isOn)
        {
            _Paths = ReadExternalFilesTools.FindAllFiles(Application.streamingAssetsPath, ".mp4");
            for (int i = 0; i < _Paths.Count; i++)
            {
                if (_Paths[i].Split('\\')[_Paths[i].Split('\\').Length - 1].Replace(".mp4", "").Equals(name))
                {
                    media.m_VideoPath = _Paths[i];
                    if (string.IsNullOrEmpty(media.m_VideoPath))
                    {
                        media.CloseVideo();
                        _VideoIndex = 0;
                    }
                    else
                    {
                        Debug.Log(_Paths[i]);
                        media.OpenVideoFromFile(_location, media.m_VideoPath, isOn);
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// 调节视频进度
        /// </summary>
        /// <param name="media"></param>
        /// <param name="slider"></param>
        /// <param name="_setVideoSeekSliderValue"></param>
        #region
        public static void OnVideoSeekSlider(MediaPlayer media ,Slider slider,float _setVideoSeekSliderValue)
        {
            if (media && slider && slider.value != _setVideoSeekSliderValue)
            {
                media.Control.Seek(slider.value * media.Info.GetDurationMs());
            }
        }
        #endregion
    }
}

