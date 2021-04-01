/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using UnityEngine;
    using System.Collections;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;
    using System.IO;
    using System;
    using NatSuite.Sharing;

    public class ReplayCam : MonoBehaviour {

        [Header("Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;

        private IMediaRecorder recorder;
        private CameraInput cameraInput;
        private AudioInput audioInput;
        private AudioSource microphoneSource;

        public Canvas canvas;

        private IEnumerator Start () {
            // Start microphone
            microphoneSource = gameObject.AddComponent<AudioSource>();
            microphoneSource.mute =
            microphoneSource.loop = true;
            microphoneSource.bypassEffects =
            microphoneSource.bypassListenerEffects = false;
            microphoneSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
            yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
            microphoneSource.Play();

            // Video Size according to screen

            videoWidth  = Screen.width;
            videoHeight = Screen.height;

        }

        private void OnDestroy () {
            // Stop microphone
            microphoneSource.Stop();
            Microphone.End(null);
        }

        public void StartRecording () {
            // Start recording
            var frameRate = 30;
            var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
            var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
            var clock = new RealtimeClock();
            recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount);
            // Create recording inputs
            cameraInput = new CameraInput(recorder, clock, Camera.main);
            
        }

        public async void StopRecording () {
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            var sharepayload = new SharePayload();
            sharepayload.AddMedia(path);
            await sharepayload.Commit();
        }

        public void TakeScreenShot()
        {
            canvas.enabled = false;
            Debug.Log("Screen Shot Taken");
            StartCoroutine(TakeScreenshotAndSave());
        }
        private IEnumerator TakeScreenshotAndSave()
        {
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();

            try
            {
                //NatShare.Share(ss);

                string filePath = Path.Combine(Application.persistentDataPath, "NFT_" + GalleryHandler.TotalImageInGallery++ + ".png");
                File.WriteAllBytes(filePath, ss.EncodeToPNG());
                Debug.Log("Image Saved at: "+filePath);
                // To avoid memory leaks
                Destroy(ss);
                Handheld.Vibrate();
                //new NativeShare().AddFile(filePath).Share();
            }
            catch (Exception e)
            {
                print(e);
            }

            //NatShare.SaveToCameraRoll(ss, "Santa AR");
            // To avoid memory leaks
            Destroy(ss);
            canvas.enabled = true;

   
        }
    }
}