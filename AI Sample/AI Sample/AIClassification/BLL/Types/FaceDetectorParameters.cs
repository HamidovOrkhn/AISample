using Accord.Vision.Detection;

namespace AI_Sample.AIClassification.BLL.Types
{
    internal class FaceDetectorParameters
    {
        public float ScalingFactor { get; private set; }
        public ObjectDetectorScalingMode ScalingMode { get; private set; }
        public ObjectDetectorSearchMode SearchMode { get; private set; }
        public bool UseParallelProcessing { get; private set; }
        public int MinimumSize { get; private set; }

        public bool IsValid { get; private set; }

        private FaceDetectorParameters(float scalingFactor, int minimumSize, ObjectDetectorScalingMode objectDetectorScalingMode,
            ObjectDetectorSearchMode objectDetectorSearchMode, bool useParallelProcessing, bool isValid)
        {
            ScalingFactor = scalingFactor;
            MinimumSize = minimumSize;
            ScalingMode = objectDetectorScalingMode;
            SearchMode = objectDetectorSearchMode;
            UseParallelProcessing = useParallelProcessing;
            IsValid = isValid;
        }

        public static FaceDetectorParameters Create(float scalingFactor, int minimumSize, ObjectDetectorScalingMode objectDetectorScalingMode,
            ObjectDetectorSearchMode objectDetectorSearchMode, bool useParallelProcessing) =>
                new FaceDetectorParameters(scalingFactor, minimumSize, objectDetectorScalingMode, objectDetectorSearchMode, useParallelProcessing, true);
    }
}