using Cinemachine;
namespace BB
{
	public static class CinemachineVirtualCameraExtensions
    {
        public static float GetFov(this CinemachineVirtualCamera camera)
            => camera.m_Lens.FieldOfView;
        public static void SetFov(this CinemachineVirtualCamera camera, float value)
        {
            var lens = camera.m_Lens;
            lens.FieldOfView = value;
            camera.m_Lens = lens;
        }
    }
}