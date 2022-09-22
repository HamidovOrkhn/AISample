using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using AI_Sample.AIClassification.BLL.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AI_Sample.AIClassification.BLL
{
    internal class FaceDetector
    {
        private HaarObjectDetector _detector;

        public FaceDetector()
        {
            _detector = new HaarObjectDetector(new FaceHaarCascade());
        }

        internal IEnumerable<Face> ExtractFaces(Bitmap picture, FaceDetectorParameters faceDetectorParameters) =>
            picture == null ?
            Enumerable.Empty<Face>() :
            ProcessFrame(picture, faceDetectorParameters).Select(rec => new Face(rec));

        private IEnumerable<Rectangle> ProcessFrame(Bitmap picture, FaceDetectorParameters faceDetectorParameters)
        {
            _detector.MinSize = new Size(faceDetectorParameters.MinimumSize, faceDetectorParameters.MinimumSize);
            _detector.ScalingFactor = faceDetectorParameters.ScalingFactor;
            _detector.ScalingMode = faceDetectorParameters.ScalingMode;
            _detector.SearchMode = faceDetectorParameters.SearchMode;
            _detector.UseParallelProcessing = faceDetectorParameters.UseParallelProcessing;
            _detector.MaxSize = new Size(600, 600);
            _detector.Suppression = 1;
            return _detector.ProcessFrame(picture);
        }
    }
}